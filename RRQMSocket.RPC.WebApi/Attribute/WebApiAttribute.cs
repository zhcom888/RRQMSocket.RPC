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
using System;
using System.Text;

namespace RRQMSocket.RPC.WebApi
{

    /// <summary>
    /// WebApiAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class WebApiAttribute : RpcAttribute
    {
        private HttpMethodType m_method;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="method"></param>
        public WebApiAttribute(HttpMethodType method)
        {
            this.m_method = method;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override Type[] GetGenericInterfaceTypes()
        {
            return new Type[] { typeof(IWebApiClient) };
        }

        /// <summary>
        /// 路由路径。必须以“/”开始。
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 函数类型。
        /// </summary>
        public HttpMethodType Method => this.m_method;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="methodInstance"></param>
        /// <returns></returns>
        public override string GetInvokenKey(MethodInstance methodInstance)
        {
            int i = 0;
            if (methodInstance.MethodFlags.HasFlag(MethodFlags.IncludeCallContext))
            {
                i = 1;
            }
            if (this.m_method == HttpMethodType.GET)
            {
                string actionUrl = GetRouteUrl(methodInstance);
                if (methodInstance.ParameterNames.Length > i)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(GetRouteUrl(methodInstance));
                    stringBuilder.Append("?");
                    for (; i < methodInstance.ParameterNames.Length; i++)
                    {
                        stringBuilder.Append(methodInstance.ParameterNames[i] + "={&}".Replace("&", i.ToString()));
                        if (i != methodInstance.ParameterNames.Length - 1)
                        {
                            stringBuilder.Append("&");
                        }
                    }
                    actionUrl = stringBuilder.ToString();
                }
                return $"GET:{actionUrl}";
            }
            else if (this.m_method == HttpMethodType.POST)
            {
                string actionUrl = GetRouteUrl(methodInstance);
                if (methodInstance.ParameterNames.Length > i + 1)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(GetRouteUrl(methodInstance));
                    stringBuilder.Append("?");
                    for (; i < methodInstance.ParameterNames.Length - 1; i++)
                    {
                        stringBuilder.Append(methodInstance.ParameterNames[i] + "={&}".Replace("&", i.ToString()));
                        if (i != methodInstance.ParameterNames.Length - 2)
                        {
                            stringBuilder.Append("&");
                        }
                    }
                    actionUrl = stringBuilder.ToString();
                }
                return $"POST:{actionUrl}";
            }
            else
            {
                return base.GetInvokenKey(methodInstance);
            }
        }

        /// <summary>
        /// 获取路由路径
        /// </summary>
        /// <param name="methodInstance"></param>
        /// <returns></returns>
        public static string GetRouteUrl(MethodInstance methodInstance)
        {
            foreach (var att in methodInstance.RpcAttributes)
            {
                if (att is WebApiAttribute httpMethodAttribute)
                {
                    if (string.IsNullOrEmpty(httpMethodAttribute.Route))
                    {
                        return $"/{methodInstance.Info.DeclaringType.Name}/{methodInstance.Name}".ToLower();
                    }

                    return httpMethodAttribute.Route.ToLower();
                }
            }
            return default;
        }
    }
}
