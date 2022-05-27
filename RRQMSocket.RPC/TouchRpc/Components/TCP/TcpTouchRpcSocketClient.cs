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
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Log;
using RRQMCore.Run;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// Rpc服务器辅助类
    /// </summary>
    public class TcpTouchRpcSocketClient : SocketClient, ITcpRpcClientBase
    {
        internal RpcActor m_rpcActor;

        private void InitRpcActor()
        {
            this.SwitchProtocolToTouchRpc();
            this.m_rpcActor = new RpcActor(true, this.Container.Resolve<ILog>());
            this.m_rpcActor.Caller = this;
            this.m_rpcActor.OnHandshaking = this.OnHandshaking;
            this.m_rpcActor.OnHandshaked = this.OnHandshaked;
            this.m_rpcActor.OutputSend = this.RpcActorSend;
            this.m_rpcActor.OnResetID = (w) => { base.ResetID(w.NewID); };
            this.m_rpcActor.OnReceived = this.OnReceived;
            this.m_rpcActor.OnClose = this.Close;
            this.m_rpcActor.OnFindRpcActor = this.FindRpcActor;
            this.m_rpcActor.OnStreamTransfering = this.OnStreamTransfering;
            this.m_rpcActor.OnStreamTransfered = this.OnStreamTransfered;
            this.m_rpcActor.OnFileTransfering = this.OnFileTransfering;
            this.m_rpcActor.OnFileTransfered = this.OnFileTransfered;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Func<IRpcClient, bool> TryCanInvoke { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsHandshaked => this.m_rpcActor == null ? false : this.m_rpcActor.IsHandshaked;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ResponseType ResponseType { get => this.m_rpcActor.ResponseType; set => this.m_rpcActor.ResponseType = value; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string RootPath { get => this.m_rpcActor.RootPath; set => this.m_rpcActor.RootPath = value; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SerializationSelector SerializationSelector => this.m_rpcActor.SerializationSelector;

        /// <summary>
        /// 验证超时时间,默认为3000ms
        /// </summary>
        public int VerifyTimeout
        {
            get => this.Config.GetValue<int>(TouchRpcConfigExtensions.VerifyTimeoutProperty);
        }

        /// <summary>
        /// 连接令箭
        /// </summary>
        public string VerifyToken
        {
            get => this.Config.GetValue<string>(TouchRpcConfigExtensions.VerifyTokenProperty);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChannelExisted(int id)
        {
            return this.m_rpcActor.ChannelExisted(id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public Channel CreateChannel()
        {
            return this.m_rpcActor.CreateChannel();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel CreateChannel(int id)
        {
            return this.m_rpcActor.CreateChannel(id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public Channel CreateChannel(string clientID)
        {
            return this.m_rpcActor.CreateChannel(clientID);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel CreateChannel(string clientID, int id)
        {
            return this.m_rpcActor.CreateChannel(clientID, id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        public void Invoke(string method, IInvokeOption invokeOption, params object[] parameters)
        {
            this.m_rpcActor.Invoke(method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Invoke<T>(string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.Invoke<T>(method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public T Invoke<T>(string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types)
        {
            return this.m_rpcActor.Invoke<T>(method, invokeOption, ref parameters, types);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        public void Invoke(string targetID, string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types)
        {
            this.m_rpcActor.Invoke(targetID, method, invokeOption, ref parameters, types);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetID"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public T Invoke<T>(string targetID, string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types)
        {
            return this.m_rpcActor.Invoke<T>(targetID, method, invokeOption, ref parameters, types);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        public void Invoke(string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types)
        {
            this.m_rpcActor.Invoke(method, invokeOption, ref parameters, types);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        public void Invoke(string id, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            this.m_rpcActor.Invoke(id, method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Invoke<T>(string id, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.Invoke<T>(id, method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task InvokeAsync(string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.InvokeAsync(method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<T> InvokeAsync<T>(string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.InvokeAsync<T>(method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task InvokeAsync(string id, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.InvokeAsync(id, method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<T> InvokeAsync<T>(string id, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            return this.m_rpcActor.InvokeAsync<T>(id, method, invokeOption, parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Ping(int timeout = 5000)
        {
            return this.m_rpcActor.Ping(timeout);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PullFile(FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PullFile(fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PullFile(string clientID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PullFile(clientID, fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PullFileAsync(FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PullFileAsync(fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PullFileAsync(string clientID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PullFileAsync(clientID, fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PushFile(FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PushFile(fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PushFile(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PushFile(targetID, fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PushFileAsync(FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PushFileAsync(fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PushFileAsync(string clientID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PushFileAsync(clientID, fileRequest, fileOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="newID"></param>
        public override void ResetID(string newID)
        {
            this.ResetID(newID, default);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="newID"></param>
        /// <param name="cancellationToken"></param>
        public void ResetID(string newID, CancellationToken cancellationToken = default)
        {
            this.m_rpcActor.ResetID(newID, cancellationToken);
            base.ResetID(newID);
        }

        #region 发送
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        public void Send(short protocol, byte[] buffer)
        {
            this.m_rpcActor.Send(protocol, buffer);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void Send(short protocol, byte[] buffer, int offset, int length)
        {
            this.m_rpcActor.Send(protocol, buffer, offset, length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dataByteBlock"></param>
        public void Send(short protocol, ByteBlock dataByteBlock)
        {
            this.m_rpcActor.Send(protocol, dataByteBlock);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        public void Send(short protocol)
        {
            this.m_rpcActor.Send(protocol);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        public void SendAsync(short protocol, byte[] buffer)
        {
            this.m_rpcActor.SendAsync(protocol, buffer);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void SendAsync(short protocol, byte[] buffer, int offset, int length)
        {
            this.m_rpcActor.SendAsync(protocol, buffer, offset, length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dataByteBlock"></param>
        public void SendAsync(short protocol, ByteBlock dataByteBlock)
        {
            this.m_rpcActor.SendAsync(protocol, dataByteBlock);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="protocol"></param>
        public void SendAsync(short protocol)
        {
            this.m_rpcActor.SendAsync(protocol);
        }

        /// <summary>
        /// 不允许直接发送
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public override void Send(byte[] buffer, int offset, int length)
        {
            throw new RRQMException("不允许直接发送，请指定任意大于0的协议，然后发送。");
        }

        /// <summary>
        /// 不允许直接发送
        /// </summary>
        /// <param name="transferBytes"></param>
        public override void Send(IList<TransferByte> transferBytes)
        {
            throw new RRQMException("不允许直接发送，请指定任意大于0的协议，然后发送。");
        }

        /// <summary>
        /// 不允许直接发送
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public override void SendAsync(byte[] buffer, int offset, int length)
        {
            throw new RRQMException("不允许直接发送，请指定任意大于0的协议，然后发送。");
        }

        /// <summary>
        /// 不允许直接发送
        /// </summary>
        /// <param name="transferBytes"></param>
        public override void SendAsync(IList<TransferByte> transferBytes)
        {
            throw new RRQMException("不允许直接发送，请指定任意大于0的协议，然后发送。");
        }
        #endregion 发送


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="streamOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result SendStream(Stream stream, StreamOperator streamOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.SendStream(stream, streamOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="streamOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> SendStreamAsync(Stream stream, StreamOperator streamOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.SendStreamAsync(stream, streamOperator, metadata);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool TrySubscribeChannel(int id, out Channel channel)
        {
            return this.m_rpcActor.TrySubscribeChannel(id, out channel);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this.m_rpcActor.SafeDispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            this.m_rpcActor.InputReceivedData(byteBlock);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConnected(RRQMEventArgs e)
        {
            EasyAction.DelayRun(this.VerifyTimeout, this, (client) =>
             {
                 if (!client.IsHandshaked)
                 {
                     client.Close("验证超时");
                 }
             });
            this.m_rpcActor.ID = this.ID;
            base.OnConnected(e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConnecting(ClientOperationEventArgs e)
        {
            this.InitRpcActor();
            this.SetDataHandlingAdapter(new FixedHeaderPackageAdapter());
            base.OnConnecting(e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDisconnected(ClientDisconnectedEventArgs e)
        {
            this.m_rpcActor.Close(e.Message);
            base.OnDisconnected(e);
        }

        private RpcActor FindRpcActor(string targetID)
        {
            try
            {
                if (this.Service.SocketClients.TryGetSocketClient(targetID, out TcpTouchRpcSocketClient socketClient))
                {
                    return socketClient.m_rpcActor;
                }
            }
            catch
            {
            }
            return null;
        }

        #region 内部委托

        internal Action<TcpTouchRpcSocketClient, MesEventArgs> internalOnHandshaked;
        internal Action<TcpTouchRpcSocketClient, VerifyOptionEventArgs> internalOnHandshaking;
        internal Action<TcpTouchRpcSocketClient, short, ByteBlock> onReceived;
        internal Action<TcpTouchRpcSocketClient, StreamStatusEventArgs> streamTransfered;
        internal Action<TcpTouchRpcSocketClient, StreamOperationEventArgs> streamTransfering;
        internal RRQMFileOperationEventHandler<TcpTouchRpcSocketClient> internalFileTransfering;
        internal RRQMTransferFileEventHandler<TcpTouchRpcSocketClient> internalFileTransfered;
        #endregion 内部委托

        #region 委托函数

        /// <summary>
        /// 在完成握手连接时
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnHandshaked(MesEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnHandshaked", this, e))
            {
                return;
            }
            this.internalOnHandshaked?.Invoke(this, e);
        }

        /// <summary>
        /// 当验证Token时，覆盖父类方法将不会触发事件和插件。
        /// </summary>
        /// <param name="e">参数</param>
        protected virtual void OnHandshaking(VerifyOptionEventArgs e)
        {
            if (e.Token == this.VerifyToken)
            {
                e.AddOperation(Operation.Permit);
            }
            else
            {
                e.Message = "Token不受理";
            }
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnHandshaking", this, e))
            {
                return;
            }
            this.internalOnHandshaking?.Invoke(this, e);
        }

        /// <summary>
        /// 触发Received事件
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="byteBlock"></param>
        protected void OnReceived(short protocol, ByteBlock byteBlock)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnReceivedProtocolData", this, new ProtocolDataEventArgs(protocol, byteBlock)))
            {
                return;
            }

            this.onReceived?.Invoke(this, protocol, byteBlock);
        }

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。覆盖父类方法将不会触发事件和插件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfered(StreamStatusEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnStreamTransfered", this, e))
            {
                return;
            }
            this.streamTransfered?.Invoke(this, e);
        }

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。覆盖父类方法将不会触发事件和插件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfering(StreamOperationEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnStreamTransfering", this, e))
            {
                return;
            }
            this.streamTransfering?.Invoke(this, e);
        }

        /// <summary>
        /// 在文件传输即将进行时触发。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFileTransfering(FileOperationEventArgs e)
        {
            if (this.UsePlugin&& this.PluginsManager.Raise<ITouchRpcPlugin>("OnFileTransfering", this, e))
            {
                return;
            }
            try
            {
                this.internalFileTransfering?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                this.Logger.Debug(LogType.Error, this, $"在事件{nameof(OnFileTransfering)}中发生异常", ex);
            }
        }

        /// <summary>
        /// 当文件传输结束之后。并不意味着完成传输，请通过<see cref="FileTransferStatusEventArgs.Result"/>属性值进行判断。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFileTransfered(FileTransferStatusEventArgs e)
        {
            if (this.UsePlugin&& this.PluginsManager.Raise<ITouchRpcPlugin>("OnFileTransfered", this, e))
            {
                return;
            }
            try
            {
                this.internalFileTransfered?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                this.Logger.Debug(LogType.Error, this, $"在事件{nameof(OnFileTransfered)}中发生异常", ex);
            }
        }

        #endregion 委托函数

        private void RpcActorSend(bool isAsync, TransferByte[] transferBytes)
        {
            if (isAsync)
            {
                base.SendAsync(transferBytes);
            }
            else
            {
                base.Send(transferBytes);
            }
        }
    }
}