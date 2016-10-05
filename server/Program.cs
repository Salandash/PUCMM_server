using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            int port;

            if (args.GetLength(0) > 0)
            {
                path = args[1];
                port = Convert.ToInt16(args[3]);
            }
            else
            {
                path = System.Reflection.Assembly.GetEntryAssembly().Location;
                port = 80;

            }

            Console.WriteLine("PATH: " + path);
            Console.WriteLine("POTR: " + port);
            Console.ReadLine();
        }
    }
}
