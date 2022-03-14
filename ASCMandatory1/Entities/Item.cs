using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Item:Entity
    {
        public Damage Damage { get; set; }
        public Item(int id, string name, char symbol, string color, Damage damage) : base(id, name, symbol, color)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
            Damage = damage;
        }
    }
}
