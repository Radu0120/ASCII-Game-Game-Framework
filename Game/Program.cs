using System;
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

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        [STAThread]
        static void Main(string[] args)
        {
            
            Actor player = new Actor(1, "player" ,'P', Color.Foreground(Color.Red), 100, 100, 10, 0, 0);
            Level level = new Level(20, 20, 10, 10, player);

            Thread thread = new Thread(() =>
                        Controls.Checkinput(ref player)
                );
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (true)
            {
                
                level.Update(player);
                level.DrawLevel();
                Thread.Sleep(6);
                Program.ClearConsole();


            }
        }
        protected static string clearBuffer = null; // Clear this if window size changes
        protected static void ClearConsole()
        {
            Console.CursorVisible = false;
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
