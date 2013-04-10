using Renren.Components.Network;
using Renren.Components.Network.Http;
using Renren.Components.RestApis.Entities;
using Renren.Components.RestApis.Shared;
using Renren.Components.Shared;
using Renren.Components.Tools;
using System;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public partial class Feed
    {
        public async static Task<INetworkAsyncToken> Get<TEntity>(
            Action<INetworkAsyncToken> respHandler, object owner,
            int page = -1, int pageSize = -1, int userId = -1) where TEntity : class
        {
            Guarder.NotNull(SocialSDKContext.LoginContext, "You should always login firstly!");

            var request = HttpRequestMessage.DefaultPost.ToRenrenStyle();
            request.QueryPairs.Add("api_key", SocialSDKContext.ApiKey);
            request.QueryPairs.Add("v", SocialSDKContext.ApiVersion);
            request.QueryPairs.Add("call_id", DateTime.Now.ToUnixUtcTime());
            request.QueryPairs.Add("session_key", SocialSDKContext.LoginContext.SessionKey);
            request.QueryPairs.Add("type", "102,103,104,110,502,504,601,701,709,107,2002,2003,2004,2005,2006,2008,2009,2012,2013,1201,8007,1101");
            request.QueryPairs.Add("has_at_id", "1");
            request.QueryPairs.Add("head_url_switch", "2");

            if (page != -1)
                request.QueryPairs.Add("page", page.ToString());
            if (pageSize != -1)
                request.QueryPairs.Add("page_size", pageSize.ToString());
            if (userId != -1)
                request.QueryPairs.Add("uid", userId.ToString());

            request.QueryPairs.Add("gz", "compression");
            request.QueryPairs.Add("sig", request.ToRenrenSig(SocialSDKContext.LoginContext.SecretKey));
            request.Target = new Uri(SocialSDKContext.RestApiBaseAddress + "feed/get", UriKind.Absolute);

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
