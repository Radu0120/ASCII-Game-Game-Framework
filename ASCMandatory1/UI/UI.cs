using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class UI
    {
        public const int AmountPerBar = 5;
        public static List<string> DrawUI(Actor player, bool designer)
        {
            List<string> UI = new List<string>();
            UI.Add(Color.Background(Color.Black) + PrintTiles(5) + Color.Foreground(Color.Yellow) + $" {player.Name}");
            UI.Add("");
            UI.Add(Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.Red)+"Health: " +PrintBar(player.HP));
            UI.Add("");
            UI.Add(Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.LightBlue) + "Mana: " + PrintTiles(1) + PrintBar(player.Mana));

            if (!designer)
            {

            }

            return UI;
        }
        public static string PrintTiles(int number)
        {
            return string.Concat(Enumerable.Repeat("  ", number));
        }
        public static string PrintBar(double value)
        {
            //{player.HP}/{player.MaxHP}
            string bar = "";
            while (value != 0)
            {
                if (value - AmountPerBar >= 0)
                {
                    bar += "\u2588";
                    value -= AmountPerBar;
                }
                else
                {
                    bar += "\u258C";
                    value = 0;
                }
            }
            return bar;
        }
        public static string PrintEquippedItem(Actor player)
        {
            return "a";
        }
    }
}
