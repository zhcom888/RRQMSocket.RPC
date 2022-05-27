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
using RRQMCore.Dependency;
using RRQMCore.Extensions;
using RRQMSocket.Http;
using RRQMSocket.Http.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RRQMSocket.RPC
{
    /// <summary>
    /// Rpc仓库
    /// </summary>
    public class RpcStore : DisposableObject
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public const string Namespace = "namespace";

        /// <summary>
        /// 代理键
        /// </summary>
        public const string ProxyKey = "proxy";

        private static readonly Dictionary<string, Type> m_proxyAttributeMap = new Dictionary<string, Type>();
        private string m_proxyUrl = "/proxy";
        private HttpService m_service;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RpcStore()
        {
            this.ServerProviders = new ServerProviderCollection();
            this.RpcParsers = new RpcParserCollection();
            this.MethodMap = new MethodMap();
            this.Container = new Container();
            SearchAttribute();
        }

        /// <summary>
        /// 代理属性映射。
        /// </summary>
        public static Dictionary<string, Type> ProxyAttributeMap => m_proxyAttributeMap;

        /// <summary>
        /// 注入容器
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// 注入配置。
        /// </summary>
        /// <param name="config"></param>
        public void Setup(RRQMConfig config)
        {
            this.Container = config.Container;
        }

        /// <summary>
        /// 函数映射图实例
        /// </summary>
        public MethodMap MethodMap { get; private set; }

        /// <summary>
        /// 请求代理。
        /// </summary>
        public Func<HttpRequest, bool> OnRequestProxy { get; set; }

        /// <summary>
        /// 代理路径。默认为“/proxy”。
        /// <para>必须以“/”开头</para>
        /// </summary>
        public string ProxyUrl
        {
            get => this.m_proxyUrl;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "/";
                }
                this.m_proxyUrl = value;
            }
        }

        /// <summary>
        /// 获取Rpc解析器集合
        /// </summary>
        public RpcParserCollection RpcParsers { get; private set; }

        /// <summary>
        /// 服务实例集合
        /// </summary>
        public ServerProviderCollection ServerProviders { get; private set; }

        /// <summary>
        /// 添加Rpc解析器
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="parser">解析器实例</param>
        /// <param name="applyServer">是否应用已注册服务</param>
        public void AddRpcParser(string key, IRpcParser parser, bool applyServer = true)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            this.RpcParsers.Add(key, parser);
            parser.SetRpcStore(this);
            if (applyServer)
            {
                Dictionary<IServerProvider, List<MethodInstance>> pairs = new Dictionary<IServerProvider, List<MethodInstance>>();

                MethodInstance[] instances = this.MethodMap.GetAllMethodInstances();

                foreach (var item in instances)
                {
                    if (!pairs.ContainsKey(item.Provider))
                    {
                        pairs.Add(item.Provider, new List<MethodInstance>());
                    }

                    pairs[item.Provider].Add(item);
                }
                foreach (var item in pairs.Keys)
                {
                    parser.OnRegisterServer(item, pairs[item].ToArray());
                }
            }
        }

        /// <summary>
        /// 关闭分享中心
        /// </summary>
        public void StopShareProxy()
        {
            this.m_service.SafeDisposeWithNull();
        }

        /// <summary>
        /// 执行Rpc
        /// </summary>
        /// <param name="methodInstance"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public InvokeResult Execute(MethodInstance methodInstance, object[] ps)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            InvokeResult invokeResult = new InvokeResult();
            IServerProvider serverProvider = this.GetServerProvider(methodInstance);
            try
            {
                //serverProvider.RpcEnter(parser, methodInvoker, methodInstance);
                if (methodInstance.HasReturn)
                {
                    invokeResult.Result = methodInstance.Invoke(serverProvider, ps);
                }
                else
                {
                    methodInstance.Invoke(serverProvider, ps);
                }
                //serverProvider.RpcLeave(parser, methodInvoker, methodInstance);
                invokeResult.Status = InvokeStatus.Success;
            }
            catch (TargetInvocationException e)
            {
                invokeResult.Status = InvokeStatus.InvocationException;
                if (e.InnerException != null)
                {
                    invokeResult.Message = "函数内部发生异常，信息：" + e.InnerException.Message;
                }
                else
                {
                    invokeResult.Message = "函数内部发生异常，信息：未知";
                }
                // serverProvider.RpcError(parser, methodInvoker, methodInstance);
            }
            catch (Exception e)
            {
                invokeResult.Status = InvokeStatus.Exception;
                invokeResult.Message = e.Message;
                //serverProvider.RpcError(parser, methodInvoker, methodInstance);
            }

            return invokeResult;
        }

        /// <summary>
        /// 从远程获取代理
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetProxyInfo(string url)
        {
            HttpClient client = new HttpClient();
            ByteBlock byteBlock = new ByteBlock();
            try
            {
                RRQMConfig config = new RRQMConfig();
                IPHost iPHost = new IPHost(url);
                config.SetRemoteIPHost(iPHost);
                client.Setup(config)
                    .Connect();

                HttpRequest httpRequest = new HttpRequest()
                    .SetUrl(iPHost.GetUrlPath())
                    .AsGet();
                HttpResponse response = client.RequestContent(httpRequest);
                if (response.StatusCode == "200")
                {
                    return response.GetBody();
                }
                throw new RRQMException(response.StatusMessage);
            }
            finally
            {
                byteBlock.Dispose();
                client.Dispose();
            }
        }

        /// <summary>
        /// 从本地获取代理
        /// </summary>
        /// <param name="attrbuteType"></param>
        /// <returns></returns>
        public ServerCellCode[] GetProxyInfo(params Type[] attrbuteType)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            List<ServerCellCode> codes = new List<ServerCellCode>();
            foreach (var attrbute in attrbuteType)
            {
                foreach (var item in this.ServerProviders)
                {
                    ServerCellCode serverCellCode = CodeGenerator.Generator(item.GetType(), attrbute);
                    codes.Add(serverCellCode);
                }
            }
            return codes.ToArray();
        }

        /// <summary>
        /// 分享代理。
        /// </summary>
        /// <param name="iPHost"></param>
        public void ShareProxy(IPHost iPHost)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (this.m_service != null)
            {
                return;
            }
            this.m_service = new HttpService();
            this.m_service.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { iPHost }))
                .Start();

            this.m_service.AddPlugin(new InternalPlugin(this));
        }

        /// <summary>
        /// 注册所有服务
        /// </summary>
        /// <returns>返回搜索到的服务数</returns>
        public int RegisterAllServer()
        {
            Type[] types = (AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes()).Where(p => typeof(ServerProvider).IsAssignableFrom(p) && !p.IsAbstract)).ToArray();

            foreach (Type type in types)
            {
                this.RegisterServer(type);
            }
            return types.Length;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IServerProvider RegisterServer<T>() where T : IServerProvider
        {
            return this.RegisterServer(typeof(T));
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public IServerProvider RegisterServer(Type providerType)
        {
            if (!typeof(IServerProvider).IsAssignableFrom(providerType))
            {
                throw new RpcException("类型不相符");
            }
            IServerProvider serverProvider = (IServerProvider)this.Container.Resolve(providerType);
            this.RegisterServer(serverProvider);
            return serverProvider;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="serverProvider"></param>
        public void RegisterServer(IServerProvider serverProvider)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            serverProvider.SetRpcStore(this);
            this.ServerProviders.Add(serverProvider);
            MethodInstance[] methodInstances = CodeGenerator.GetMethodInstances(serverProvider.GetType());

            foreach (var item in methodInstances)
            {
                item.Provider = serverProvider;
                this.MethodMap.Add(item);
            }
            foreach (var parser in this.RpcParsers)
            {
                parser.OnRegisterServer(serverProvider, methodInstances);
            }
        }

        /// <summary>
        /// 移除Rpc解析器
        /// </summary>
        /// <param name="parserName"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public bool RemoveRpcParser(string parserName, out IRpcParser parser)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return this.RpcParsers.TryRemove(parserName, out parser);
        }

        /// <summary>
        /// 移除Rpc解析器
        /// </summary>
        /// <param name="parserName"></param>
        /// <returns></returns>
        public bool RemoveRpcParser(string parserName)
        {
            return this.RemoveRpcParser(parserName, out _);
        }

        /// <summary>
        /// 设置服务方法可用性
        /// </summary>
        /// <param name="methodKey">方法名</param>
        /// <param name="enable">可用性</param>
        /// <exception cref="RpcException"></exception>
        public void SetMethodEnable(string methodKey, bool enable)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (this.MethodMap.TryGet(methodKey, out MethodInstance methodInstance))
            {
                methodInstance.IsEnable = enable;
            }
            else
            {
                throw new RpcException("未找到该方法");
            }
        }

        /// <summary>
        /// 获取解析器
        /// </summary>
        /// <param name="parserKey"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public bool TryGetRpcParser(string parserKey, out IRpcParser parser)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return this.RpcParsers.TryGetRpcParser(parserKey, out parser);
        }

        /// <summary>
        /// 移除注册服务
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public int UnregisterServer(IServerProvider provider)
        {
            return this.UnregisterServer(provider.GetType());
        }

        /// <summary>
        /// 移除注册服务
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public int UnregisterServer(Type providerType)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (!typeof(IServerProvider).IsAssignableFrom(providerType))
            {
                throw new RpcException("类型不相符");
            }
            this.ServerProviders.Remove(providerType);
            if (this.MethodMap.RemoveServer(providerType, out IServerProvider serverProvider, out MethodInstance[] instances))
            {
                foreach (var parser in this.RpcParsers)
                {
                    parser.OnUnregisterServer(serverProvider, instances);
                }

                return instances.Length;
            }
            return 0;
        }

        /// <summary>
        /// 移除注册服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int UnregisterServer<T>() where T : ServerProvider
        {
            return this.UnregisterServer(typeof(T));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                this.StopShareProxy();
                foreach (var item in this.RpcParsers)
                {
                    item.SafeDispose();
                }
            }

            base.Dispose(disposing);
        }

        private IServerProvider GetServerProvider(MethodInstance methodInstance)
        {
            return methodInstance.Provider;
        }

        private static void SearchAttribute()
        {
            Type[] types = (AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(s => s.GetTypes()).Where(p => typeof(RpcAttribute).IsAssignableFrom(p) && !p.IsAbstract)).ToArray();

            foreach (Type type in types)
            {
                ProxyAttributeMap.TryAdd(type.Name.Replace("Attribute", string.Empty).ToLower(), type);
            }
        }
    }

    internal class InternalPlugin : HttpPluginBase
    {
        private readonly RpcStore m_rpcCerter;

        public InternalPlugin(RpcStore rpcCerter)
        {
            this.m_rpcCerter = rpcCerter;
        }

        protected override void OnGet(ITcpClientBase client, HttpContextEventArgs e)
        {
            if (e.Context.Request.UrlEquals(this.m_rpcCerter.ProxyUrl))
            {
                bool? b = this.m_rpcCerter.OnRequestProxy?.Invoke(e.Context.Request);
                if (b == false)
                {
                    using (ByteBlock byteBlock = new ByteBlock())
                    {
                        e.Context.Response
                            .FromText("拒绝响应内容")
                        .SetStatus("403", "Forbidden")
                        .Build(byteBlock);
                        client.DefaultSend(byteBlock);
                    }
                    return;
                }
                if (e.Context.Request.TryGetQuery(RpcStore.ProxyKey, out string value))
                {
                    List<Type> types = new List<Type>();

                    if (value.Equals("all", StringComparison.CurrentCultureIgnoreCase))
                    {
                        types = RpcStore.ProxyAttributeMap.Values.ToList();
                    }
                    else
                    {
                        string[] vs = value.Split(',');
                        foreach (var item in vs)
                        {
                            if (RpcStore.ProxyAttributeMap.TryGetValue(item, out Type type))
                            {
                                types.Add(type);
                            }
                        }
                    }


                    e.Context.Request.TryGetQuery(RpcStore.Namespace, out string names);

                    names = string.IsNullOrEmpty(names) ? "RRQMProxy" : names;

                    string code = CodeGenerator.ConvertToCode(names, this.m_rpcCerter.GetProxyInfo(types.ToArray()));

                    using (ByteBlock byteBlock = new ByteBlock())
                    {
                        e.Context.Response
                        .SetStatus()
                        .SetContent(code)
                        .SetContentTypeFromFileName($"{names}.cs")
                        .Build(byteBlock);
                        client.DefaultSend(byteBlock);
                    }
                }
            }
            base.OnGet(client, e);
        }
    }
}