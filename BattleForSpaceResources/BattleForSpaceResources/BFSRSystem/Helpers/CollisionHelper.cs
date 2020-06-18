using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys.BFSRSystem.Helpers
{
    public class CollisionHelper
    {
        public static int[] GetNearXY(int i, int x1, int y1)
        {
            int[] xy = new int[2];
            xy[0] = x1;
            xy[1] = y1;
            switch (i)
            {
                case 1:
                    xy[0] = x1 + 1;
                    break;
                case 2:
                    xy[0] = x1 - 1;
                    break;
                case 3:
                    xy[1] = y1 + 1;
                    break;
                case 4:
                    xy[1] = y1 - 1;
                    break;
                case 5:
                    xy[0] = x1 + 1;
                    xy[1] = y1 + 1;
                    break;
                case 6:
                    xy[0] = x1 - 1;
                    xy[1] = y1 + 1;
                    break;
                case 7:
                    xy[0] = x1 + 1;
                    xy[1] = y1 - 1;
                    break;
                case 8:
                    xy[0] = x1 - 1;
                    xy[1] = y1 - 1;
                    break;
            }
            return xy;
        }
        public static int[] GetMapNearXY(int i, int x1, int y1)
        {
            int[] xy = new int[2];
            xy[0] = x1;
            xy[1] = y1;
            switch (i)
            {
                case 1:
                    xy[0] = x1 + 2;
                    break;
                case 2:
                    xy[0] = x1 - 2;
                    break;
                case 3:
                    xy[1] = y1 + 2;
                    break;
                case 4:
                    xy[1] = y1 - 2;
                    break;
                case 5:
                    xy[0] = x1 + 1;
                    xy[1] = y1 + 2;
                    break;
                case 6:
                    xy[0] = x1 + 2;
                    xy[1] = y1 + 2;
                    break;
                case 7:
                    xy[0] = x1 - 1;
                    xy[1] = y1 + 2;
                    break;
                case 8:
                    xy[0] = x1 - 2;
                    xy[1] = y1 + 2;
                    break;
                case 9:
                    xy[0] = x1 + 1;
                    xy[1] = y1 - 2;
                    break;
                case 10:
                    xy[0] = x1 + 2;
                    xy[1] = y1 - 2;
                    break;
                case 11:
                    xy[0] = x1 - 1;
                    xy[1] = y1 - 2;
                    break;
                case 12:
                    xy[0] = x1 - 2;
                    xy[1] = y1 - 2;
                    break;
                case 13:
                    xy[0] = x1 - 2;
                    xy[1] = y1 - 1;
                    break;
                case 14:
                    xy[0] = x1 - 2;
                    xy[1] = y1 + 1;
                    break;
                case 15:
                    xy[0] = x1 + 2;
                    xy[1] = y1 - 1;
                    break;
                case 16:
                    xy[0] = x1 + 2;
                    xy[1] = y1 + 1;
                    break;
            }
            return xy;
        }
    }
}
