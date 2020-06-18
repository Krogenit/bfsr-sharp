using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Core(1024, 600, false, true))
            //using (var game = new Core(0, 0, false,false))
            {
                game.Run();
            }
        }
    }
}
