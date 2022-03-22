using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Level
    {
        //public static int currententitytodraw = 0;
        const int blinkingtime = 20;
        static long frame = 0;
        public string Name { get; set; }
        public Tile[,] Map { get; set; }
        public Position SpawnPoint { get; set; } = new Position();
        public List<Actor> Actors { get; set; }
        public Position Bounds { get; set; } = new Position();
        public Level(string name, int maxX, int maxY, int spawnX, int spawnY, Actor player)
        {
            Name = name;
            Bounds.X = maxY;
            Bounds.Y= maxX;
            Map = new Tile[Bounds.X, Bounds.Y];
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create(player);
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
                        Map[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                        AddEntity(player, Position.Create(i,j));
                    }

                    //adding walls
                    else if (i == Bounds.X - 1 || i == 0 || j == Bounds.Y - 1 || j == 0)
                    {
                        Map[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                        Map[i, j].Entities.Add(Clone<Entity>.CloneObject(Entity.entityIndex[0]));
                    }
                    else Map[i, j] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
                    
                }
            }
        }
        public List<string> DrawLevel(bool designer, ref int count)
        {
            List<string> level = new List<string>();
            for (int i = 0; i < Bounds.X; i++)
            {
                string line = "";
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if (Map[i, j].Entities.Count >0)
                    {
                        if(Map[i, j].Entities.Count>1) // extra entities on the tile, must show them alternatively, newest first
                        {
                            Map[i, j].Blink(blinkingtime, frame);
                            line += Map[i, j].Color + Map[i, j].Entities[Map[i, j].currententitytodraw].Color + Map[i, j].Entities[Map[i, j].currententitytodraw].Symbol + " ";
                        }
                        else // no extra entities present on the tile
                        {
                            line += Map[i, j].Color + Map[i, j].Entities[0].Color + Map[i, j].Entities[0].Symbol + " ";
                        }
                    }
                    else line += Map[i, j].Color + " " + " ";
                    
                }
                level.Add(line);
            }
            //count++;
            return level;
        }
        #endregion


        #region UpdateLevel
        public void Update()
        {
            foreach(Entity entity in GetEntitiesFromMap(this))
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
            int oldX;
            if (CheckCollision(newposition, entity))
            {
                UpdateEntityPosition(entity, newposition);
                return;
            }
            oldX = newposition.X;
            newposition.X = entity.Position.X;
            if(CheckCollision(newposition, entity))
            {
                UpdateEntityPosition(entity, newposition);
                return;
            }
            newposition.X = oldX;
            newposition.Y = entity.Position.Y;
            if (CheckCollision(newposition, entity))
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
                        Designer.AddEntity(this, action.Position, action.Entity);
                    }
                    break;
            }
        }
        #endregion


        #region Misc Methods
        //check if the specific position has an entity
        public Entity GetEntityFromPosition(Position position)
        {
            if (Map[position.X, position.Y].Entities.Count>0) return Map[position.X, position.Y].Entities[0];
            else return null;
        }
        public void UpdateEntityPosition(Entity entity, Position position)
        {
            if (entity.Attributes.Contains("Phase"))
            {
                Map[entity.Position.X, entity.Position.Y].Entities.Remove(entity);
                entity.Position = position;
                Map[position.X, position.Y].Entities.Add(entity);
                Map[position.X, position.Y].currententitytodraw = Map[position.X, position.Y].Entities.Count-1;
            }
            else
            {
                Map[entity.Position.X, entity.Position.Y].Entities.Clear(); //setting old tile's entity to null
                entity.Position = position;
                Map[position.X, position.Y].Entities.Add(entity);
            }
            
        }
        public Position GetPositionFromTile(Tile tile)
        {
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if(Map[i, j] == tile)
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
            return Map[position.X, position.Y];
        }
        public bool CheckCollision(Position position, Entity entity)
        {
            if(position.X > Bounds.X - 1 || position.X < 0 || position.Y > Bounds.Y - 1 || position.Y < 0) // check the map boundaries
            {
                return false;
            }
            if (this.GetEntityFromPosition(position) != null) //if there is an entity, check for phase
            {
                if (this.GetEntityFromPosition(position).Attributes.Contains("Phase") || entity.Attributes.Contains("Phase"))
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
        public void RemoveEntity(Position position)
        {
            Map[position.X, position.Y].Entities.Remove(Map[position.X, position.Y].Entities[0]);
            Map[position.X, position.Y].currententitytodraw--;
        }
        public void AddTile(Tile tile, Position position)
        {
            foreach (Entity entity in Map[position.X, position.Y].Entities)
            {
                tile.Entities.Add(entity);
            }
            Map[position.X, position.Y] = null;
            Map[position.X, position.Y] = tile;
        }
        public void RemoveTile(Position position)
        {
            Map[position.X, position.Y] = null;
            Map[position.X, position.Y] = Clone<Tile>.CloneObject(Tile.tileIndex[0]);
        }
        public List<Entity> GetEntitiesFromMap(Level level)
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if (Map[i, j].Entities.Count>0)
                    {
                        foreach(Entity entity in Map[i, j].Entities)
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
