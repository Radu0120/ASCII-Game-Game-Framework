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
        public static bool playing = true;
        [STAThread]
        static void Main(string[] args)
        {
            Console.Clear();
            Catalog.Populate();
            string message = "Type 1 to start the game, or 2 to start the level designer";
            Console.WriteLine(message);

            bool stop = false;

            string input = Console.ReadLine().ToLower();
            while (!stop)
            {
                if (input == "1")
                {
                    StartGame();
                    stop = true;
                }
                else if (input == "2")
                {
                    StartDesigner();
                    stop = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(message);
                }
                if(!stop) input = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("Thank you for playing, press any key to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }
        public static void StartGame()
        {
            Actor player = new Actor(1, "player", 'P', Color.Red, 105, 100, 15, 0, 0, Entity.Type.Actor);
            player.Attributes.Add("Player");
            player.isAlive = true;
            string input = "";
            Level chosenlevel = null;
            while (input != "close" && chosenlevel == null)
            {
                Console.Clear();
                Console.WriteLine("Pick one of the levels below to play, or 'close' to exit the game");
                for (int i = 0; i < Level.levelIndex.Count; i++)
                {
                    Console.WriteLine(Level.levelIndex[i].Name);
                }
                input = Console.ReadLine();
                foreach (Level level in Level.levelIndex.Values)
                {
                    if (input == level.Name)
                    {
                        chosenlevel = level;
                        player.Position = Level.levelIndex[chosenlevel.ID].Maps[Level.levelIndex[chosenlevel.ID].StartingMap.X, Level.levelIndex[chosenlevel.ID].StartingMap.Y].SpawnPoint;
                        Level.levelIndex[chosenlevel.ID].Maps[Level.levelIndex[chosenlevel.ID].StartingMap.X, Level.levelIndex[chosenlevel.ID].StartingMap.Y].AddEntity(player, player.Position);
                        Level.Player = player;
                    }
                }
            }
            if (input == "close")
            {
                return;
            }
            RunGameLogic(player, false, chosenlevel);
        }
        public static void StartDesigner()
        {
            string input = "";
            Level chosenlevel = null;
            while (input != "close" && chosenlevel==null)
            {
                Console.Clear();
                Console.WriteLine("Pick one of the levels below to edit, or type 'new' to create a new level, or 'close' to exit the game");
                for(int i = 0; i < Level.levelIndex.Count; i++)
                {
                    Console.WriteLine(Level.levelIndex[i].Name);
                }
                input = Console.ReadLine();
                if(input == "new")
                {
                    Actor cursor = new Actor(1, "player", 'X', Color.Red, 105, 100, 20, 0, 0, Entity.Type.Actor);
                    cursor.isAlive = true;
                    cursor.Attributes.Add("Phase");
                    cursor.Attributes.Add("Designer");
                    cursor.Attributes.Add("Player");
                    Console.WriteLine("What should your level be named?");
                    input = Console.ReadLine();
                    chosenlevel = new Level(Level.levelIndex.Count, input, cursor);
                    Level.CurrentLevel = chosenlevel.ID;
                    Level.Player = chosenlevel.GetPlayer();
                    break;
                }
                else
                {
                    foreach (Level level in Level.levelIndex.Values)
                    {
                        if (input == level.Name)
                        {
                            chosenlevel = level;
                            Level.Player = chosenlevel.GetPlayer();
                        }
                    }
                }
            }
            if (input == "close")
            {
                return;
            }
            Designer.CurrentState = Designer.State.MainMenu;
            RunGameLogic(Level.Player, true, chosenlevel);
        }
        private static void RunGameLogic(Actor player, bool designer, Level level)
        {
            Level.CurrentLevel = level.ID;
            Thread ActionsThread = new Thread(() =>
                        Controls.CheckInput(ref player, ref playing)
                );
            ActionsThread.SetApartmentState(ApartmentState.STA);
            ActionsThread.Start();
            Thread MovementThread = new Thread(() =>
                        Controls.CheckMovement(ref player, ref playing)
                );
            MovementThread.SetApartmentState(ApartmentState.STA);
            MovementThread.Start();
            Console.Clear();
            Thread AIThread = new Thread(() =>
                        AI.Think()
                );
            AIThread.Start();

            int WindowHeight = level.GetCurrentMap().Bounds.X + 2;
            int WindowWidth = level.GetCurrentMap().Bounds.Y * 2 + 4 + 45;
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            while (playing)
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
            foreach(string line in map.DrawMap(designer))
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
