using System;
using Final_Year_Project.Networking;


namespace Server
{
    class Program
    {
        static void Main(string[] args) 
        {
            try
            {
                ServerMain server = new ServerMain(false, 4444);
                ServerMain.Start();
                ServerMain.Stop();
            }
            catch (Exception e)
            {
                Console.Write("Error occured. Possible port clash");
            }
        }
    }
}
