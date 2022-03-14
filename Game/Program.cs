using System;
using System.Runtime.InteropServices;
using System.Text;
using ASCMandatory1;

namespace Game
{
    internal class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        static void Main(string[] args)
        {
            Console.CursorVisible=false;
            Actor player = new Actor(1, "player" ,'P', Color.Foreground(Color.Red), 100, 100, 1, 0, 0);
            Level level = new Level(20, 20, 10, 10);
            while (true)
            {
                Program.ClearConsole();
                level.DrawLevel();
                Console.ReadKey(true);
            }
        }
        protected static string clearBuffer = null; // Clear this if window size changes
        protected static void ClearConsole()
        {
            if (clearBuffer == null)
            {
                var line = "".PadLeft(Console.WindowWidth, ' ');
                var lines = new StringBuilder();

                for (var i = 0; i < Console.WindowHeight; i++)
                {
                    lines.AppendLine(line);
                }

                clearBuffer = lines.ToString();
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(clearBuffer);
            Console.SetCursorPosition(0, 0);
        }
    }
}
