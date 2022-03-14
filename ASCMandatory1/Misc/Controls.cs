using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCMandatory1
{
    public class Controls
    {
        public static void Checkinput(ref Actor player)
        {
            while (true)
            {
                if (IsAnyKeyDown())
                {
                    Move(ref player);
                    int wait = Convert.ToInt32(1000 / player.Speed);
                    Thread.Sleep(wait);
                    
                }
            }
        }
        public static void Move(ref Actor actor)
        {
            Position newposition = new Position() { X = actor.Position.X, Y = actor.Position.Y};
            if (Keyboard.IsKeyDown(Key.W))
            {
                if (Keyboard.IsKeyDown(Key.A))
                {
                    newposition.X--;
                    newposition.Y--;
                }
                else if (Keyboard.IsKeyDown(Key.D))
                {
                    newposition.X--;
                    newposition.Y++;
                }
                else
                {
                    newposition.X--;
                }
            }
            else if (Keyboard.IsKeyDown(Key.A))
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    newposition.X++;
                    newposition.Y--;
                }
                else
                {
                    newposition.Y--;
                }
            }
            else if (Keyboard.IsKeyDown(Key.S))
            {
                if (Keyboard.IsKeyDown(Key.D))
                {
                    newposition.X++;
                    newposition.Y++;
                }
                else
                {
                    newposition.X++;
                }
            }
            else if (Keyboard.IsKeyDown(Key.D))
            {
                newposition.Y++;
            }
            actor.PendingMovement = newposition;
        }
        public static bool IsAnyKeyDown()
        {
            var values = Enum.GetValues(typeof(Key));

            foreach (var v in values)
                if (((Key)v) != Key.None && Keyboard.IsKeyDown((Key)v))
                    return true;

            return false;
        }
    }
}
