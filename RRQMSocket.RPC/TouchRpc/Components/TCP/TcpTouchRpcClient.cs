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
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// TcpRpcClient
    /// </summary>
    public class TcpTouchRpcClient : TcpClientBase, ITcpTouchRpcClient
    {
        private RpcActor m_rpcActor;
        private Timer m_timer;
        private int m_failCount = 0;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Func<IRpcClient, bool> TryCanInvoke { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ID => this.m_rpcActor.ID;

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
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChannelExisted(int id)
        {
            return this.m_rpcActor.ChannelExisted(id);
        }

        /// <summary>
        /// 建立Tcp连接，并且执行握手。
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override ITcpClient Connect(int timeout = 5000)
        {
            return this.Connect(default, timeout);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual ITcpClient Connect(CancellationToken token = default, int timeout = 5000)
        {
            if (this.IsHandshaked)
            {
                return this;
            }
            if (!this.Online)
            {
                base.Connect(timeout);
            }

            this.m_rpcActor.Handshake(this.Config.GetValue<string>(TouchRpcConfigExtensions.VerifyTokenProperty), token, timeout);
            return this;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual Task<ITcpClient> ConnectAsync(CancellationToken token = default, int timeout = 5000)
        {
            return Task.Run(() =>
            {
                return this.Connect(token, timeout);
            });
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

        void IRpcParser.OnRegisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
        }

        void IRpcParser.OnUnregisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
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
        /// <param name="clientID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PushFile(string clientID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            return this.m_rpcActor.PushFile(clientID, fileRequest, fileOperator, metadata);
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
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public void ResetID(string id, CancellationToken cancellationToken = default)
        {
            this.m_rpcActor.ResetID(id, cancellationToken);
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

        void IRpcParser.SetRpcStore(RpcStore rpcService)
        {
            this.m_rpcActor.SetRpcStore(rpcService);
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
        /// <param name="config"></param>
        protected override void LoadConfig(RRQMConfig config)
        {
            this.InitRpcActor();
            this.RootPath = config.GetValue<string>(TouchRpcConfigExtensions.RootPathProperty);
            this.ResponseType = config.GetValue<ResponseType>(TouchRpcConfigExtensions.ResponseTypeProperty);
            this.m_rpcActor.SerializationSelector = config.GetValue<SerializationSelector>(TouchRpcConfigExtensions.SerializationSelectorProperty);
            base.LoadConfig(config);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConnecting(ClientConnectingEventArgs e)
        {
            this.SetDataHandlingAdapter(new FixedHeaderPackageAdapter());
            base.OnConnecting(e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDisconnected(ClientDisconnectedEventArgs e)
        {
            this.m_timer.SafeDisposeWithNull();
            this.m_rpcActor.Close(e.Message);
            base.OnDisconnected(e);
        }

        private void InitRpcActor()
        {
            this.SwitchProtocolToTouchRpc();
            this.m_rpcActor = new RpcActor(false, this.Container.Resolve<ILog>())
            {
                OnHandshaking = (e) => { e.IsPermitOperation = true; },
                OutputSend = this.RpcActorSend,
                OnHandshaked = this.OnHandshaked,
                OnReceived = this.OnReceived,
                OnClose = this.Close,
                OnStreamTransfering = this.OnStreamTransfering,
                OnStreamTransfered = this.OnStreamTransfered,
                OnFileTransfering = this.OnFileTransfering,
                OnFileTransfered = this.OnFileTransfered,
                Caller=this
            };
        }

        #region 事件

        /// <summary>
        /// 当文件传输结束之后。并不意味着完成传输，请通过<see cref="FileTransferStatusEventArgs.Result"/>属性值进行判断。
        /// </summary>
        public event RRQMTransferFileEventHandler<TcpTouchRpcClient> FileTransfered;

        /// <summary>
        /// 在文件传输即将进行时触发。
        /// </summary>
        public event RRQMFileOperationEventHandler<TcpTouchRpcClient> FileTransfering;

        /// <summary>
        /// 在完成握手连接时
        /// </summary>
        public event RRQMMessageEventHandler<TcpTouchRpcClient> Handshaked;

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event RRQMProtocolReceivedEventHandler<TcpTouchRpcClient> Received;

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。
        /// </summary>
        public event RRQMStreamStatusEventHandler<TcpTouchRpcClient> StreamTransfered;

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。
        /// </summary>
        public event RRQMStreamOperationEventHandler<TcpTouchRpcClient> StreamTransfering;

        #endregion 事件

        #region 事件触发

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
                this.FileTransfered?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                this.Logger.Debug(LogType.Error, this, $"在事件{nameof(FileTransfered)}中发生异常", ex);
            }
        }

        /// <summary>
        /// 在文件传输即将进行时触发。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFileTransfering(FileOperationEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnFileTransfering", this, e))
            {
                return;
            }
            try
            {
                this.FileTransfering?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                this.Logger.Debug(LogType.Error, this, $"在事件{nameof(FileTransfering)}中发生异常", ex);
            }
        }

        /// <summary>
        /// 在完成握手连接时
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnHandshaked(MesEventArgs e)
        {
            this.m_timer.SafeDisposeWithNull();

            if (this.Config.GetValue<HeartbeatValue>(TouchRpcConfigExtensions.HeartbeatFrequencyProperty) is HeartbeatValue heartbeat)
            {
                this.m_timer = new Timer((obj) =>
                {
                    if (this.Ping())
                    {
                        this.m_failCount = 0;
                    }
                    else
                    {
                        if (++this.m_failCount > heartbeat.MaxFailCount)
                        {
                            this.Close("自动心跳失败次数达到最大，已清理连接。");
                            this.m_timer.SafeDisposeWithNull();
                        }
                    }
                }, null, heartbeat.Interval, heartbeat.Interval);
            }

            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnHandshaked", this, e))
            {
                return;
            }

            this.Handshaked?.Invoke(this, e);
        }

        /// <summary>
        /// 收到数据。
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="byteBlock"></param>
        protected virtual void OnReceived(short protocol, ByteBlock byteBlock)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnReceivedProtocolData", this, new ProtocolDataEventArgs(protocol, byteBlock)))
            {
                return;
            }
            this.Received?.Invoke(this, protocol, byteBlock);
        }

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfered(StreamStatusEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnStreamTransfered", this, e))
            {
                return;
            }
            this.StreamTransfered?.Invoke(this, e);
        }

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfering(StreamOperationEventArgs e)
        {
            if (this.UsePlugin && this.PluginsManager.Raise<ITouchRpcPlugin>("OnStreamTransfering", this, e))
            {
                return;
            }
            this.StreamTransfering?.Invoke(this, e);
        }

        #endregion 事件触发

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