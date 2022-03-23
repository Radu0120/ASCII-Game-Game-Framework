using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ASCMandatory1;

namespace Game
{
    internal class Program
    {
        //protected static string clearBuffer = null; // Clear this if window size changes
        static int count = 0;
        [STAThread]
        static void Main(string[] args)
        {
            Catalog.Populate();
            string message = "Type 1 to start the game, or 2 to start the level designer";
            Console.WriteLine(message);
            string input = Console.ReadLine().ToLower();
            while (true)
            {
                if (input == "1")
                {
                    StartGame();
                }
                else if (input == "2")
                {
                    StartDesigner();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(message);
                }
                input = Console.ReadLine();
            }
            
        }
        public static void StartGame()
        {
            Actor player = new Actor(1, "pedritu", 'P', Color.Red, 105, 100, 10, 0, 0);
            Map map = new Map(80, 48, 10, 10, player);

            RunGameLogic(map, player, false);
        }
        public static void StartDesigner()
        {
            Actor cursor = new Actor(1, "player", 'X', Color.Red, 105, 100, 10, 0, 0);
            cursor.Attributes.Add("Phase");
            cursor.Attributes.Add("Designer");

            Console.WriteLine("Type the name of the map");
            string name = Console.ReadLine();

            Console.WriteLine("Type the width of the map");
            int width = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Type the height of the map");
            int height = Int32.Parse(Console.ReadLine());

            Map newmap = new Map(width, height, 5, 5, cursor);
            //Designer.Object = Entity.Clone(Entity.entityIndex[0]);
            //Designer.Tile = Tile.Clone(Tile.tileIndex[0]);
            Designer.CurrentState = Designer.State.Menu;

            RunGameLogic(newmap, cursor, true);
        }
        private static void RunGameLogic(Map map, Actor player, bool designer)
        {
            Thread threadPlayer = new Thread(() =>
                        Controls.Checkinput(ref player, ref map)
                );
            threadPlayer.SetApartmentState(ApartmentState.STA);
            threadPlayer.Start();
            Console.Clear();
            int WindowHeight = map.Bounds.X + 2;
            int WindowWidth = map.Bounds.Y * 2 + 4 + 45;
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;
            while (true)
            {
                if (Console.WindowHeight != WindowHeight || Console.WindowWidth != WindowWidth)
                {
                    WindowHeight = Console.WindowHeight;
                    WindowWidth = Console.WindowWidth;
                    Console.Clear();
                }
                map.Update();
                DrawGame(map, designer, player);
                Thread.Sleep(16);
            }
        }
        private static void DrawGame(Map map, bool designer, Actor player)
        {
            
            Console.CursorVisible = false;
            Console.SetCursorPosition(2, 1);
            Dictionary<int,string> UI = ASCMandatory1.UI.DrawUI(player, designer, map.Bounds.X);
            int UILine = 0;
            foreach(string line in map.DrawMap(designer, ref count))
            {
                Console.Write(line);
                if (UILine <= UI.Count-1)
                {
                    Console.Write(UI[UILine]+"   ");
                    UILine++;
                }
                Console.Write(Color.Background(Color.Black) + "\n" + "  ");
            }
        }
    }
}
