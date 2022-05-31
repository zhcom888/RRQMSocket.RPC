namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// ITouchRpcPlugin
    /// </summary>
    public interface ITouchRpcPlugin : ITcpPlugin
    {
        /// <summary>
        /// 在文件传输即将进行时触发。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnFileTransfering(ITouchRpc client, FileOperationEventArgs e);

        /// <summary>
        /// 当文件传输结束之后。并不意味着完成传输，请通过<see cref="FileTransferStatusEventArgs.Result"/>属性值进行判断。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnFileTransfered(ITouchRpc client, FileTransferStatusEventArgs e);

        /// <summary>
        /// 收到协议数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnReceivedProtocolData(ITouchRpc client, ProtocolDataEventArgs e);

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnStreamTransfering(ITouchRpc client, StreamOperationEventArgs e);

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。
        /// 当流数据传输结束之后。并不意味着完成传输，请通过<see cref="StreamStatusEventArgs.Result"/>属性值进行判断。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnStreamTransfered(ITouchRpc client, StreamStatusEventArgs e);

        /// <summary>
        /// 在完成握手连接时。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void OnHandshaked(ITouchRpc client, MesEventArgs e);

        /// <summary>
        /// 在验证Token时
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        void OnHandshaking(ITouchRpc client, VerifyOptionEventArgs e);
    }
}
