using RRQMSocket.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.WebApi
{
    /// <summary>
    /// IWebApiClient
    /// </summary>
    public interface IWebApiClient : IRpcClient, IHttpClient
    {
    }
}
