using Renren.Components.Network;
using Renren.Components.Network.Http;
using Renren.Components.RestApis.Entities;
using Renren.Components.RestApis.Shared;
using Renren.Components.Tools;
using Renren.Components.Shared;
using Renren.Components.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Renren.Components.RestApis
{
    public partial class Photo
    {
        public async static Task<INetworkAsyncToken> UploadBin<TEntity>(
            Action<INetworkAsyncToken> respHandler, object owner, IProgressIndicator<int> indicator,
           StorageFile file, long albumId, string caption, int default_album_switch = 1) where TEntity : class
        {
            Guarder.NotNull(SocialSDKContext.LoginContext, "You should always login firstly!");

            var request = HttpRequestMessage.DefaultMultiPart;
            request.QueryPairs.Add("api_key", SocialSDKContext.ApiKey);
            request.QueryPairs.Add("v", SocialSDKContext.ApiVersion);
            request.QueryPairs.Add("call_id", DateTime.Now.ToUnixUtcTime());
            request.QueryPairs.Add("session_key", SocialSDKContext.LoginContext.SessionKey);

            if (albumId != -1)
            {
                request.QueryPairs.Add("aid", albumId.ToString());
            }
            if (default_album_switch != -1)
            {
                request.QueryPairs.Add("default_album_switch", default_album_switch.ToString());
            }
            if (!string.IsNullOrEmpty(caption))
            {
                request.QueryPairs.Add("caption", caption);
            }

            request.QueryPairs.Add("gz", "compression");
            request.QueryPairs.Add("sig", request.ToRenrenSig(SocialSDKContext.LoginContext.SecretKey));
            request.Target = new Uri(SocialSDKContext.RestApiBaseAddress + "photos/uploadbin", UriKind.Absolute);

            var stream = await file.OpenReadAsync();
            {
                var managed = stream.AsStreamForRead();

                request.Formdata.Itmes.Add(new FormdataItem() {
                    ContentType = file.Name.GetMimeContentTypeFromFileName(),
                    Data = managed,
                    FileName = file.Name,
                    Name = "data"
                });
            }

            var http = await HttpTransferEngine<HttpTransition>.CreateInitedInstance();
            INetworkAsyncToken token =
                http.CreateToken<RenrenRespMessage<TEntity, RenrenRemoteError>>(
                respHandler,
                owner, request, null, null, indicator);

            http.SendToken(token);

            return token;
        }
    }
}
