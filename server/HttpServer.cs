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
        private HttpServerState state;
        public TcpListener tcpListener;
        public IPEndPoint ipPoint;

        public HttpServer(int port)
        {
            ipPoint = new IPEndPoint(IPAddress.Loopback, port);
        }

        #region Methods
        public void _Start()
        {
            tcpListener = new TcpListener(ipPoint);
        }

        public void _Stop()
        {
            tcpListener.Stop();
        }
        #endregion

        #region Properties
        public HttpServerState serverState { get { return state; } set { state = value; } }
        #endregion

    }
}
