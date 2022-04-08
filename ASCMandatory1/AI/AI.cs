using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class AI
    {
        public enum Type
        {
            Enemy, Projectile, Friendly
        }
        public Type AIType { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int SightRadius { get; set; }
        public bool Hostile { get; set; }
        public int Agression { get; set; }
        public bool isActive { get; set; }
        public int WarmUp { get; set; }
        public Position RandomTarget { get; set; }
        public AI(int id, string name, int sightradius, bool hostile, int agression, Type type)
        {
            ID = id;
            Name = name;
            SightRadius = sightradius;
            Hostile = hostile;
            Agression = agression;
            AIType = type;
        }
        public AI() { }
        public bool CanSee(Actor target, Actor actor)
        {
            int distance = Math.Abs(target.Position.X - actor.Position.X) + Math.Abs(target.Position.Y - actor.Position.Y);
            if(distance <= SightRadius)
            {
                actor.AddStatusEffect("Searching", 2000);
                return true;
            }
            else if (actor.HasStatusEffect("Searching"))
            {
                return !actor.HasStatusEffectExpired("Searching");
            }
            else return false;
        }
        public static void Target(Actor actor, Actor player) //used for enemies
        {
            while(actor.AI.isActive)
            {
                if(actor.AI.CanSee(player, actor))
                {
                    actor.PendingMovement = Pathfinder.AStar(player.Position, actor);
                }
                else if(actor.AI.RandomTarget == null || actor.HasStatusEffectExpired("Waiting")) 
                {
                    actor.AI.RandomTarget = Pathfinder.Wander(actor);
                }
                else
                {
                    actor.PendingMovement = Pathfinder.AStar(actor.AI.RandomTarget, actor);
                    if (Position.AreEqual(actor.AI.RandomTarget, actor.Position)) actor.AI.RandomTarget = null;
                }
                int wait = Convert.ToInt32(1000 / actor.Speed); //determines ai walking speed
                Thread.Sleep(wait);
            }
        }
        public static void Propagate(Projectile projectile) // used for projectiles
        {
            Thread.Sleep(36);
            Map map = Level.GetCurrentLevel().GetCurrentMap();
            while (projectile.Range > 0)
            {
                if (projectile.CheckCollision(map))
                {
                    projectile.Range = 0;
                    break;
                }
                Position pendingpos = new Position();
                switch (projectile.CurrentDirection)
                { 
                    case Actor.Direction.Up:
                        pendingpos = Position.Create(projectile.Position.X - 1, projectile.Position.Y);
                        break;
                    case Actor.Direction.Down:
                        pendingpos = Position.Create(projectile.Position.X + 1, projectile.Position.Y);
                        break;
                    case Actor.Direction.Left:
                        pendingpos = Position.Create(projectile.Position.X, projectile.Position.Y - 1);
                        break;
                    case Actor.Direction.Right:
                        pendingpos = Position.Create(projectile.Position.X, projectile.Position.Y + 1);
                        break;
                    case Actor.Direction.UpLeft:
                        pendingpos = Position.Create(projectile.Position.X - 1, projectile.Position.Y - 1);
                        break;
                    case Actor.Direction.UpRight:
                        pendingpos = Position.Create(projectile.Position.X - 1, projectile.Position.Y + 1);
                        break;
                    case Actor.Direction.DownLeft:
                        pendingpos = Position.Create(projectile.Position.X + 1, projectile.Position.Y - 1);
                        break;
                    case Actor.Direction.DownRight:
                        pendingpos = Position.Create(projectile.Position.X + 1, projectile.Position.Y + 1);
                        break;
                }
                projectile.PendingMovement = pendingpos;
                if (projectile.CheckCollision(map))
                {
                    projectile.Range = 0;
                    break;
                }
                projectile.Range--;
                int wait = Convert.ToInt32(1000 / projectile.Speed); //determines ai walking speed
                Thread.Sleep(wait);
            }
            if(projectile.Range <= 0)
            {
                map.RemoveEntity(projectile.Position, projectile);
                projectile = null;
            }
        }
        public static void Think()
        {
            while (true)
            {
                List<Actor> list = Level.GetCurrentLevel().GetCurrentMap().GetActorsFromPlayableMap().Where(a => !a.Attributes.Contains("Player") && !a.AI.isActive).ToList();
                foreach (Actor actor in list)
                {
                    actor.AI.isActive = true;
                    switch (actor.AI.AIType)
                    {
                        case Type.Enemy:
                            Task.Run(() =>
                            {
                                Target(actor, Level.Player);
                            });
                            break;
                        case Type.Projectile:
                            Task.Run(() =>
                            {
                                Propagate(actor as Projectile);
                            });
                            break;
                    }
                }
            }

        }
    }
}
