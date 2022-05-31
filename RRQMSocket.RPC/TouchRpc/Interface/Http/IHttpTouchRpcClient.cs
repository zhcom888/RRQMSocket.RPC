using RRQMSocket.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// IHttpRpcClient
    /// </summary>
    public interface IHttpTouchRpcClient :IHttpClient, IHttpRpcClientBase, IRpcParser
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        ITcpClient Connect(CancellationToken token = default, int timeout = 5000);

        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ITcpClient> ConnectAsync(CancellationToken token = default, int timeout = 5000);
    }

    /// <summary>
    /// IHttpRpcClientBase
    /// </summary>
    public interface IHttpRpcClientBase : IHttpClientBase, ITouchRpc
    {
    }
}
