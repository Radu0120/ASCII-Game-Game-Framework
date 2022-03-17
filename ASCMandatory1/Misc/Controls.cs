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
        public static void Checkinput(ref Actor player, ref Level level)
        {
            while (true)
            {
                if (IsAnyKeyDown())
                {
                    Move(ref player);
                    DoDesignerAction(ref player, ref level);
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
                newposition.X--;
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                newposition.X++;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                newposition.Y++;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                newposition.Y--;
            }
            actor.PendingMovement = newposition;
        }
        public static void DoDesignerAction(ref Actor actor, ref Level level)
        {
            Action action = new Action();
            if (Keyboard.IsKeyDown(Key.Back))
            {
                action.Type = Action.ActionType.Destroy;
                action.Target = level.GetEntityFromPosition(actor.Position);
            }
            else if (Keyboard.IsKeyDown(Key.Enter))
            {
                action.Type = Action.ActionType.Build;
                action.Entity = Designer.Object;
                action.Position = actor.Position;
            }
            else if (Keyboard.IsKeyDown(Key.D))
            {
                
            }
            else if (Keyboard.IsKeyDown(Key.A))
            {
                
            }
            actor.PendingAction = action;
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
