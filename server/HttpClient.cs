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
        private ClientState stateClient;
        private bool disposed;
        private HttpServer _server;
        private TcpClient _tcp;
        private static readonly Regex PrologRegex = new Regex("^([A-Z]+) ([^ ]+) (HTTP/[^ ]+)$", RegexOptions.Compiled);
        #endregion

        public HttpClient(HttpServer serv)
        {
            disposed = false;
            Server = serv;
        }

        public void BeginRequest() { }
        
        void IDisposable.Dispose()
        {
            disposed = true;
        }

        #region Properties
        public HttpServer Server { get { return _server; } private set { _server = value; } }
        public TcpClient TcpClient { get { return _tcp; } private set { _tcp = value; } }
        #endregion
    }
}
