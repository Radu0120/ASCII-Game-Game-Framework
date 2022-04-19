using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position() { }
        public static Position Create(int x, int y)
        {
            return new Position() { X = x, Y = y };
        }
        public static bool AreEqual(Position pos1, Position pos2)
        {
            if(pos1.X == pos2.X && pos1.Y == pos2.Y)
            {
                return true;
            }
            else return false;
        }
    }
}
