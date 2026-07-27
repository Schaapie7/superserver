// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using SuperServer.Shared;

var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
socket.Bind(new IPEndPoint(IPAddress.Any, 7113));

Channel<Datagram> queue = Channel.CreateBounded<Datagram>(new BoundedChannelOptions(256) {FullMode = BoundedChannelFullMode.DropOldest, SingleReader = true, SingleWriter = true});

Thread recieveThread = new Thread(Receive);

recieveThread.Start();

await foreach(var msg in queue.Reader.ReadAllAsync())
{
    System.Console.WriteLine($"Received {msg.Payload.Length} bytes from {msg.Sender}");
    socket.SendTo(msg.Payload.Span, msg.Sender);
}

void Receive()
{
    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
    while(true)
    {
        try
        {
            Byte[] buffer = new Byte[256];
            int received = socket.ReceiveFrom(buffer, ref remoteEP);
            var datagram = new Datagram(buffer.AsMemory(0, received), (IPEndPoint)remoteEP, Stopwatch.GetTimestamp());
            AddToQueue(datagram);
        }
        catch (SocketException e)
        {
        }
    }
}

void AddToQueue(Datagram datagram)
{
    queue.Writer.TryWrite(datagram);
}