using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class WorldObject:Entity
    {
        public WorldObject(int id, string name, char symbol, string color) : base(id, name, symbol, color)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
        }
    }
}
