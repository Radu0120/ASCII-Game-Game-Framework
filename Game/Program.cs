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
            Console.WriteLine("Type play to start the game, or level to start the level designer");
            string input = Console.ReadLine().ToLower();
            while (true)
            {
                if (input == "play")
                {
                    StartGame();
                }
                else if (input == "level")
                {
                    StartDesigner();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Type play to start the game, or level to start the level designer");
                }
                input = Console.ReadLine();
            }
            
        }
        public static void StartGame()
        {
            Actor player = new Actor(1, "pedritu", 'P', Color.Foreground(Color.Red), 105, 100, 10, 0, 0);
            Level level = new Level("level1", 80, 48, 10, 10, player);

            RunGameLogic(level, player, false);
        }
        public static void StartDesigner()
        {
            Actor cursor = new Actor(1, "player", 'X', Color.Foreground(Color.Red), 105, 100, 10, 0, 0);
            cursor.Attributes.Add("Phase");
            cursor.Attributes.Add("Designer");

            Console.WriteLine("Type the name of the level");
            string name = Console.ReadLine();

            Console.WriteLine("Type the width of the level");
            int width = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Type the height of the level");
            int height = Int32.Parse(Console.ReadLine());

            Level newlevel = new Level(name, width, height, 5, 5, cursor);
            Designer.Object = Entity.Clone(Entity.entityIndex[0]);

            RunGameLogic(newlevel, cursor, true);
        }
        private static void RunGameLogic(Level level, Actor player, bool designer)
        {
            Thread thread = new Thread(() =>
                        Controls.Checkinput(ref player, ref level)
                );
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Console.Clear();
            int WindowHeight = level.Bounds.X + 2;
            int WindowWidth = level.Bounds.Y * 2 + 4 + 40;
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
                level.Update(player, ref count);
                DrawGame(level, designer, player);
                Thread.Sleep(16);
            }
        }
        private static void DrawGame(Level level, bool designer, Actor player)
        {
            
            Console.CursorVisible = false;
            //Console.SetCursorPosition(0, 0);
            //Console.Write(level.UnDrawLevel());
            Console.SetCursorPosition(2, 1);
            List<string> UI = ASCMandatory1.UI.DrawUI(player, designer);
            int UILine = 0;
            foreach(string line in level.DrawLevel(designer, ref count))
            {
                Console.Write(line);
                if (UILine <= UI.Count-1)
                {
                    Console.Write(UI[UILine]);
                    UILine++;
                }
                Console.Write(Color.Background(Color.Black) + "\n" + "  ");
            }
        }
    }
}
