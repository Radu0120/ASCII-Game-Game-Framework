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
        public int ID { get; set; }
        public string Name { get; set; }
        public int SightRadius { get; set; }
        public bool Hostile { get; set; }
        public int Agression { get; set; }
        public bool isActive { get; set; }
        public int WarmUp { get; set; }
        public Position RandomTarget { get; set; }
        public AI(int id, string name, int sightradius, bool hostile, int agression)
        {
            ID = id;
            Name = name;
            SightRadius = sightradius;
            Hostile = hostile;
            Agression = agression;
        }
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
        public static void Move(Actor actor, Actor player)
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
        public static void Think()
        {
            while (true)
            {
                List<Actor> list = Level.GetCurrentLevel().GetCurrentMap().GetActorsFromPlayableMap().Where(a => !a.Attributes.Contains("Player") && !a.AI.isActive).ToList();
                foreach (Actor actor in list)
                {
                    actor.AI.isActive = true;
                    Task.Run(() =>
                    {
                        Move(actor, Level.Player);
                    });
                }
            }

        }
    }
}
