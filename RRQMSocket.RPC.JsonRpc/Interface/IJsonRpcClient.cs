using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.JsonRpc
{
    /// <summary>
    /// IJsonRpcClient
    /// </summary>
    public interface IJsonRpcClient:ITcpClient,IRpcClient
    {
    }
}
