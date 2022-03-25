using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Map
    {
        //public static int currententitytodraw = 0;
        const int blinkingtime = 20;
        static long frame = 0;
        public int LevelID { get; set; }
        public Position MapPosition { get; set; }
        public Tile[,] PlayableMap { get; set; }
        public Position SpawnPoint { get; set; } = new Position();
        public List<Actor> Actors { get; set; }
        public Position Bounds { get; set; } = new Position();
        public Map(int maxX, int maxY, int spawnX, int spawnY, Actor player, int levelid)
        {
            LevelID = levelid;
            Bounds.X = maxY;
            Bounds.Y= maxX;
            PlayableMap = new Tile[Bounds.X, Bounds.Y];
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create(player);
        }
        public Map(int maxX, int maxY, int levelid)
        {
            LevelID = levelid;
            Bounds.X = maxY;
            Bounds.Y = maxX;
            PlayableMap = new Tile[Bounds.X, Bounds.Y];
            this.Create();
        }
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
            foreach(Entity entity in GetEntitiesFromPlayableMap(this))
            {
                if(entity is Actor)
                {
                    Actor newentity = (Actor)entity;
                    if (newentity.PendingMovement != null)
                    {
                        MoveEntity(newentity, newentity.PendingMovement);
                        newentity.PendingMovement = null;
                    }
                    if (newentity.PendingAction != null)
                    {
                        DoAction(newentity, newentity.PendingAction);
                        newentity.PendingAction = null;
                    }
                }
            }
            frame++;
        }
        //tries to move the entity to the new position if the tile is empty
        public void MoveEntity(Entity entity, Position newposition)
        {
            bool changemap = false;
            int oldX;
            if (CheckCollision(newposition, entity, ref changemap))
            {
                if (changemap)
                {
                    GoToNextMap(entity);
                    return;
                }
                UpdateEntityPosition(entity, newposition);
                return;
            }
            oldX = newposition.X;
            newposition.X = entity.Position.X;
            if(CheckCollision(newposition, entity, ref changemap))
            {
                UpdateEntityPosition(entity, newposition);
                return;
            }
            newposition.X = oldX;
            newposition.Y = entity.Position.Y;
            if (CheckCollision(newposition, entity, ref changemap))
            {
                UpdateEntityPosition(entity, newposition);
                return;
            }
            
        }
        public void DoAction(Entity actor, Action action) //actor = entity doing the action
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
            }
        }
        #endregion


        #region Misc Methods
        public object GetEntityFromPosition(Position position)
        {
            if (PlayableMap[position.X, position.Y].Entities.Count>0) return PlayableMap[position.X, position.Y].Entities[0];
            else return null;
        }
        public void UpdateEntityPosition(Entity entity, Position position)
        {
            if (entity.Attributes.Contains("Phase"))
            {
                PlayableMap[entity.Position.X, entity.Position.Y].Entities.Remove(entity);
                entity.Position = position;
                PlayableMap[position.X, position.Y].Entities.Add(entity);
                PlayableMap[position.X, position.Y].currententitytodraw = PlayableMap[position.X, position.Y].Entities.Count-1;
            }
            else
            {
                PlayableMap[entity.Position.X, entity.Position.Y].Entities.Clear(); //setting old tile's entity to null
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
                if (CheckMapBounds(entity))
                {
                    changemap = true;
                    return true;
                }
                else return false;
            }
            if (this.GetEntityFromPosition(position) != null) //if there is an entity, check for phase
            {
                Entity thisentity = (Entity)this.GetEntityFromPosition(position);
                if (thisentity.Attributes.Contains("Phase") || entity.Attributes.Contains("Phase"))
                {
                    return true;
                }
                return false;
            }
            else // no entity, can move in
            {
                return true;
            }
        }
        public bool CheckMapBounds(Entity entity)
        {
            return Level.levelIndex[this.LevelID].CheckNextMap(entity.Direction);
        }
        public void GoToNextMap(Entity entity)
        {
            Position newmapposition = new Position();
            switch (entity.Direction)
            {
                case Level.Direction.Up:
                    newmapposition.X = Bounds.X-1;
                    newmapposition.Y = entity.Position.Y;
                    Level.levelIndex[LevelID].CurrentMap.X--;
                    break;
                case Level.Direction.Down:
                    newmapposition.X = 0;
                    newmapposition.Y = entity.Position.Y;
                    Level.levelIndex[LevelID].CurrentMap.X++;
                    break;
                case Level.Direction.Left:
                    newmapposition.X = entity.Position.X;
                    newmapposition.Y = Bounds.Y-1;
                    Level.levelIndex[LevelID].CurrentMap.Y--;
                    break;
                case Level.Direction.Right:
                    newmapposition.X = entity.Position.X;
                    newmapposition.Y = 0;
                    Level.levelIndex[LevelID].CurrentMap.Y++;
                    break;
                default:
                    break;
            }
            this.RemoveEntity(entity.Position, entity);
            entity.Position = Position.Create(newmapposition.X, newmapposition.Y);
            Level.levelIndex[LevelID].GetCurrentMap().AddEntity(entity, newmapposition);
        }
        public void AddEntity(Entity entity, Position position)
        {
            bool proceed;
            entity.Position = Position.Create(position.X, position.Y);
            if (entity.Attributes.Contains("Phase"))
            {
                GetTileFromPosition(position).Entities.Add(entity);
                return;
            }
            if (GetTileFromPosition(position).Entities.Count > 0)
            {
                proceed = true;
                foreach(Entity entityfromtile in GetTileFromPosition(position).Entities)
                {
                    if (entityfromtile.Attributes.Contains("Solid"))
                    {
                        proceed = false;
                        break;
                    }
                }
                if(proceed) GetTileFromPosition(position).Entities.Add(entity);
            }
            else
            {
                GetTileFromPosition(position).Entities.Add(entity);
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
            foreach (Entity entity in PlayableMap[position.X, position.Y].Entities)
            {
                tile.Entities.Add(entity);
            }
            PlayableMap[position.X, position.Y] = null;
            PlayableMap[position.X, position.Y] = tile;
        }
        public void RemoveTile(Position position)
        {
            PlayableMap[position.X, position.Y] = null;
            PlayableMap[position.X, position.Y] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
        }
        public List<Entity> GetEntitiesFromPlayableMap(Map level)
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if (PlayableMap[i, j].Entities.Count>0)
                    {
                        foreach(Entity entity in PlayableMap[i, j].Entities)
                        {
                            entities.Add(entity);
                        }
                    }
                }
            }
            return entities;
        } 
        #endregion
    }
}
