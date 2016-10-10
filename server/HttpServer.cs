using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class HttpServer
    {
        public void _Start()
        {
            tcpListener = new TcpListener(ipPoint);
        }

        public void _Stop()
        {
            tcpListener.Stop();
        }

        private HttpServerState serverState;
        public TcpListener tcpListener;
        public IPEndPoint ipPoint;

        public HttpServer(int port)
        {
            ipPoint = new IPEndPoint(IPAddress.Loopback, port);
        }

    }
}
