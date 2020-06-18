using BattleForSpaceResources.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFSRServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerCore sc = new ServerCore(true);
            while(true)
            {
                if(Console.ReadLine().Equals("stop"))
                {
                    sc.Stop();
                    break;
                }
            }
        }
    }
}