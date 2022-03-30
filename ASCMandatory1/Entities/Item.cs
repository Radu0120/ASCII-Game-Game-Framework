using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Item:Entity
    {
        public static Dictionary<int, Item> itemIndex { get; set; } = new Dictionary<int, Item>();
        public Damage Damage { get; set; }
        public Item(int id, string name, char symbol, int[] color, Damage damage, Type type) : base(id, name, symbol, color, type)
        {
            ObjectType = type;
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = ASCMandatory1.Color.Foreground(color);
            Damage = damage;
        }
        public Item() { }
    }
}
