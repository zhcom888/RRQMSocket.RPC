using RRQMCore.ByteManager;
using RRQMCore.Log;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// RpcActorBase
    /// </summary>
    public interface IRpcActorBase
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        ILog Logger { get; }

        /// <summary>
        /// Ping
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool Ping(int timeout = 5000);

        /// <summary>
        /// 序列化选择器
        /// </summary>
        SerializationSelector SerializationSelector { get; }

        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        void Send(short protocol, byte[] buffer);

        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        void Send(short protocol, byte[] buffer, int offset, int length);

        /// <summary>
        /// 发送协议流
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dataByteBlock"></param>
        void Send(short protocol, ByteBlock dataByteBlock);

        /// <summary>
        /// 发送协议状态
        /// </summary>
        /// <param name="protocol"></param>
        void Send(short protocol);

        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        void SendAsync(short protocol, byte[] buffer);

        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        void SendAsync(short protocol, byte[] buffer, int offset, int length);

        /// <summary>
        /// 发送协议流
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dataByteBlock"></param>
        void SendAsync(short protocol, ByteBlock dataByteBlock);

        /// <summary>
        /// 发送协议状态
        /// </summary>
        /// <param name="protocol"></param>
        void SendAsync(short protocol);
    }
}
