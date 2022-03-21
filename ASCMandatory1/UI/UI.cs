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
        public static Dictionary<int, string> DrawUI(Actor player, bool designer)
        {
            Dictionary<int, string> UI = new Dictionary<int, string>();
            UI.Add(1,Color.Background(Color.Black) + PrintTiles(5) + Color.Foreground(Color.Yellow) + $" {player.Name}");
            UI.Add(3,Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.Red)+"Health: " +PrintBar(player.HP));
            UI.Add(5,Color.Background(Color.Black) + PrintTiles(3) + Color.Foreground(Color.LightBlue) + "Mana: " + PrintTiles(1) + PrintBar(player.Mana));

            if (!designer)
            {
                UI.Add(15,Color.Background(Color.Black)+ PrintTiles(3) + Color.Foreground(Color.White) + PrintEquippedItem(player));
            }
            else
            {
                PrintLines(UI, 14);
                PrintDesignerUI(UI);
                PrintDesignerObject(UI);
            }
            FillUI(UI);
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
        public static void FillUI(Dictionary<int, string> UI)
        {
            for(int i = 0; i < 41 ; i++)
            {
                if (!UI.ContainsKey(i))
                {
                    PrintLines(UI, i);
                }
            }
        }
        public static void PrintLines(Dictionary<int, string> UI, int position)
        {
            UI.Add(position, Color.Background(Color.Black) + Color.Foreground(Color.White) + "                          ");
        }
        public static string PrintEquippedItem(Actor player)
        {
            if(player.EquippedWeapon != null)
            {
                return player.EquippedWeapon.Name;
            }
            else
            {
                return "No equipped item";
            }
        }
        public static void PrintDesignerUI(Dictionary<int, string> UI)
        {
            switch (Designer.CurrentState)
            {
                case Designer.State.Menu:
                    PrintDesignerMenu(UI);
                    break;
                case Designer.State.Actor:
                    PrintDesignerItemIndex(Actor.actorIndex, UI);
                    break;
                case Designer.State.Item:
                    PrintDesignerItemIndex(Item.itemIndex, UI);
                    break;
                case Designer.State.WorldOject:
                    PrintDesignerItemIndex(WorldObject.worldobjectIndex, UI);
                    break;
                case Designer.State.Tile:
                    PrintDesignerItemIndex(Tile.tileIndex, UI);
                    break;
                default:
                    break;
            }
            UI.Add(MaxKey(UI)+2, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Esc : Go back    ");
        }
        public static void PrintDesignerObject(Dictionary<int, string> UI)
        {
            if (Designer.Object == null)
            {
                if(Designer.Tile == null)
                {
                    UI.Add(15, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "None    ");
                    return;
                }
                UI.Add(15, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + Designer.Tile.Name+ "    ");
                return;
            }
            UI.Add(15, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + Designer.Object.Name+ "    ");
        }
        public static void PrintDesignerMenu(Dictionary<int, string> UI)
        {
            UI.Add(17, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Z : Actor menu    ");
            UI.Add(18, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "X : Item menu    ");
            UI.Add(19, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "C : WorldObject menu    ");
            UI.Add(20, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "V : Tile menu    ");
        }
        public static void PrintDesignerItemIndex(Dictionary<int, Actor> index, Dictionary<int, string> UI)
        {
            int end = index.Count;
            for (int i = 1; i <= end; i++)
            {
                UI.Add(17 + i - 1,Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + $"{i} : {index[i - 1].Name}         "); ;
            }
        }
        public static void PrintDesignerItemIndex(Dictionary<int, Item> index, Dictionary<int, string> UI)
        {
            int end = index.Count;
            for (int i = 1; i <= end; i++)
            {
                UI.Add(17 + i - 1, Color.Background(Color.Black)+Color.Foreground(Color.White)+ PrintTiles(2) + $"{i} : {index[i - 1].Name}          ");
            }
        }
        public static void PrintDesignerItemIndex(Dictionary<int, WorldObject> index, Dictionary<int, string> UI)
        {
            int end = index.Count;
            for (int i = 1; i <= end; i++)
            {
                UI.Add(17 + i - 1, Color.Background(Color.Black)+Color.Foreground(Color.White)+ PrintTiles(2) + $"{i} : {index[i - 1].Name}          ");
            }
        }
        public static void PrintDesignerItemIndex(Dictionary<int, Tile> index, Dictionary<int, string> UI)
        {
            int end = index.Count;
            for (int i = 1; i <= end; i++)
            {
                UI.Add(17 + i - 1, Color.Background(Color.Black)+Color.Foreground(Color.White)+ PrintTiles(2) + $"{i} : {index[i - 1].Name}          ");
            }
        }
        public static int MaxKey(Dictionary<int, string> dict)
        {
            int max = 0;
            foreach(var keyValuePair in dict)
            {
                if(keyValuePair.Key>max) max = keyValuePair.Key;
            }
            return max;
        }
    }
}
