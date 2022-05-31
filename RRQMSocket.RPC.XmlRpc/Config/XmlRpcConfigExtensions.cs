using RRQMSocket.RPC.XmlRpc;

namespace RRQMSocket
{
    /// <summary>
    /// XmlRpcConfigExtensions
    /// </summary>
    public static class XmlRpcConfigExtensions
    {
        /// <summary>
        /// 构建XmlRpcClient类客户端，并连接
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TClient BuildWithXmlRpcClient<TClient>(this RRQMConfig config) where TClient : IXmlRpcClient
        {
            TClient client = config.Container.Resolve<TClient>();
            client.Setup(config);
            client.Connect();
            return client;
        }

        /// <summary>
        /// 构建XmlRpcClient类客户端，并连接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static XmlRpcClient BuildWithXmlRpcClient(this RRQMConfig config)
        {
            return BuildWithXmlRpcClient<XmlRpcClient>(config);
        }
    }
}
