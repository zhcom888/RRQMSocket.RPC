//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// 表示即将握手
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <param name="client"></param>
    /// <param name="e"></param>
    public delegate void HandshakingEventHandler<TClient>(TClient client, VerifyOptionEventArgs e) where TClient : IRpcActor;

    /// <summary>
    /// 传输文件操作处理
    /// </summary>
    /// <param name="client"></param>
    /// <param name="e"></param>
    public delegate void RRQMFileOperationEventHandler<TClient>(TClient client, FileOperationEventArgs e);

    /// <summary>
    /// 协议数据
    /// </summary>
    /// <param name="client"></param>
    /// <param name="protocol"></param>
    /// <param name="byteBlock"></param>
    public delegate void RRQMProtocolReceivedEventHandler<TClient>(TClient client, short protocol, ByteBlock byteBlock) where TClient : IRpcActor;

    /// <summary>
    /// 收到流操作
    /// </summary>
    /// <param name="client"></param>
    /// <param name="e"></param>
    public delegate void RRQMStreamOperationEventHandler<TClient>(TClient client, StreamOperationEventArgs e) where TClient : IRpcActor;

    /// <summary>
    /// 流状态
    /// </summary>
    /// <param name="client"></param>
    /// <param name="e"></param>
    public delegate void RRQMStreamStatusEventHandler<TClient>(TClient client, StreamStatusEventArgs e) where TClient : IRpcActor;

    /// <summary>
    /// 传输文件消息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="e"></param>
    public delegate void RRQMTransferFileEventHandler<TClient>(TClient client, FileTransferStatusEventArgs e);
}