﻿using RRQMSocket.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.TouchRpc
{
    /// <summary>
    /// IHttpTouchRpcService
    /// </summary>
    public interface IHttpTouchRpcService:IHttpService, IRpcParser, IIDRpcActor
    {
    }
}
