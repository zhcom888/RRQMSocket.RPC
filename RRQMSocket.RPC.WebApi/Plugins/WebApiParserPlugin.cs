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
using RRQMCore.Converter;
using RRQMCore.Extensions;
using RRQMSocket.Http;
using RRQMSocket.Http.Plugins;
using System;
using System.Net.Sockets;

namespace RRQMSocket.RPC.WebApi
{
    /// <summary>
    /// WebApi解析器
    /// </summary>
    public class WebApiParserPlugin : HttpPluginBase, IRpcParser
    {
        private StringConverter m_converter;
        private RpcStore m_rpcCerter;
        private RouteMap routeMap;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WebApiParserPlugin()
        {
            this.routeMap = new RouteMap();
            this.m_converter = new StringConverter();
            this.m_converter.Clear();
            this.m_converter.Add(new JsonStringToClassConverter());
        }

        /// <summary>
        /// 转化器
        /// </summary>
        public StringConverter Converter => this.m_converter;

        /// <summary>
        /// 获取路由映射图
        /// </summary>
        public RouteMap RouteMap => this.routeMap;

        /// <summary>
        /// 所属服务器
        /// </summary>
        public RpcStore RpcStore => this.m_rpcCerter;

        void IRpcParser.SetRpcStore(RpcStore rpcService)
        {
            this.m_rpcCerter = rpcService;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected override void OnGet(ITcpClientBase client, HttpContextEventArgs e)
        {
            if (this.routeMap.TryGet(e.Context.Request.RelativeURL.ToLower(), out MethodInstance methodInstance))
            {
                e.Handled = true;

                InvokeResult invokeResult = new InvokeResult();
                object[] ps = null;
                if (methodInstance.IsEnable)
                {
                    try
                    {
                        ps = new object[methodInstance.Parameters.Length];
                        int i = 0;
                        if (methodInstance.MethodFlags.HasFlag(MethodFlags.IncludeCallContext))
                        {
                            WebApiServerCallContext callContext = new WebApiServerCallContext(client, e.Context, methodInstance);
                            ps[i] = callContext;
                            i++;
                        }
                        if (e.Context.Request.Query == null)
                        {
                            for (; i < methodInstance.Parameters.Length; i++)
                            {
                                ps[i] = methodInstance.ParameterTypes[i].GetDefault();
                            }
                        }
                        else
                        {
                            for (; i < methodInstance.Parameters.Length; i++)
                            {
                                if (e.Context.Request.Query.TryGetValue(methodInstance.ParameterNames[i], out string value))
                                {
                                    ps[i] = this.m_converter.ConvertFrom(value, methodInstance.ParameterTypes[i]);
                                }
                                else
                                {
                                    ps[i] = methodInstance.ParameterTypes[i].GetDefault();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        invokeResult.Status = InvokeStatus.Exception;
                        invokeResult.Message = ex.Message;
                    }
                }
                else
                {
                    invokeResult.Status = InvokeStatus.UnEnable;
                }
                if (invokeResult.Status == InvokeStatus.Ready)
                {
                    invokeResult = this.m_rpcCerter.Execute(methodInstance, ps);
                }

                if (e.Context.Response.Responsed)
                {
                    return;
                }
                HttpResponse httpResponse = e.Context.Response;
                switch (invokeResult.Status)
                {
                    case InvokeStatus.Success:
                        {
                            httpResponse.FromJson(this.m_converter.ConvertTo(invokeResult.Result)).SetStatus();
                            break;
                        }
                    case InvokeStatus.UnFound:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("404");
                            break;
                        }
                    case InvokeStatus.UnEnable:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("405");
                            break;
                        }
                    case InvokeStatus.InvocationException:
                    case InvokeStatus.Exception:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("422");
                            break;
                        }
                }

                using (ByteBlock byteBlock = new ByteBlock())
                {
                    httpResponse.Build(byteBlock);
                    client.DefaultSend(byteBlock);
                }

                if (!e.Context.Request.KeepAlive)
                {
                    client.Shutdown(SocketShutdown.Both);
                }
            }

            base.OnGet(client, e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected override void OnPost(ITcpClientBase client, HttpContextEventArgs e)
        {
            if (this.routeMap.TryGet(e.Context.Request.RelativeURL.ToLower(), out MethodInstance methodInstance))
            {
                e.Handled = true;

                InvokeResult invokeResult = new InvokeResult();
                object[] ps = null;
                if (methodInstance.IsEnable)
                {
                    try
                    {
                        ps = new object[methodInstance.Parameters.Length];
                        int i = 0;
                        if (methodInstance.MethodFlags.HasFlag(MethodFlags.IncludeCallContext))
                        {
                            WebApiServerCallContext callContext = new WebApiServerCallContext(client, e.Context, methodInstance);
                            ps[i] = callContext;
                            i++;
                        }

                        if (e.Context.Request.Query == null)
                        {
                            for (; i < methodInstance.Parameters.Length - 1; i++)
                            {
                                ps[i] = methodInstance.ParameterTypes[i].GetDefault();
                            }
                        }
                        else
                        {
                            for (; i < methodInstance.Parameters.Length - 1; i++)
                            {
                                if (e.Context.Request.Query.TryGetValue(methodInstance.ParameterNames[i], out string value))
                                {
                                    ps[i] = this.m_converter.ConvertFrom(value, methodInstance.ParameterTypes[i]);
                                }
                                else
                                {
                                    ps[i] = methodInstance.ParameterTypes[i].GetDefault();
                                }
                            }
                        }

                        int index = methodInstance.Parameters.Length - 1;

                        if (index >= 0)
                        {
                            string str = e.Context.Request.GetBody();
                            ps[index] = this.m_converter.ConvertFrom(str, methodInstance.ParameterTypes[index]);
                        }
                    }
                    catch (Exception ex)
                    {
                        invokeResult.Status = InvokeStatus.Exception;
                        invokeResult.Message = ex.Message;
                    }
                }
                else
                {
                    invokeResult.Status = InvokeStatus.UnEnable;
                }
                if (invokeResult.Status == InvokeStatus.Ready)
                {
                    invokeResult = this.m_rpcCerter.Execute(methodInstance, ps);
                }

                if (e.Context.Response.Responsed)
                {
                    return;
                }
                HttpResponse httpResponse = e.Context.Response;
                switch (invokeResult.Status)
                {
                    case InvokeStatus.Success:
                        {
                            httpResponse.FromJson(this.m_converter.ConvertTo(invokeResult.Result)).SetStatus();
                            break;
                        }
                    case InvokeStatus.UnFound:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("404", invokeResult.Status.ToString());
                            break;
                        }
                    case InvokeStatus.UnEnable:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("405", invokeResult.Status.ToString());
                            break;
                        }
                    case InvokeStatus.InvocationException:
                    case InvokeStatus.Exception:
                        {
                            string jsonString = this.m_converter.ConvertTo(new ActionResult() { Status = invokeResult.Status, Message = invokeResult.Message });
                            httpResponse.FromJson(jsonString).SetStatus("422", invokeResult.Status.ToString());
                            break;
                        }
                }

                using (ByteBlock byteBlock = new ByteBlock())
                {
                    httpResponse.Build(byteBlock);
                    client.DefaultSend(byteBlock);
                }

                if (!e.Context.Request.KeepAlive)
                {
                    client.Shutdown(SocketShutdown.Both);
                }
            }

            base.OnPost(client, e);
        }

        #region RPC解析器

        void IRpcParser.OnRegisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
            foreach (var methodInstance in methodInstances)
            {
                string actionUrl = WebApiAttribute.GetRouteUrl(methodInstance);
                if (!string.IsNullOrEmpty(actionUrl))
                {
                    this.routeMap.Add(actionUrl, methodInstance);
                }
            }
        }

        void IRpcParser.OnUnregisterServer(IServerProvider provider, MethodInstance[] methodInstances)
        {
            foreach (var methodInstance in methodInstances)
            {
                string actionUrl = WebApiAttribute.GetRouteUrl(methodInstance);
                if (!string.IsNullOrEmpty(actionUrl))
                {
                    this.routeMap.Remove(actionUrl);
                }
            }
        }

        #endregion RPC解析器
    }
}