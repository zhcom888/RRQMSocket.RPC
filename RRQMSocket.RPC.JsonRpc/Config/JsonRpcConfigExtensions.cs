using RRQMCore.Dependency;
using RRQMSocket.RPC.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket
{
    /// <summary>
    /// JsonRpcConfigExtensions
    /// </summary>
    public static class JsonRpcConfigExtensions
    {
        /// <summary>
        /// 构建JsonRpc类客户端，并连接
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TClient BuildWithJsonRpcClient<TClient>(this RRQMConfig config) where TClient : IJsonRpcClient
        {
            TClient client = config.Container.Resolve<TClient>();
            client.Setup(config);
            client.Connect();
            return client;
        }

        /// <summary>
        /// 构建JsonRpc类客户端，并连接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static JsonRpcClient BuildWithJsonRpcClient(this RRQMConfig config)
        {
            return BuildWithJsonRpcClient<JsonRpcClient>(config);
        }

        /// <summary>
        /// TcpJsonRpc
        /// </summary>
        public static Protocol TcpJsonRpc { get; private set; } = new Protocol("TcpJsonRpc");

        /// <summary>
        /// 转化Protocol协议标识
        /// </summary>
        /// <param name="client"></param>
        public static void SwitchProtocolToTcpJsonRpc(this ITcpClientBase client)
        {
            client.Protocol = TcpJsonRpc;
        }

        /// <summary>
        /// 设置JsonRpc的协议。
        /// </summary>
        public static readonly DependencyProperty JRPTProperty =
            DependencyProperty.Register("JRPT", typeof(JRPT), typeof(JsonRpcConfigExtensions),JRPT.Tcp);

        /// <summary>
        /// 设置JsonRpc的协议。默认为<see cref="JRPT.Tcp"/>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RRQMConfig SetJRPT(this RRQMConfig config,JRPT value)
        {
            config.SetValue(JRPTProperty,value);
            return config;
        }
    }
}
