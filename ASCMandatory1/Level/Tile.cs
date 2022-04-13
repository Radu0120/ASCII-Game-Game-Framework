using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    [Serializable]
    public class Tile
    {
        public int currententitytodraw = 0;
        public string Name { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        public List<string> Attributes { get; set; }
        public List<object> Entities { get; set; }
        public static Dictionary<int, Tile> tileIndex { get; set; } = new Dictionary<int, Tile>() { { 0, new Tile(0, "Void", ASCMandatory1.Color.Gray) } };
        public Tile(int id, string name, int[] color)
        {
            Id= id;
            Name= name;
            Color = ASCMandatory1.Color.Background(color);
            Entities = new List<object>();
            Attributes = new List<string> { "None" };
        }
        public Tile(int id, string name, int[] color, List<string>attributes)
        {
            Id= id;
            Name = name;
            Color = ASCMandatory1.Color.Background(color);
            Entities = new List<object>();
            foreach (string attribute in attributes)
            {
                Attributes.Add(attribute);
            }
        }
        public Tile() { }
        public void Blink(int blinkingtime, long frame) //method to calculate which entity the tile should show in case there is more than 1
        {
            if(currententitytodraw > this.Entities.Count - 1)
            {
                currententitytodraw = this.Entities.Count - 1;
            }
            if(frame % blinkingtime == 0)
            {
                if (currententitytodraw>0)
                {
                    currententitytodraw--;
                }
                else
                {
                    currententitytodraw = this.Entities.Count-1;
                }
            }
        }

    }
}
