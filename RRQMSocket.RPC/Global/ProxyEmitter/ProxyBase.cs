using System;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.ProxyEmitter
{
    /// <summary>
    /// Base class for all Proxy class
    /// </summary>
    public abstract class ProxyBase : IRpcClient
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract Func<IRpcClient, bool> TryCanInvoke { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        public abstract void Invoke(string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract T Invoke<T>(string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract T Invoke<T>(string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
        public abstract void Invoke(string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract Task InvokeAsync(string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="invokeOption"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract Task<T> InvokeAsync<T>(string method, IInvokeOption invokeOption, params object[] parameters);
    }
}
