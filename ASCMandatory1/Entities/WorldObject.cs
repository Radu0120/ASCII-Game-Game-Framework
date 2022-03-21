using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class WorldObject:Entity
    {
        public static Dictionary<int, WorldObject> worldobjectIndex { get; set; } = new Dictionary<int, WorldObject>();
        public WorldObject(int id, string name, char symbol, int[] color) : base(id, name, symbol, color)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = ASCMandatory1.Color.Foreground(color);
        }
    }
}
