using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCMandatory1
{
    public class Controls
    {

        public static void Move()
        {
            if (Keyboard.IsKeyDown(Key.W))
            {
                if (Keyboard.IsKeyDown(Key.A))
                {

                }
                else if (Keyboard.IsKeyDown(Key.D))
                {

                }
            }
            else if (Keyboard.IsKeyDown(Key.A))
            {
                if (Keyboard.IsKeyDown(Key.W))
                {

                }
                else if (Keyboard.IsKeyDown(Key.D))
                {

                }
            }
            else if (Keyboard.IsKeyDown(Key.S))
            {
                if (Keyboard.IsKeyDown(Key.A))
                {

                }
                else if (Keyboard.IsKeyDown(Key.D))
                {

                }
            }
            else if (Keyboard.IsKeyDown(Key.D))
            {
                if (Keyboard.IsKeyDown(Key.A))
                {

                }
                else if (Keyboard.IsKeyDown(Key.D))
                {

                }
            }
        }
    }
}
