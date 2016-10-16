using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace server
{
    class HttpServer : IDisposable
    {
        #region Members
        private HttpServerState _state;
        public TcpListener tcpListener;
        public IPEndPoint ipPoint;
        private object synLock = new object();
        private Dictionary<HttpClient, bool> _clients = new Dictionary<HttpClient, bool>();
        private AutoResetEvent clientsChangedEvent = new AutoResetEvent(false);
        private int _readBufferSize;
        private int _writeBufferSize;
        private TimeSpan _readTime;
        private TimeSpan _writeTime;
        private TimeSpan _shutDown;
        private string _serverBanner;
        private bool _disposed = false;
        #endregion

        public HttpServer(int port)
        {
            ipPoint = new IPEndPoint(IPAddress.Loopback, port);
            serverState = new HttpServerState();
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;


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

        private void BeginAcceptTcpClient() { }
        private void AcceptTcpClientCallback(IAsyncResult ar) { }
        private void RegisterClient(HttpServer serv) { }

        void IDisposable.Dispose()
        {
            _disposed = true;
        } 

        private void VerifyState(HttpServerState state)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (_state != state)
                throw new InvalidOperationException(String.Format("Expected server to be in the '{0}' state", state));
        }
        #endregion

        #region Properties
        public HttpServerState serverState { get { return _state; } set { _state = value; } }
        public int ReadBufferSize { get { return _readBufferSize; } set { _readBufferSize = value; } }
        public int WriteBufferSize { get { return _writeBufferSize; } set { _writeBufferSize = value; } }
        public TimeSpan ReadTimeOut { get { return _readTime; } set { _readTime = value; } }
        public TimeSpan WriteTimeOut { get { return _writeTime; } set { _writeTime = value; } }
        public TimeSpan ShutdownTimeOut { get { return _shutDown; } set { _shutDown = value; } }
        public string ServerBanner { get { return _serverBanner; } set { _serverBanner = value; } }
        #endregion

    }
}
