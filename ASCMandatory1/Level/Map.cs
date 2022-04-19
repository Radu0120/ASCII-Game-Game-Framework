using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameFramework
{
    [Serializable]
    public class Map
    {
        //public static int currententitytodraw = 0;
        const int blinkingtime = 20;
        static long frame = 0;
        public static Dictionary<int,Map> mapIndex = new Dictionary<int,Map>();
        public int ID { get; set; }
        public int LevelID { get; set; }
        public Position LevelPosition { get; set; }
        public Tile[,] PlayableMap { get; set; }
        public Position SpawnPoint { get; set; } = new Position();
        public Position Bounds { get; set; } = new Position();
        public Map(int maxWidth, int maxHeight, int spawnX, int spawnY, Actor player, int levelid, Position levelposition)
        {
            LevelPosition = Clone<Position>.CloneObject(levelposition);
            ID = mapIndex.Count() + 1;
            LevelID = levelid;
            Bounds.X = maxHeight;
            Bounds.Y= maxWidth;
            PlayableMap = new Tile[Bounds.X, Bounds.Y];
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create(player);
        }
        public Map(int maxX, int maxY, int levelid, Position levelposition)
        {
            LevelPosition = Clone<Position>.CloneObject(levelposition);
            ID = mapIndex.Count() + 1;
            LevelID = levelid;
            Bounds.X = maxY;
            Bounds.Y = maxX;
            PlayableMap = new Tile[Bounds.X, Bounds.Y];
            this.Create();
        }
        public Map() { }
        #region Rendering
        private void Create(Actor player)
        {
            for (int i=0; i < Bounds.X; i++)
            {
                for (int j=0; j < Bounds.Y; j++)
                {
                    //adding the player
                    if (i == SpawnPoint.Y && j == SpawnPoint.X)
                    {
                        PlayableMap[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                        AddEntity(player, Position.Create(i,j));
                    }

                    //adding walls
                    else if (i == Bounds.X - 1 || i == 0 || j == Bounds.Y - 1 || j == 0)
                    {
                        PlayableMap[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                        PlayableMap[i, j].Entities.Add(Clone<WorldObject>.CloneObject(WorldObject.worldobjectIndex[0]));
                    }
                    else PlayableMap[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                }
            }
        }
        private void Create()
        {
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                   PlayableMap[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                }
            }
        }
        public List<string> DrawMap(bool designer)
        {
            List<string> map = new List<string>();
            for (int i = 0; i < Bounds.X; i++)
            {
                string line = "";
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if (PlayableMap[i, j].Entities.Count >0)
                    {
                        if(PlayableMap[i, j].Entities.Count>1) // extra entities on the tile, must show them alternatively, newest first
                        {
                            if(PlayableMap[i, j].currententitytodraw>= PlayableMap[i, j].Entities.Count)
                            {
                                PlayableMap[i, j].currententitytodraw = PlayableMap[i, j].Entities.Count - 1;
                            }
                            Entity entity = (Entity)PlayableMap[i, j].Entities[PlayableMap[i, j].currententitytodraw];
                            PlayableMap[i, j].Blink(blinkingtime, frame);
                            line += PlayableMap[i, j].Color + entity.Color + entity.Symbol + " ";
                        }
                        else // no extra entities present on the tile
                        {
                            Entity entity = (Entity)PlayableMap[i, j].Entities[0];
                            line += PlayableMap[i, j].Color + entity.Color + entity.Symbol + " ";
                        }
                    }
                    else line += PlayableMap[i, j].Color + " " + " ";
                    
                }
                map.Add(line);
            }
            return map;
        }
        #endregion


        #region UpdateMap
        public void Update()
        {
            foreach(Actor actor in GetActorsFromPlayableMap().Where(a => a.isAlive))
            {
                if (actor.PendingMovement != null)
                {
                    MoveEntity(actor, actor.PendingMovement);
                    actor.PendingMovement = null;
                }
                if (actor.PendingAction != null)
                {
                    DoAction(actor, actor.PendingAction);
                    actor.PendingAction = null;
                }
                if(actor.HP <= 0 && !(actor is Projectile))
                {
                    actor.isAlive = false;
                }
            }
            this.KillActors();
            frame++;
        }
        //tries to move the entity to the new position if the tile is empty
        public void MoveEntity(Entity entity, Position newposition)
        {
            bool changemap = false;
            int oldX;
            if (CheckCollision(newposition, entity, ref changemap))
            {
                if (entity.Attributes.Contains("Player"))
                {
                    if (changemap)
                    {
                        GoToNextMap(entity, newposition);
                        return;
                    }
                }
                UpdateEntityPosition(entity, newposition);
                return;
            }
            oldX = newposition.X;
            newposition.X = entity.Position.X;
            if(CheckCollision(newposition, entity, ref changemap))
            {
                if (entity.Attributes.Contains("Player"))
                {
                    if (changemap)
                    {
                        GoToNextMap(entity, newposition);
                        return;
                    }
                }
                UpdateEntityPosition(entity, newposition);
                return;
            }
            newposition.X = oldX;
            newposition.Y = entity.Position.Y;
            if (CheckCollision(newposition, entity, ref changemap))
            {
                if (entity.Attributes.Contains("Player"))
                {
                    if (changemap)
                    {
                        GoToNextMap(entity, newposition);
                        return;
                    }
                }
                UpdateEntityPosition(entity, newposition);
                return;
            }
            
        }
        public void DoAction(Actor actor, Action action) //actor = entity doing the action
        {
            switch (action.Type)
            {
                case Action.ActionType.Destroy:
                    Designer.RemoveEntity(this, actor.Position);
                    break;
                case Action.ActionType.Build:
                    if(Designer.CurrentState == Designer.State.Tile)
                    {
                        Designer.AddTile(this, action.Position, action.Tile);
                    }
                    else
                    {
                        Entity entity = (Entity)action.Entity;
                        Designer.AddEntity(this, action.Position, entity);
                    }
                    break;
                case Action.ActionType.Attack:
                    this.GetActorFromPosition(action.Position).TakeDamage(actor.DealDamage());
                    break;
            }
        }
        public void KillActors()
        {
            this.GetActorsFromPlayableMap().Where(a => !a.isAlive).ToList().ForEach(a => { this.RemoveEntity(a.Position, a); a = null; });
        }
        #endregion


        #region Misc Methods
        public object GetEntityFromPosition(Position position)
        {
            if (PlayableMap[position.X, position.Y].Entities.Count>0) return PlayableMap[position.X, position.Y].Entities[0];
            else return null;
        }
        public List<Entity> GetEntitiesFromPosition(Position position)
        {
            List<Entity> entities = new List<Entity>();
            PlayableMap[position.X, position.Y].Entities.Where(entity => entity is Entity).ToList().ForEach(entity => entities.Add((Entity)entity));
            return entities;
        }
        public Actor GetActorFromPosition(Position position)
        {
            if (PlayableMap[position.X, position.Y].Entities.Count > 0)
            {
                if (PlayableMap[position.X, position.Y].Entities[0] is Actor)
                    return (Actor)PlayableMap[position.X, position.Y].Entities[0];
                else return null;
            }
            else return null;
        }
        public void UpdateEntityPosition(Entity entity, Position position)
        {
            if(entity is Actor) Actor.SetDirection(position, (Actor)entity);
            Actor actor = GetActorFromPosition(position);
            if (entity.Attributes.Contains("Phase"))
            {
                PlayableMap[entity.Position.X, entity.Position.Y].Entities.Remove(entity);
                entity.Position = position;
                PlayableMap[position.X, position.Y].Entities.Add(entity);
                PlayableMap[position.X, position.Y].currententitytodraw = PlayableMap[position.X, position.Y].Entities.Count-1;
            }
            else if (actor!=null)
            {

                if (actor.Attributes.Contains("Phase"))
                {
                    PlayableMap[entity.Position.X, entity.Position.Y].Entities.Remove(entity);
                    entity.Position = position;
                    PlayableMap[position.X, position.Y].Entities.Add(entity);
                }
            }
            else
            {
                PlayableMap[entity.Position.X, entity.Position.Y].Entities.Remove(entity); //setting old tile's entity to null
                entity.Position = position;
                PlayableMap[position.X, position.Y].Entities.Add(entity);
            }
        }
        public Position GetPositionFromTile(Tile tile)
        {
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if(PlayableMap[i, j] == tile)
                    {
                        Position position = new Position() { X = i, Y = j };
                        return position;
                    }
                }
            }
            return null;
        }
        public Tile GetTileFromPosition(Position position)
        {
            return PlayableMap[position.X, position.Y];
        }
        public bool CheckCollision(Position position, Entity entity, ref bool changemap)
        {
            if(position.X > Bounds.X - 1 || position.X < 0 || position.Y > Bounds.Y - 1 || position.Y < 0) // check the map boundaries
            {
                if (CheckMapBounds(position))
                {
                    changemap = true;
                    return true;
                }
                else return false;
            }
            else if (entity.Attributes.Contains("Phase")) // if the moving entity has phase, it can move in
            {
                return true;
            }
            else if (this.GetEntitiesFromPosition(position).Where(e => e.Attributes.Contains("Solid")).Count() > 0) //check position for any solid entities
            {
                return false;
            }
            else // no entity, can move in
            {
                return true;
            }
        }
        public bool CheckMapBounds(Position position)
        {
            return Level.levelIndex[this.LevelID].CheckNextMap(position);
        }
        public void GoToNextMap(Entity entity, Position position)
        {
            Position newmapposition = new Position();
            if (position.X < 0)
            {
                newmapposition.X = Bounds.X - 1;
                newmapposition.Y = entity.Position.Y;
                Level.levelIndex[LevelID].CurrentMap.X--;
            }
            if (position.X > Level.MapBoundHeight -1)
            {
                newmapposition.X = 0;
                newmapposition.Y = entity.Position.Y;
                Level.levelIndex[LevelID].CurrentMap.X++;
            }
            if (position.Y < 0)
            {
                newmapposition.Y = Bounds.Y - 1;
                newmapposition.X = entity.Position.X;
                Level.levelIndex[LevelID].CurrentMap.Y--;
            }
            if (position.Y > Level.MapBoundWidth - 1)
            {
                newmapposition.Y = 0;
                newmapposition.X = entity.Position.X;
                Level.levelIndex[LevelID].CurrentMap.Y++;
            }
            this.RemoveEntity(entity.Position, entity);
            entity.Position = Position.Create(newmapposition.X, newmapposition.Y);
            Level.levelIndex[LevelID].GetCurrentMap().AddEntity(entity, newmapposition);
        }
        public void AddEntity(Entity entity, Position position)
        {
            Entity newentity = null;
            if (entity.Attributes.Contains("Player"))
            {
                newentity = entity;
            }
            else
            {
                switch (entity)
                {
                    case Projectile:
                        newentity = Clone<Projectile>.CloneObject((Projectile)entity);
                        break;
                    case Actor:
                        newentity = Clone<Actor>.CloneObject((Actor)entity);
                        break;
                    case Item:
                        newentity = Clone<Item>.CloneObject((Item)entity);
                        break;
                    case WorldObject:
                        newentity = Clone<WorldObject>.CloneObject((WorldObject)entity);
                        break;
                }
            }
            newentity.Position = Position.Create(position.X, position.Y);
            if (newentity.Attributes.Contains("Phase") || newentity.Attributes.Contains("Player")) //if the entity has phase, it can always move in
            {
                PlayableMap[position.X, position.Y].Entities.Add(newentity);
            }
            else if(this.GetEntitiesFromPosition(position).Count>0) //if the tile has at least 1 entity, must check if its solid
            {
                if(this.GetEntitiesFromPosition(position).Where(e => e.Attributes.Contains("Solid")).ToList().Count() > 0)
                {
                    return;
                }
                else
                {
                    PlayableMap[position.X, position.Y].Entities.Add(newentity);
                }
            }
            else // no entity, can move in
            {
                PlayableMap[position.X, position.Y].Entities.Add(newentity);
            }
        }
        public void RemoveEntity(Position position, Entity entity)
        {
            PlayableMap[position.X, position.Y].Entities.Remove(entity);
            if(PlayableMap[position.X, position.Y].currententitytodraw != 0)
            {
                PlayableMap[position.X, position.Y].currententitytodraw--;
            }
        }
        public void AddTile(Tile tile, Position position)
        {
            //foreach (Entity entity in PlayableMap[position.X, position.Y].Entities.ToList())
            //{
            //    tile.Entities.Add(entity);
            //}
            //PlayableMap[position.X, position.Y] = null;
            //PlayableMap[position.X, position.Y] = tile;
            PlayableMap[position.X, position.Y].Color = tile.Color;
            PlayableMap[position.X, position.Y].Id = tile.Id;
            PlayableMap[position.X, position.Y].Name = tile.Name;
            PlayableMap[position.X, position.Y].Attributes = PlayableMap[position.X, position.Y].Attributes;
        }
        public void RemoveTile(Position position)
        {
            PlayableMap[position.X, position.Y] = null;
            PlayableMap[position.X, position.Y] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
        }
        public List<Entity> GetEntitiesFromPlayableMap()
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    //if (PlayableMap[i, j].Entities.Count>0)
                    //{
                    //    foreach(Entity entity in PlayableMap[i, j].Entities)
                    //    {
                    //        entities.Add(entity);
                    //    }
                    //}
                    PlayableMap[i, j].Entities.ToList().ForEach(e => entities.Add((Entity)e));
                }
            }
            return entities;
        }
        public List<Actor> GetActorsFromPlayableMap()
        {
            List<Actor> actors = new List<Actor>();
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    //if (PlayableMap[i, j].Entities.Count > 0)
                    //{
                    //    PlayableMap[i, j].Entities.Where(entity => entity is Actor).ToList().ForEach(entity => actors.Add((Actor)entity));
                    //}
                    if(PlayableMap[i, j].Entities.Where(entity => entity is Actor).ToList().Count > 0)
                    PlayableMap[i, j].Entities.Where(entity => entity is Actor).ToList().ForEach(entity => actors.Add((Actor)entity));
                }
            }
            return actors;
        }
        #endregion
    }
}
