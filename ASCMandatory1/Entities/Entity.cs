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
        public Entity(int id, string name, char symbol, string color)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
        }
    }
}
