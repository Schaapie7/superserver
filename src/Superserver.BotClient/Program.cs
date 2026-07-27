using System.Net;
using System.Net.Sockets;
using System.Text;

var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
socket.Connect(new IPEndPoint(IPAddress.Loopback, 7113));
socket.ReceiveTimeout = 1000;
socket.Send(Encoding.UTF8.GetBytes("hello"));
