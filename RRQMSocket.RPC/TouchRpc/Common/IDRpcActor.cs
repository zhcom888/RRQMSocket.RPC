using RRQMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// IDRpcActor
    /// </summary>
    public class IDRpcActor : IRpcClient
    {
        private readonly string m_targetID;
        private readonly IRpcActor m_rpcActor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="rpcActor"></param>
        public IDRpcActor(string targetID, IRpcActor rpcActor)
        {
            this.m_targetID = targetID;
            this.m_rpcActor = rpcActor;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Func<IRpcClient, bool> TryCanInvoke { get => this.m_rpcActor.TryCanInvoke; set => this.m_rpcActor.TryCanInvoke = value; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            this.m_rpcActor.Dispose();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        public void Invoke(string method, IInvokeOption invokeOption, params object[] parameters)
        {
            this.m_rpcActor.Invoke(this.m_targetID, method, invokeOption, parameters);
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
            return this.m_rpcActor.Invoke<T>(this.m_targetID, method, invokeOption, parameters);
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
            return this.m_rpcActor.Invoke<T>(this.m_targetID, method, invokeOption, ref parameters, types);
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
            this.m_rpcActor.Invoke(this.m_targetID, method, invokeOption, ref parameters, types);
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
            return this.m_rpcActor.InvokeAsync(this.m_targetID, method, invokeOption, parameters);
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
            return this.m_rpcActor.InvokeAsync<T>(this.m_targetID, method, invokeOption, parameters);
        }
    }
}
