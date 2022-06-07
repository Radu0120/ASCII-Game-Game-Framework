using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Button
    {
        public Position Position { get; set; }
        public int Lenght { get; set; }
        public string Text { get; set; }
        public ConsoleColor BGColor { get; set; }
        public ConsoleColor FGColor { get; set; }
        public int Thickness { get; set; }
        //public Action Method { get; set; }

        public enum ButtonState
        {
            Pressed, NotPressed, Hover
        }
        public ButtonState State { get; set; }
        public Button(Position position, ConsoleColor bg, ConsoleColor fg, string text, int thickness)
        {
            State = ButtonState.NotPressed;
            Position = position;
            Lenght = text.Length;
            BGColor = bg;
            FGColor = fg;
            Text = text;
            if (thickness < 0)
            {
                Thickness = 0;
            }
            else if (thickness > 2)
            {
                Thickness = 2;
            }
            else Thickness = thickness;
        }
        public bool IsPressed(int x, int y, int mousestate)
        {
            if(mousestate == 1 && x >= Position.X - 2*Thickness && x<= Position.X+Lenght - 1 + 2*Thickness && y >= Position.Y-Thickness && y<= Position.Y+Thickness)
            {
                return true;
            }
            return false;
        }

        public void DrawButton()
        {
            ConsoleColor initialbg = Console.ForegroundColor;
            ConsoleColor initialfg = Console.BackgroundColor;

            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = FGColor;
            Console.BackgroundColor = BGColor;

            Console.Write(Text);

            for(int i = Position.Y - Thickness; i <= Position.Y + Thickness; i++)
            {
                for (int j = Position.X - 2 * Thickness; j <= Position.X + Lenght - 1 + 2*Thickness; j++)
                {
                    Console.SetCursorPosition(j,i);
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(Text);

            Console.ForegroundColor = initialbg;
            Console.BackgroundColor = initialfg;
        }
    }
}
