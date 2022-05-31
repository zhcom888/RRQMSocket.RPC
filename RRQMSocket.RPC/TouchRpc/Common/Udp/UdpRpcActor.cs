using RRQMCore.Log;
using System.Net;

namespace RRQMSocket.RPC.TouchRpc
{
    internal class UdpRpcActor : RpcActor
    {
        private readonly UdpSessionBase m_udpSession;
        private readonly EndPoint m_endPoint;

        public int Tick;

        public UdpRpcActor(UdpSessionBase udpSession, EndPoint endPoint, ILog logger) : base(false, logger)
        {
            this.OutputSend = this.RpcActorSend;
            this.m_udpSession = udpSession;
            this.m_endPoint = endPoint;
        }

        private void RpcActorSend(bool isAsync, TransferByte[] transferBytes)
        {
            if (isAsync)
            {
                this.m_udpSession.SendAsync(this.m_endPoint, transferBytes);
            }
            else
            {
                this.m_udpSession.Send(this.m_endPoint, transferBytes);
            }
        }

    }
}
