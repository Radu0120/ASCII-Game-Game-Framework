using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class WorldObject:Entity
    {
        public static Dictionary<int, WorldObject> worldobjectIndex { get; set; } = new Dictionary<int, WorldObject>()
        {
            { 0, new WorldObject(0, "Wall", 'W', ASCMandatory1.Color.Black, new List<string>(){"Solid"}) }
        };
        public WorldObject(int id, string name, char symbol, int[] color, List<string> attributes) : base(id, name, symbol, color)
        {
            Attributes = new List<string>();
            foreach (string attribute in attributes)
            {
                Attributes.Add(attribute);
            }
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = ASCMandatory1.Color.Foreground(color);
        }
        public WorldObject() { }
    }
}
