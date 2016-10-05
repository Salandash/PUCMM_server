using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static int Main(string[] args)
        {
            string path;
            int port;

            if (args.GetLength(0) > 0)
            {
                if (args[0] == "--path")
                {
                    path = args[1];
                    port = 80;
                }

                else if (args[0] == "--port")
                {
                    port = Convert.ToInt16(args[1]);
                    path = System.Reflection.Assembly.GetEntryAssembly().Location;
                }

                else if (args[0] == "--path" && args[2] == "--port")
                {
                    path = args[1];
                    port = Convert.ToInt16(args[3]);
                }

                else if (args[2] == "--path" && args[0] == "--port")
                {
                    path = args[3];
                    port = Convert.ToInt16(args[1]);
                }
                else
                {
                    path = args[1];
                    port = Convert.ToInt16(args[3]);
                }
            }
            else
            {
                path = System.Reflection.Assembly.GetEntryAssembly().Location;
                port = 80;
            }
            Console.WriteLine("PATH: " + path);
            Console.WriteLine("PORT: " + port);
            return 0;
        }
    }
}
