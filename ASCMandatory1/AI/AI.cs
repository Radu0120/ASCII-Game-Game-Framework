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
        public AI(int id, string name, int sightradius, bool hostile, int agression)
        {
            ID = id;
            Name = name;
            SightRadius = sightradius;
            Hostile = hostile;
            Agression = agression;
        }
        //public static void StopAIFromMap(Map map)
        //{
        //    foreach(Entity entity in map.GetEntitiesFromPlayableMap())
        //    {
        //        if (!entity.Attributes.Contains("Player"))
        //        {
        //            entity.AI.isActive = false;
        //        }
        //    }
        //}
        //public static void StartAIFromMap(Map map)
        //{
        //    foreach (Entity entity in map.GetEntitiesFromPlayableMap())
        //    {
        //        if (!entity.Attributes.Contains("Player"))
        //        {
        //            entity.AI.isActive = true;
        //        }
        //    }
        //}
        public bool CanSee(Actor target, Actor actor)
        {
            int distance = Math.Abs(target.Position.X - actor.Position.X) + Math.Abs(target.Position.Y - actor.Position.Y);
            if(distance <= SightRadius)
            {
                return true;
            }
            else return false;
        }
        public static void Move(Actor actor, Actor player)
        {
            while(actor.AI.isActive)
            {
                if(actor.AI.CanSee(player, actor))
                {
                    actor.PendingMovement = Pathfinder.AStar(player, actor);
                }
                int wait = Convert.ToInt32(1000 / actor.Speed);
                Thread.Sleep(wait);
            }
        }
        public static void Think()
        {
            while (true)
            {
                //Parallel.ForEach(Level.GetCurrentLevel().GetCurrentMap().GetActorsFromPlayableMap().Where(a => !a.Attributes.Contains("Player") && !a.AI.isActive),
                //    actor => {
                //        actor.AI.isActive = true;
                //        Thread ai = new Thread(() =>
                //            actor.AI.Move(actor, Level.Player)
                //        );
                //    });
                //foreach(Actor actor in Level.GetCurrentLevel().GetCurrentMap().GetActorsFromPlayableMap().Where(a => !a.Attributes.Contains("Player") && !a.AI.isActive))
                //{
                //    actor.AI.isActive = true;
                //    Task.Run(() =>
                //    {
                //        actor.AI.Move(actor, Level.Player);
                //    });
                //}
                List<Actor> list = Level.GetCurrentLevel().GetCurrentMap().GetActorsFromPlayableMap().Where(a => !a.Attributes.Contains("Player") && !a.AI.isActive).ToList();
                if(list.Count > 0)
                {

                }
                foreach (Actor actor in list)
                {
                    actor.AI.isActive = true;
                    //Thread ai = new Thread(() =>
                    //{
                    //    Move(actor, Level.Player);
                    //});
                    Task.Run(() =>
                    {
                        Move(actor, Level.Player);
                    });
                }
            }

        }
    }
}
