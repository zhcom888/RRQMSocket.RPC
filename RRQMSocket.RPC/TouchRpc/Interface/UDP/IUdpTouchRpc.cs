using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// IUdpTouchRpc
    /// </summary>
    public interface IUdpTouchRpc:IUdpSession, IRpcParser, IRpcClient, ITouchRpc
    {
    }
}
