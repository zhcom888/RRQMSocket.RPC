using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// ITcpTouchRpcService
    /// </summary>
    public interface ITcpTouchRpcService:ITcpService, IRpcParser, IIDRpcActor
    {
    }
}
