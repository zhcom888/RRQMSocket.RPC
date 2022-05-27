using RRQMSocket.RPC.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket
{
    /// <summary>
    /// WebApiConfigExtensions
    /// </summary>
    public static class WebApiConfigExtensions
    {
        /// <summary>
        /// 构建WebApiClient类客户端，并连接
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TClient BuildWithWebApiClient<TClient>(this RRQMConfig config) where TClient : IWebApiClient
        {
            TClient client = config.Container.Resolve<TClient>();
            client.Setup(config);
            client.Connect();
            return client;
        }

        /// <summary>
        ///  构建WebApiClient类客户端，并连接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static WebApiClient BuildWithWebApiClient(this RRQMConfig config)
        {
            return BuildWithWebApiClient<WebApiClient>(config);
        }
    }
}
