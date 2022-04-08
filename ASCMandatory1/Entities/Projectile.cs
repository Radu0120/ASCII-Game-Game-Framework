using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Projectile:Actor
    {
        //public int Speed { get; set; }
        //public AI AI { get; set; }
        //public Position PendingMovement { get; set; }
        //public Actor.Direction Direction { get; set; }
        public int Range { get; set; }
        public Projectile(int id, string name, char symbol, int[] color, int speed, int range,Type type, AI ai, Actor.Direction direction) : base(id, name, symbol, color, type)
        {
            ObjectType = type;
            Speed = speed;
            Id = id;
            Name = name;
            Range = range;
            Symbol = symbol;
            PendingMovement = null;
            CurrentDirection = direction;
            AI = ai;
            Color = ASCMandatory1.Color.Foreground(color);
            Attributes.Add("Phase");
        }
        public Projectile() { }
        public bool CheckCollision(Map map)
        {
            if(PendingMovement != null)
            {
                if (PendingMovement.X > map.Bounds.X - 1 || PendingMovement.X < 0 || PendingMovement.Y > map.Bounds.Y - 1 || PendingMovement.Y < 0)
                {
                    return true;
                }
            }
            List<Entity> list = map.GetEntitiesFromPosition(Position).Where(e => !(e is Projectile)).ToList();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
