using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class WorldObject:Entity
    {
        public static Dictionary<int, WorldObject> worldobjectIndex { get; set; } = new Dictionary<int, WorldObject>()
        {
            { 0, new WorldObject(0, "Wall", 'W', GameFramework.Color.Black, new List<string>(){"Solid"})}
        };
        public WorldObject(int id, string name, char symbol, int[] color, List<string> attributes) : base(id, name, symbol, color)
        {
            ObjectType = Type.WorldObject;
            Attributes = new List<string>();
            foreach (string attribute in attributes)
            {
                Attributes.Add(attribute);
            }
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = GameFramework.Color.Foreground(color);
        }
        public WorldObject() { }
    }
}
