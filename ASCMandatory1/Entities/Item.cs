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
        public int AttackRange { get; set; }
        public int ProjectileSpeed { get; set; }
        public char ProjectileSymbol { get; set; }
        public string ProjectileColor { get; set; }
        public Item(int id, string name, char symbol, char projectileSymbol, int[] color, int[]projectileColor, int projectileSpeed, Damage damage, int range) : base(id, name, symbol, color)
        {
            Attributes = new List<string>();
            Attributes.Add("Phase");
            ObjectType = Type.Item;
            Id = id;
            Name = name;
            ProjectileSpeed = projectileSpeed;
            Symbol = symbol;
            ProjectileSymbol = projectileSymbol;
            AttackRange = range;
            Color = ASCMandatory1.Color.Foreground(color);
            ProjectileColor = ASCMandatory1.Color.Foreground(projectileColor);
            Damage = damage;
        }
        public Item() { }
    }
}
