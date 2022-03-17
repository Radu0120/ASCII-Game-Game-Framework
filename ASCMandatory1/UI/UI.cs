using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class UI
    {
        public static List<string> DrawUI(Actor player)
        {
            List<string> UI = new List<string>();
            UI.Add(Color.Background(Color.Black) + PrintTiles(5) + Color.Foreground(Color.Yellow) + $" {player.Name}\u258C");
            UI.Add("");
            UI.Add(Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.Red)+"Health:" + Color.Foreground(Color.White)+$" {player.HP}/{player.MaxHP}");
            UI.Add(Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.LightBlue) + "Mana:" + Color.Foreground(Color.White) + $" {player.Mana}/{player.MaxMana}");
            
            return UI;
        }
        public static string PrintTiles(int number)
        {
            return string.Concat(Enumerable.Repeat("  ", number));
        }
    }
}
