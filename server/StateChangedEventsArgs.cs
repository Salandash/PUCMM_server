using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class StateChangedEventsArgs : EventArgs
    {
        public StateChangedEventsArgs(HttpServerState old, HttpServerState current)
        {
            NewState = current;
            OldState = old;
        }
        #region Properties
        public HttpServerState NewState
        {
            get;
            private set;
        }
        public HttpServerState OldState
        {
            get;
            private set;
        }

        #endregion
    }
}
