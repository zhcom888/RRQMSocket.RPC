namespace RRQMSocket.RPC.WebApi
{
    /// <summary>
    /// 请求函数类型
    /// </summary>
    public enum HttpMethodType
    {
        /// <summary>
        /// 以GET方式。支持调用上下文。
        /// <para>以该方式时，所有的参数类型必须是基础类型。所有的参数来源均来自url参数。</para>
        /// </summary>
        GET,

        /// <summary>
        /// 以Post方式。支持调用上下文。
        /// <para>以该方式时，可以应对以下情况：</para>
        /// <list type="bullet">
        /// <item>仅有一个参数时，该参数可以为任意类型，且参数来源为Body</item>
        /// <item>当有多个参数时，最后一个参数可以为任意类型，且参数来源为Body，其余参数均必须是基础类型，且来自url参数。</item>
        /// </list>
        /// </summary>
        POST
    }
}
