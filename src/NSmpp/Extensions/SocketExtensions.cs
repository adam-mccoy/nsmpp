using System.Net.Sockets;
using System.Threading.Tasks;

namespace NSmpp.Extensions
{
    internal static class SocketExtensions
    {
        internal static Task ConnectAsync(this Socket socket, string host, int port)
        {
            return Task.Factory.FromAsync(socket.BeginConnect(host, port, null, null), socket.EndConnect);
        }
    }
}
