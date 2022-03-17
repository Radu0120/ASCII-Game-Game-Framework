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
        protected static string clearBuffer = null; // Clear this if window size changes
        static int count = 0;
        const int blinkingtime = 20;
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Type play to start the game, or level to start the level designer");
            string input = Console.ReadLine().ToLower();
            if(input == "play")
            {
                StartGame();
            }
            else if(input == "level")
            {
                StartDesigner();
            }
        }
        public static void StartGame()
        {
            Actor player = new Actor(1, "player", 'P', Color.Foreground(Color.Red), 100, 100, 10, 0, 0);
            Level level = new Level("level1", 20, 20, 10, 10, player);

            RunGameLogic(level, player, false);
        }
        public static void StartDesigner()
        {
            Actor cursor = new Actor(1, "player", 'X', Color.Foreground(Color.Red), 100, 100, 10, 0, 0);
            cursor.Attributes.Add("Phase");

            Console.WriteLine("Type the name of the level");
            string name = Console.ReadLine();

            Console.WriteLine("Type the width of the level");
            int width = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Type the height of the level");
            int height = Int32.Parse(Console.ReadLine());

            Level newlevel = new Level(name, width, height, 10, 10, cursor);
            Designer.Object = Entity.Clone(Entity.entityIndex[0]);

            RunGameLogic(newlevel, cursor, true);
        }
        //protected static void ClearConsole()
        //{
        //    Console.CursorVisible = false;
        //    if (clearBuffer == null)
        //    {
        //        var line = "".PadLeft(Console.WindowWidth, ' ');
        //        var lines = new StringBuilder();

        //        for (var i = 0; i < Console.WindowHeight; i++)
        //        {
        //            lines.AppendLine(line);
        //        }

        //        clearBuffer = lines.ToString();
        //    }

        //    Console.SetCursorPosition(0, 0);
        //    Console.Write(clearBuffer);
        //    Console.SetCursorPosition(0, 0);
        //}

        private static void RunGameLogic(Level level, Actor player, bool designer)
        {
            Thread thread = new Thread(() =>
                        Controls.Checkinput(ref player, ref level)
                );
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Console.Clear();
            int WindowHeight = level.Bounds.Y + 2 + 10;
            int WindowWidth = level.Bounds.X * 2 + 4 + 40;
            while (true)
            {
                if (Console.WindowHeight != WindowHeight || Console.WindowWidth != WindowWidth)
                {
                    WindowHeight = Console.WindowHeight;
                    WindowWidth = Console.WindowWidth;
                    Console.Clear();
                }
                level.Update(player);
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
            List<string> UI = ASCMandatory1.UI.DrawUI(player);
            int UILine = 0;
            foreach(string line in level.DrawLevel(designer, ref count, blinkingtime))
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
