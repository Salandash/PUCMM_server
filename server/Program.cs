using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        public void _checkport(int port)
        {
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            if (isAvailable)
            {
                Console.WriteLine("PORT UNUSED");
            }
            else
            {
                Console.WriteLine("PORT USED");
            }

        }

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

                    if (!Directory.Exists(path))
                    {
                        Console.WriteLine("PATH inexistente: " + path);
                        return 1;
                    }

                }

                else if (args[0] == "--port")
                {
                    int i;
                    bool b = Int32.TryParse(args[1], out i);

                    if(!b)
                    {
                        Console.WriteLine("Puerto no es un número");
                        return 1;
                    }

                    port = Convert.ToInt16(args[1]);
                    path = System.Reflection.Assembly.GetEntryAssembly().Location;

                }

                else if (args[0] == "--path" && args[2] == "--port")
                {
                    int i;
                    bool b = Int32.TryParse(args[2], out i);

                    if (!b)
                    {
                        Console.WriteLine("PORT is not a number");
                        return 1;
                    }

                    path = args[1];
                    port = Convert.ToInt16(args[3]);

                    if (!Directory.Exists(path))
                    {
                        Console.WriteLine("PATH does not exist: " + path);
                        return 1;
                    }
                    
                }

                else if (args[2] == "--path" && args[0] == "--port")
                {
                    int i;
                    bool b = Int32.TryParse(args[1], out i);

                    if (!b)
                    {
                        Console.WriteLine("PORT is not a number");
                        return 1;
                    }

                    path = args[3];
                    port = Convert.ToInt16(args[1]);

                    if (!Directory.Exists(path))
                    {
                        Console.WriteLine("PATH does not exist: " + path);
                        return 1;
                    }

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
            Program o = new Program();
            o._checkport(port);
            return 0;
        }
    }
}
