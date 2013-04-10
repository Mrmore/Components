using Renren.Components.Network;
using Renren.Components.Network.Http;
using Renren.Components.RestApis.Entities;
using Renren.Components.RestApis.Shared;
using Renren.Components.Tools;
using Renren.Components.Shared;
using System;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public partial class Client
    {
        public async static Task<INetworkAsyncToken> LogIn<TEntity>(string username, string password,
            Action<INetworkAsyncToken> respHandler, object owner,
            string verifycode = null, int isverify = -1) where TEntity : class
        {
            password = password.ToMD5String().ToLower();

            var request = HttpRequestMessage.DefaultPost.ToRenrenStyle();
            request.QueryPairs.Add("api_key", SocialSDKContext.ApiKey);
            request.QueryPairs.Add("uniq_id", SocialSDKContext.UDID);
            request.QueryPairs.Add("v", SocialSDKContext.ApiVersion);
            request.QueryPairs.Add("call_id", DateTime.Now.ToUnixUtcTime());
            request.QueryPairs.Add("password", password);
            request.QueryPairs.Add("user", username);

            if (isverify != -1)
            {
                isverify = 1;
                request.QueryPairs.Add("isverify", isverify.ToString());
            }
            if (!string.IsNullOrEmpty(verifycode))
            {
                request.QueryPairs.Add("verifycode", verifycode);
            }

            request.QueryPairs.Add("gz", "compression");
            request.QueryPairs.Add("sig", request.ToRenrenSig(SocialSDKContext.SecretKey));
            request.Target = new Uri(SocialSDKContext.RestApiBaseAddress + "client/login", UriKind.Absolute);

            var http = await HttpTransferEngine<HttpTransition>.CreateInitedInstance();
            INetworkAsyncToken token = 
                http.CreateToken<RenrenRespMessage<TEntity, RenrenRemoteError>>(
                respHandler,
                owner, request);

            http.SendToken(token);

            return token;
        }
    }
}