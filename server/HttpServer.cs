using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace server
{
    class HttpServer : IDisposable
    {
        #region Members
        private HttpServerState _state;
        public TcpListener _tcpListener;
        private object _syncLock = new object();
        private Dictionary<HttpClient, bool> _clients = new Dictionary<HttpClient, bool>();
        private AutoResetEvent _clientsChangedEvent = new AutoResetEvent(false);
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
            EndPoint = new IPEndPoint(IPAddress.Loopback, port);
            State = HttpServerState.Stopped;
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            ShutdownTimeOut = TimeSpan.FromSeconds(90);
            WriteTimeOut = TimeSpan.FromSeconds(90);
            ReadTimeOut = TimeSpan.FromSeconds(90);
            ServerBanner = String.Format("PUCMM_HTTP/{0}", GetType().Assembly.GetName().Version);



        }

        #region Methods
        public void Start()
        {
            VerifyState(HttpServerState.Stopped);
            TimeOutManager = new HttpTimeoutManager(this);
            _tcpListener = new TcpListener(EndPoint);
            ServerUtility = new HttpServerUtility();
            State = HttpServerState.Starting;
            try
            {
                _tcpListener.Start();
                EndPoint = (IPEndPoint)_tcpListener.LocalEndpoint;
                State = HttpServerState.Started;
                BeginAcceptTcpClient();
            }
            catch
            {
                Console.WriteLine("Server could not start.");
                State = HttpServerState.Stopped;
            }
        }

        public void Stop()
        {
            try
            {
                _tcpListener.Stop();
                State = HttpServerState.Stopped;
            }
            catch
            {
                Console.WriteLine("Server could not stop.");
                State = HttpServerState.Stopping;
            }
        }
        private void BeginAcceptTcpClient()
        {
            TcpListener listen = _tcpListener;

            if (listen == null)
            {
                Console.WriteLine("Local Listener null");
            }

            listen.BeginAcceptTcpClient(AcceptTcpClientCallback, listen);
        }

        internal void RaiseRequest(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            OnRequestReceived(new HttpRequestEventArgs(context));
        }

        internal bool RaiseUnhandledException(HttpContext context, Exception exception)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var e = new HttpExceptionEventArgs(context, exception);
            OnUnhandledException(e);
            return e.Handled;
        }

        internal void UnregisterClient(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("Client argument in HttpServer.UnregisterClient() method is null");
            }

            lock (_syncLock)
            {
                Debug.Assert(_clients.ContainsKey(client));
                _clients.Remove(client);
                _clientsChangedEvent.Set();
            }
        }

        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            var tcpListener = _tcpListener;

            if (tcpListener == null)
            {
                Console.WriteLine("Local Listener null");
                return;
            }

            var tcpClient = tcpListener.EndAcceptTcpClient(ar);

            if (State.ToString().Equals("Stopped"))
            {
                tcpClient.Close();
            }

            var httpClient = new HttpClient(this, tcpClient);

            RegisterClient(httpClient);

            httpClient.BeginRequest();

            BeginAcceptTcpClient();
            
        }

        private void RegisterClient(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("Server attempted to register is null");
            }
            lock (_syncLock)
            {
                _clients.Add(client, false);
                _clientsChangedEvent.Set();
            }
        }

        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                if (_state == HttpServerState.Started)
                    Stop();
                if (_clientsChangedEvent != null)
                {
                    ((IDisposable)_clientsChangedEvent).Dispose();
                    _clientsChangedEvent = null;
                }
                _disposed = true;
            }

            if (TimeOutManager != null)
            {
                TimeOutManager.Dispose();
                TimeOutManager = null;
            }
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
        public HttpServerState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnStateChanged(EventArgs.Empty);
            }
        }

        internal HttpServerUtility ServerUtility
        {
            get;
            private set;
        }

        internal HttpTimeoutManager TimeOutManager
        {
            get;
            private set;
        }

        public int ReadBufferSize
        {
            get { return _readBufferSize; }
            set { _readBufferSize = value; }
        }

        public int WriteBufferSize
        {
            get { return _writeBufferSize; }
            set { _writeBufferSize = value; }
        }

        public TimeSpan ReadTimeOut
        {
            get { return _readTime; }
            set { _readTime = value; }
        }

        public TimeSpan WriteTimeOut
        {
            get { return _writeTime; }
            set { _writeTime = value; }
        }

        public TimeSpan ShutdownTimeOut
        {
            get { return _shutDown; }
            set { _shutDown = value; }
        }

        public string ServerBanner
        {
            get { return _serverBanner; }
            set { _serverBanner = value; }
        }

        public IPEndPoint EndPoint { get; set; }
        #endregion

        #region Events
        public event HttpExceptionEventHandler UnhandledException;
        protected virtual void OnUnhandledException(HttpExceptionEventArgs args)
        {
            var ev = UnhandledException;
            if (ev != null)
            {
                ev(this, args);
            }
        }

        public event EventHandler StateChanged;
        protected virtual void OnStateChanged(EventArgs args)
        {
            var ev = StateChanged;
            if (ev != null)
            {
                ev(this, args);
            }
        }

        public event HttpRequestEventHandler RequestReceived;
        protected virtual void OnRequestReceived(HttpRequestEventArgs args)
        {
            var ev = RequestReceived;
            if (ev != null)
            {
                ev(this, args);
            }
        }

        #endregion
    }
}
