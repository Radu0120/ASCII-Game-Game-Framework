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
        public static Dictionary<int, string> DrawUI(Actor player, bool designer, int mapend)
        {
            Dictionary<int, string> UI = new Dictionary<int, string>();
            UI.Add(1,Color.Background(Color.Black) + PrintTiles(2) + Color.Foreground(Color.Yellow) + $" {player.Name}       ");
            UI.Add(3,Color.Background(Color.Black) + PrintTiles(1) + Color.Foreground(Color.Red)+"Health: " +PrintBar(player.HP)+"           ");
            UI.Add(5,Color.Background(Color.Black) + PrintTiles(1) + Color.Foreground(Color.LightBlue) + "Mana: " + PrintTiles(1) + PrintBar(player.Mana)+"          ");

            if (!designer)
            {
                UI.Add(15,Color.Background(Color.Black)+ PrintTiles(1) + Color.Foreground(Color.White) + PrintEquippedItem(player));
            }
            else
            {
                int position = mapend/2;
                PrintDesignerObject(UI, position);
                PrintDesignerUI(UI, position);
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
        public static void PrintDesignerUI(Dictionary<int, string> UI, int position)
        {
            switch (Designer.CurrentState)
            {
                case Designer.State.Menu:
                    PrintDesignerMenu(UI, position);
                    break;
                case Designer.State.Actor:
                    PrintDesignerItemIndex(Actor.actorIndex, UI, position);
                    break;
                case Designer.State.Item:
                    PrintDesignerItemIndex(Item.itemIndex, UI, position);
                    break;
                case Designer.State.WorldOject:
                    PrintDesignerItemIndex(WorldObject.worldobjectIndex, UI, position);
                    break;
                case Designer.State.Tile:
                    PrintDesignerItemIndex(Tile.tileIndex, UI, position);
                    break;
                default:
                    break;
            }
            UI.Add(MaxKey(UI)+2, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Esc : Go back    ");
        }
        public static void PrintDesignerObject(Dictionary<int, string> UI, int position)
        {
            if (Designer.Object == null)
            {
                if(Designer.Tile == null)
                {
                    UI.Add(position, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Building: None    ");
                    return;
                }
                UI.Add(position, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Building: " + Designer.Tile.Name+ "    ");
                return;
            }
            UI.Add(position, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Building: " + Designer.Object.Name+ "    ");
        }
        public static void PrintDesignerMenu(Dictionary<int, string> UI, int position)
        {
            UI.Add(position + 2, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "Z : Actor menu    ");
            UI.Add(position + 3, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "X : Item menu    ");
            UI.Add(position + 4, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "C : WorldObject menu    ");
            UI.Add(position + 5, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + "V : Tile menu    ");
        }
        public static void PrintDesignerItemIndex<T>(Dictionary<int, T> index, Dictionary<int, string> UI, int position)
        {
            int end = index.Count;
            if(index.Count>0)
            {
                switch (index[0])
                {
                    case Actor:
                        for (int i = 1; i <= end; i++)
                        {
                            Actor a = (Actor)(object)index[i-1];
                            UI.Add(position + 2 + i - 1, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + $"{i} : {a.Name}         "); ;
                        }
                        break;
                    case Item:
                        for (int i = 1; i <= end; i++)
                        {
                            Item a = (Item)(object)index[i-1];
                            UI.Add(position + 2 + i - 1, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + $"{i} : {a.Name}         "); ;
                        }
                        break;
                    case WorldObject:
                        for (int i = 1; i <= end; i++)
                        {
                            WorldObject a = (WorldObject)(object)index[i-1];
                            UI.Add(position + 2 + i - 1, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + $"{i} : {a.Name}         "); ;
                        }
                        break;
                    case Tile:
                        for (int i = 1; i <= end; i++)
                        {
                            Tile a = (Tile)(object)index[i-1];
                            UI.Add(position + 2 + i - 1, Color.Background(Color.Black) + Color.Foreground(Color.White) + PrintTiles(2) + $"{i} : {a.Name}         "); ;
                        }
                        break;
                    default:
                        break;

                }
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
