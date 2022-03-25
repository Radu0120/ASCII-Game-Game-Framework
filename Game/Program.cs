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
            //Actor player = new Actor(1, "pedritu", 'P', Color.Red, 105, 100, 10, 0, 0);
            //Map map = new Map(80, 48, 10, 10, player, 1);

            //RunGameLogic(map, player, false);
        }
        public static void StartDesigner()
        {
            Actor cursor = new Actor(1, "player", 'X', Color.Red, 105, 100, 10, 0, 0);
            cursor.Attributes.Add("Phase");
            cursor.Attributes.Add("Designer");

            string input = "";
            Level chosenlevel = new Level();
            while (input != "close")
            {
                Console.Clear();
                Console.WriteLine("Pick one of the levels below to edit, or type 'new' to create a new level, or 'close' to exit the game");
                for(int i = 0; i < Level.levelIndex.Count; i++)
                {
                    Console.WriteLine(Level.levelIndex[i]);
                }
                input = Console.ReadLine();
                if(input == "new")
                {
                    Console.WriteLine("What should your level be named?");
                    input = Console.ReadLine();
                    chosenlevel = new Level(Level.levelIndex.Count, input, cursor);
                    break;
                }
                else
                {
                    foreach (Level level in Level.levelIndex.Values)
                    {
                        if (input == level.Name)
                        {
                            chosenlevel = level;
                            break;
                        }
                    }
                }
            }
            if (input == "close")
            {
                return;
            }

            Designer.CurrentState = Designer.State.BuildMenu;

            RunGameLogic(cursor, true, chosenlevel);
        }
        private static void RunGameLogic(Actor player, bool designer, Level level)
        {
            Level.CurrentLevel = level.ID;
            Thread threadPlayer = new Thread(() =>
                        Controls.CheckInput(ref player)
                );
            threadPlayer.SetApartmentState(ApartmentState.STA);
            threadPlayer.Start();
            Console.Clear();
            
            int WindowHeight = level.GetCurrentMap().Bounds.X + 2;
            int WindowWidth = level.GetCurrentMap().Bounds.Y * 2 + 4 + 45;
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
                level.GetCurrentMap().Update();
                DrawGame(level.GetCurrentMap(), designer, player);
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
