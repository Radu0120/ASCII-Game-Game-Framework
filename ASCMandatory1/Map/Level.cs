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
        public string Name { get; set; }
        public Tile[,] Map { get; set; }
        public Position SpawnPoint { get; set; } = new Position();
        public List<Actor> Actors { get; set; }
        public Position Bounds { get; set; } = new Position();
        public Level(string name, int maxX, int maxY, int spawnX, int spawnY, Actor player)
        {
            Name = name;
            Map = new Tile[maxX, maxY];
            Bounds.X=maxX;
            Bounds.Y=maxY;
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create(player);
        }
        //public Level(string name, int maxX, int maxY, Actor cursor)
        //{
        //    Name = name;
        //    Map = new Tile[maxX, maxY];
        //    Bounds.X = maxX;
        //    Bounds.Y = maxY;
        //    this.Create(cursor);
        //}
        private void Create(Actor player)
        {
            for (int i=0; i < Bounds.X; i++)
            {
                for (int j=0; j < Bounds.Y; j++)
                {
                    //adding the player
                    if (i == SpawnPoint.X && j == SpawnPoint.Y)
                    {
                        Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                        AddEntity(player, Position.Create(i,j));
                    }

                    //adding walls
                    else if (i == Bounds.X - 1 || i == 0 || j == Bounds.Y - 1 || j == 0)
                    {
                        Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                        Map[i, j].Entities.Add(Entity.Clone(Entity.entityIndex[0]));
                    }
                    else Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                    
                }
            }
        }
        public void Update(Actor actor)
        {
            if (actor.PendingMovement != null)
            {
                MoveEntity(actor, actor.PendingMovement);
                actor.PendingMovement = null;
            }
            if(actor.PendingAction != null)
            {
                DoAction(actor, actor.PendingAction);
                actor.PendingAction = null;
            }
        }
        public string DrawLevel(bool designer, ref int count)
        {
            string level = "";
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if (Map[i, j].Entities.Count >0)
                    {
                        if(Map[i, j].Entities.Count>1) // extra entities on the tile, must show them alternatively
                        {
                            if (count < 20)
                            {
                                level += Map[i, j].Color + Map[i, j].Entities[0].Color + Map[i, j].Entities[0].Symbol + " ";
                            }
                            else
                            {
                                level += Map[i, j].Color + Map[i, j].Entities[1].Color + Map[i, j].Entities[1].Symbol + " ";
                            }
                        }
                        else // no extra entities present on the tile
                        {
                            level += Map[i, j].Color + Map[i, j].Entities[0].Color + Map[i, j].Entities[0].Symbol + " ";
                        }
                    }
                    else level += Map[i, j].Color + Map[i, j].Symbol + " ";
                    
                }
                
                level += Color.Background(Color.Black)+"\n"+"  ";
            }
            count++;
            return level;
        }
        //tries to move the entity to the new position if the tile is empty
        public void MoveEntity(Entity entity, Position newposition)
        {
            bool valid = !(newposition.X > Bounds.X - 1 || newposition.X < 0 || newposition.Y > Bounds.Y - 1 || newposition.Y < 0);
            if(valid)
            {
                if(this.GetEntityFromPosition(newposition) == null || entity.Attributes.Contains("Phase"))
                {
                    this.UpdateEntityPosition(entity, newposition);
                }
            }
            else return;
        }
        public void DoAction(Entity actor, Action action) //actor = entity doing the action
        {
            switch (action.Type)
            {
                case Action.ActionType.Destroy:
                    Designer.RemoveEntity(this, actor.Position);
                    break;
                case Action.ActionType.Build:
                    Designer.AddEntity(this, action.Position, action.Entity);
                    break;
            }
        }
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
            }
            else
            {
                Map[entity.Position.X, entity.Position.Y].Entities.Clear(); //setting old tile's entity to null
                entity.Position = position;
                Map[position.X, position.Y].Entities.Add(entity);
                //Map[position.X, position.Y].Entity.Position = position;
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
        public void AddEntity(Entity entity, Position position)
        {
            bool proceed;
            entity.Position = Position.Create(position.X, position.Y);
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
        }
    }
}
