using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{

    public enum ClientState
    {
            ReadingProlog,
            ReadingHeaders,
            ReadingContent,
            PrintingContent,
            WritingHeaders,
            WritingContent,
            Closed
    }
}
