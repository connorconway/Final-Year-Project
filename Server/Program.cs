using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server(false, 4444);
                Server.Start();
                Server.Stop();
            }
            catch (Exception)
            {
                Console.Write("The server can not start. Possible port clash. \n");
            }  
        }
    }
}
