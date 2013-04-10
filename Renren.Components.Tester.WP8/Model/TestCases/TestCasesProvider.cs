using GalaSoft.MvvmLight.Threading;
using Renren.Components.Network;
using Renren.Components.RestApis;
using Renren.Components.RestApis.Entities;
using Renren.Components.Network.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if NETFX_CORE
using Renren.Components.Async;
using Windows.UI.Popups;
#endif


namespace Renren.Components.Tester.Model.TestCases
{
    public class TestCasesProvider : ITestCasesProvider
    {
        public void GetData(Action<TestCasesModel, Exception> callback)
        {
            var item = new TestCasesModel("Renren Comp. Test Cases");
            int count = 0;
            item.Cases.Add(new TestCase()
            {
                Category = "Rest Api",
                Group = "Client",
                Description = "Test the client.login rest api",
                NeedJumpPage = false,
                Id = count++,
                RunAction = (c, o) =>
                    {
                        var loginToken = RestApis.Client.LogIn<LoginInfo>(
                            "51171041@qq.com", "81036198",
                            (t) =>
                            {
                                if (t.Status == NetworkStatus.Completed &&
                                    t.Response.Status == RespStatus.Succeed)
                                {
                                    var resp = t.Response as RenrenRespMessage<LoginInfo, RenrenRemoteError>;
                                    SocialSDKContext.LoginContext = resp.Result;
                                }
                                reportResult(c, t);
                            }, o);
                    }
            });

            item.Cases.Add(new TestCase()
            {
                Category = "Rest Api",
                Group = "Feed",
                Description = "Test the feed.get rest api",
                NeedJumpPage = false,
                Id = count++,
                RunAction = (c, o) =>
                {
                    var feedsToken = RestApis.Feed.Get<FeedList>(
                        (t) =>
                        {
                            reportResult(c, t);
                        }, o);
                }
            });

            item.Cases.Add(new TestCase()
            {
                Category = "Http test",
                Group = "Get",
                Description = "Test the http get method",
                NeedJumpPage = false,
                Id = count++,
                RunAction = async (c, o) =>
                {
                    var request = HttpRequestMessage.DefaultGet;
                    request.Target = new Uri("http://www.renren.com");

                    var http = await HttpTransferEngine<HttpTransition>.CreateInitedInstance();
                    var token =
                        http.CreateToken<NetworkRespMessage>(
                        (t) => reportResult(c, t),
                        o, request);

                    http.SendToken(token);
                }
            });

            item.Cases.Add(new TestCase()
            {
                Category = "Multi-Media",
                Group = "Audio UGC",
                Description = "Enum audio ugc and play",
                NeedJumpPage = true,
                JumpPageType = Type.GetType("Renren.Components.Tester.Win8.Views.AudioUGCPage"),
                JumpPageUri = "/Views/AudioUGCPage.xaml",
                Id = count++,
                RunAction = null
            });

            item.Cases.Add(new TestCase()
            {
                Category = "Multi-Media",
                Group = "Upload",
                Description = "Loading files using network component",
                NeedJumpPage = true,
                JumpPageType = Type.GetType("Renren.Components.Tester.Win8.Views.MultiUploadPage"),
                JumpPageUri = "/Views/MultiUploadPage.xaml",
                Id = count++,
                RunAction = null
            });

            //item.Cases.Add(new TestCase()
            //{
            //    Category = "Cache",
            //    Group = "Cache",
            //    Description = "Cache Tester",
            //    NeedJumpPage = true,
            //    JumpPageType = Type.GetType("Renren.Components.Tester.Win8.Views.CacheTestPage"),
            //    JumpPageUri = "/Views/CacheTestPage.xaml",
            //    Id = count++,
            //    RunAction = null
            //});

            item.Cases.Add(new TestCase()
            {
                Category = "Cache",
                Group = "Cache",
                Description = "Cache Demo",
                NeedJumpPage = true,
                JumpPageType = Type.GetType("Renren.Components.Tester.Win8.Views.CachePage"),
                JumpPageUri = "/Views/CachePage.xaml",
                Id = count++,
                RunAction = null
            });

            item.Cases.Add(new TestCase()
            {
                Category = "Image Tools",
                Group = "Images",
                Description = "Have a look at what the ImageTools provide.",
                NeedJumpPage = true,
                JumpPageType = Type.GetType("Renren.Components.Tester.Win8.Views.ImageTools.ImageToolsPage"),
                JumpPageUri = "/Views/ImageTools/ImageToolsPage.xaml",
                Id = count++,
                RunAction = null
            });            
            callback(item, null);
        }

#if NETFX_CORE
        AsyncLock locker = new AsyncLock();
#endif
        private void reportResult(TestCase c, INetworkAsyncToken t)
        {
            DispatcherHelper.RunAsync( async() =>
            {
                if (t.Status == Network.NetworkStatus.Completed)
                {
#if NETFX_CORE
                    using (var release = await locker.LockAsync())
                    //lock (this)
                    {
                        MessageDialog dialog = new MessageDialog(t.Response.Prompts);
                        await dialog.ShowAsync();
                    }
#else
                    MessageBox.Show(t.Response.Prompts);
#endif
                    c.Result = true;
                }
                else
                {
                    c.Result = false;
                }
            });
        }
    }
}
