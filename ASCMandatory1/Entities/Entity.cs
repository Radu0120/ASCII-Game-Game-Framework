using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char Symbol { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        public List<string> Attributes { get; set; }
        public static Dictionary<int, Entity> entityIndex { get; } = new Dictionary<int, Entity>()
        { 
            { 
                0, new Entity(0, "Wall", 'W', ASCMandatory1.Color.Foreground(ASCMandatory1.Color.Black), new List<string>(){"Solid"}) 
            } 
        };
        public Entity(int id, string name, char symbol, string color)
        {
            Attributes = new List<string>();
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
        }
        public Entity(int id, string name, char symbol, string color, List<string> attributes)
        {
            Attributes = new List<string>();
            foreach (string attribute in attributes)
            {
                Attributes.Add(attribute);
            }
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
        }
        public Entity()
        {
            Symbol = 'W';
            Color = ASCMandatory1.Color.Foreground(ASCMandatory1.Color.Black);
        }
        public static Entity Clone(Entity entity)
        {
            Entity clone = new Entity();
            clone.Id = entity.Id;
            clone.Name = entity.Name;
            clone.Color = entity.Color;
            clone.Symbol = entity.Symbol;
            clone.Position = entity.Position;
            clone.Attributes = new List<string>();
            if (entity.Attributes != null)
            {
                foreach (string attribute in entity.Attributes)
                {
                    clone.Attributes.Add(attribute);
                }
            }


            return clone;
        }
    }
}
