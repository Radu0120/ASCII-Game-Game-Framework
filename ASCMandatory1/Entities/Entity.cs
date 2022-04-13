using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Entity
    {
        public enum Type
        {
            Actor, Item, WorldObject, Projectile
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public char Symbol { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        public List<string> Attributes { get; set; }
        public Type ObjectType { get; set; }
        
        public Entity(int id, string name, char symbol, int[] color)
        {
            Attributes = new List<string>();
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = ASCMandatory1.Color.Foreground(color);
        }
        protected Entity(int id, string name, char symbol, int[] color, List<string> attributes)
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
        protected Entity(int id, string name)
        {
            Attributes = new List<string>();
            Id = id;
            Name = name;
        }
        public Entity()
        {
            Symbol = 'W';
            Color = ASCMandatory1.Color.Foreground(ASCMandatory1.Color.Black);
        }
    }
}
