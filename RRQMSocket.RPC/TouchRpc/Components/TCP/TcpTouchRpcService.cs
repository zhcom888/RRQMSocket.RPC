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
using RRQMCore.Data.Security;
using RRQMCore.Serialization;
using System;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// TCP Rpc解释器
    /// </summary>
    public class TcpTouchRpcService : TcpTouchRpcService<TcpTouchRpcSocketClient>
    {
    }

    /// <summary>
    /// TcpRpcParser泛型类型
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public class TcpTouchRpcService<TClient> : TcpService<TClient>, ITcpTouchRpcService where TClient : TcpTouchRpcSocketClient
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpTouchRpcService()
        {
            this.m_ID = this.GetDefaultNewID();
        }

        #region 字段
        private readonly string m_ID;
        private RpcStore m_rpcService;

        #endregion 字段

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public RpcStore RpcStore => this.m_rpcService;

        /// <summary>
        /// 客户端请求连接
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="e"></param>
        protected override void OnConnecting(TClient socketClient, ClientOperationEventArgs e)
        {
            socketClient.m_rpcActor.SetRpcStore(this.m_rpcService);
            socketClient.m_rpcActor.SerializationSelector = this.Config.GetValue<SerializationSelector>(TouchRpcConfigExtensions.SerializationSelectorProperty);

            socketClient.internalFileTransfering = this.PrivateFileTransfering;
            socketClient.internalFileTransfered = this.PrivateFileTransfered;
            socketClient.ResponseType = this.Config.GetValue<ResponseType>(TouchRpcConfigExtensions.ResponseTypeProperty);
            socketClient.RootPath = this.Config.GetValue<string>(TouchRpcConfigExtensions.RootPathProperty);

            socketClient.internalOnHandshaked = this.PrivateHandshaked;
            socketClient.internalOnHandshaking = this.PrivateOnHandshaking;
            socketClient.streamTransfered = this.PrivateStreamTransfered;
            socketClient.streamTransfering = this.PrivateStreamTransfering;
            socketClient.onReceived = this.OnReceived;
            base.OnConnecting(socketClient, e);
        }

        #region 事件

        /// <summary>
        /// 当文件传输结束之后。并不意味着完成传输，请通过<see cref="FileTransferStatusEventArgs.Result"/>属性值进行判断。
        /// </summary>
        public event RRQMTransferFileEventHandler<TClient> FileTransfered;

        /// <summary>
        /// 文件传输开始之前
        /// </summary>
        public event RRQMFileOperationEventHandler<TClient> FileTransfering;

        /// <summary>
        /// 在完成握手连接时
        /// </summary>
        public event RRQMMessageEventHandler<TClient> Handshaked;

        /// <summary>
        /// 表示即将握手
        /// </summary>
        public event HandshakingEventHandler<TClient> Handshaking;

        /// <summary>
        /// 收到数据
        /// </summary>
        public event RRQMProtocolReceivedEventHandler<TClient> Received;

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。
        /// </summary>
        public event RRQMStreamStatusEventHandler<TClient> StreamTransfered;

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。
        /// </summary>
        public event RRQMStreamOperationEventHandler<TClient> StreamTransfering;

        /// <summary>
        /// 在完成握手连接时
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected virtual void OnHandshaked(TClient client, MesEventArgs e)
        {
            this.Handshaked?.Invoke(client, e);
        }

        /// <summary>
        /// 在验证Token时
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        protected virtual void OnHandshaking(TClient client, VerifyOptionEventArgs e)
        {
            this.Handshaking?.Invoke(client, e);
        }

        /// <summary>
        /// 流数据处理，用户需要在此事件中对e.Bucket手动释放。覆盖父类方法将不会触发事件和插件。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfered(TClient client, StreamStatusEventArgs e)
        {
            this.StreamTransfered?.Invoke(client, e);
        }

        /// <summary>
        /// 即将接收流数据，用户需要在此事件中对e.Bucket初始化。覆盖父类方法将不会触发事件和插件。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected virtual void OnStreamTransfering(TClient client, StreamOperationEventArgs e)
        {
            this.StreamTransfering?.Invoke(client, e);
        }

        private void OnReceived(TcpTouchRpcSocketClient client, short protocol, ByteBlock byteBlock)
        {
            this.Received?.Invoke((TClient)client, protocol, byteBlock);
        }

        private void PrivateFileTransfered(TcpTouchRpcSocketClient client, FileTransferStatusEventArgs e)
        {
            this.FileTransfered?.Invoke((TClient)client, e);
        }

        private void PrivateFileTransfering(TcpTouchRpcSocketClient client, FileOperationEventArgs e)
        {
            this.FileTransfering?.Invoke((TClient)client, e);
        }

        private void PrivateHandshaked(TcpTouchRpcSocketClient client, MesEventArgs e)
        {
            this.OnHandshaked((TClient)client, e);
        }

        private void PrivateOnHandshaking(TcpTouchRpcSocketClient client, VerifyOptionEventArgs e)
        {
            this.OnHandshaking((TClient)client, e);
        }

        private void PrivateStreamTransfered(TcpTouchRpcSocketClient client, StreamStatusEventArgs e)
        {
            this.OnStreamTransfered((TClient)client, e);
        }

        private void PrivateStreamTransfering(TcpTouchRpcSocketClient client, StreamOperationEventArgs e)
        {
            this.OnStreamTransfering((TClient)client, e);
        }

        #endregion 事件

        #region Rpc

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
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                client.Invoke(targetID, method, invokeOption, ref parameters, types);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
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
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.Invoke<T>(targetID, method, invokeOption, ref parameters, types);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }

        }


        /// <summary>
        /// 反向调用客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        public void Invoke(string targetID, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                client.Invoke(method, invokeOption, parameters);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// 反向调用客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        /// <returns>返回值</returns>
        public T Invoke<T>(string targetID, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.Invoke<T>(method, invokeOption, parameters);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// 反向调用客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        public Task InvokeAsync(string targetID, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.InvokeAsync(method, invokeOption, parameters);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// 反向调用客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        /// <returns>返回值</returns>
        public Task<T> InvokeAsync<T>(string targetID, string method, IInvokeOption invokeOption, params object[] parameters)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.InvokeAsync<T>(method, invokeOption, parameters);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        #endregion Rpc

        #region 通道

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public Channel CreateChannel(string targetID)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.CreateChannel();
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel CreateChannel(string targetID, int id)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.CreateChannel(id);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        #endregion 通道

        #region File

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Result PullFile(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.PullFile(fileRequest, fileOperator, metadata);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PullFileAsync(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.PullFileAsync(fileRequest, fileOperator, metadata);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
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
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.PushFile(fileRequest, fileOperator, metadata);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public Task<Result> PushFileAsync(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null)
        {
            if (this.TryGetSocketClient(targetID, out TClient client))
            {
                return client.PushFileAsync(fileRequest, fileOperator, metadata);
            }
            else
            {
                throw new ClientNotFindException(ResType.ClientNotFind.GetDescription(targetID));
            }
        }

        #endregion File

        #region RPC解析器

        void IRpcParser.OnRegisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
        }

        void IRpcParser.OnUnregisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
        }

        void IRpcParser.SetRpcStore(RpcStore rpcService)
        {
            this.m_rpcService = rpcService;
        }
        #endregion RPC解析器
    }
}