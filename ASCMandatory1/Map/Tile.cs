using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Tile
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        public List<string> Attributes { get; set; }
        public char Symbol { get; set; }
        public List<Entity> Entities { get; set; }
        public static Dictionary<int, Tile> tileIndex { get; set; } = new Dictionary<int, Tile>() { { 0, new Tile(0, "Void", ASCMandatory1.Color.Gray, ' ') } };
        public Tile(int id, string name, int[] color, char symbol)
        {
            Id= id;
            Name= name;
            Color = ASCMandatory1.Color.Background(color);
            Symbol= symbol;
            Entities = new List<Entity>();
            Attributes = new List<string> { "None" };
        }
        public Tile(int id, string name, int[] color, char symbol, List<string>attributes)
        {
            Id= id;
            Name = name;
            Color = ASCMandatory1.Color.Background(color);
            Symbol = symbol;
            Entities = new List<Entity>();
            Attributes = attributes;
        }
        public Tile() { }
        public static Tile Clone(Tile tile)
        {
            Tile clone = new Tile();
            clone.Id = tile.Id;
            clone.Name = tile.Name;
            clone.Color = tile.Color;
            clone.Symbol = tile.Symbol;
            clone.Attributes = tile.Attributes;
            clone.Entities = new List<Entity>();
            foreach (Entity entity in tile.Entities)
            {
                clone.Entities.Add(entity);
            }

            return clone;
        }

    }
}
