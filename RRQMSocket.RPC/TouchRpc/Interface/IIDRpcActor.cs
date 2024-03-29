﻿using RRQMCore;
using System;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// 具有ID交互的Rpc接口
    /// </summary>
    public interface IIDRpcActor
    {
        /// <summary>
        /// 创建一个和其他客户端的通道
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public Channel CreateChannel(string targetID);

        /// <summary>
        /// 创建一个和其他客户端的通道
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel CreateChannel(string targetID, int id);

        /// <summary>
        /// 从对点拉取文件
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Result PullFile(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null);

        /// <summary>
        /// 异步从对点拉取文件
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task<Result> PullFileAsync(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null);

        /// <summary>
        /// 向对点推送文件
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Result PushFile(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null);

        /// <summary>
        /// 异步向对点推送文件
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="fileRequest"></param>
        /// <param name="fileOperator"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task<Result> PushFileAsync(string targetID, FileRequest fileRequest, FileOperator fileOperator, Metadata metadata = null);

        /// <summary>
        /// 调用对应ID的客户端Rpc
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
        Task InvokeAsync(string targetID, string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// 调用对应ID的客户端Rpc
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
        Task<T> InvokeAsync<T>(string targetID, string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// 调用对应ID的客户端Rpc
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
        void Invoke(string targetID, string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// 调用对应ID的客户端Rpc
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
        T Invoke<T>(string targetID, string method, IInvokeOption invokeOption, params object[] parameters);

        /// <summary>
        /// 调用对应ID的客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <param name="types"></param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        void Invoke(string targetID, string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types);

        /// <summary>
        /// 调用对应ID的客户端Rpc
        /// </summary>
        /// <param name="targetID">客户端ID</param>
        /// <param name="method">方法名</param>
        /// <param name="invokeOption">调用配置</param>
        /// <param name="parameters">参数</param>
        /// <param name="types"></param>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">调用内部异常</exception>
        /// <exception cref="ClientNotFindException">没有找到ID对应的客户端</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        /// <returns>返回值</returns>
        T Invoke<T>(string targetID, string method, IInvokeOption invokeOption, ref object[] parameters, Type[] types);
    }
}
