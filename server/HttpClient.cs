using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class HttpClient : IDisposable
    {
        #region Members
        private ClientState _state;
        private bool _disposed;
        private HttpServer _server;
        private TcpClient _tcp;
        private static readonly Regex PrologRegex = new Regex("^([A-Z]+) ([^ ]+) (HTTP/[^ ]+)$", RegexOptions.Compiled);
        #endregion

        public HttpClient(HttpServer serv)
        {
            _disposed = false;
            Server = serv;
            _tcp = new TcpClient();
            ClientState = new ClientState();
        }

        public void BeginRequest() { }
        
        void IDisposable.Dispose()
        {
            _disposed = true;
        }

        #region Properties
        public HttpServer Server
        {
            get { return _server; }
            private set { _server = value;}
        }

        public TcpClient TcpClient
        {
            get { return _tcp; }
            private set { _tcp = value; }
        }

        public ClientState ClientState
        {
            get { return _state; }
            private set { _state = value; }
        }
        #endregion
    }
}
