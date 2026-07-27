
using System.Net;

namespace SuperServer.Shared
{
    public readonly struct Datagram
    {
        public readonly ReadOnlyMemory<byte> Payload;
        public readonly IPEndPoint Sender;
        public readonly long ReceiveTimestamp; 

        public Datagram(ReadOnlyMemory<byte> payload, IPEndPoint sender, long receiveTimestamp)
        {
            this.Payload = payload;
            this.Sender = sender;
            this.ReceiveTimestamp = receiveTimestamp;
        }
    }
}
