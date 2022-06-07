using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class MouseState
    {
        public int X;
        public int Y;
        public enum ButtonState
        {
            LMBPressed = 1, RMBPressed = 2
        }
        public ButtonState PressedButton;
        public MouseState(int x, int y, int buttonstate)
        {
            X = x;
            Y = y;
            PressedButton = (ButtonState)buttonstate;
        }
    }
}
