﻿using System;
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
        public static int currententitytodraw = 0;
        const int blinkingtime = 20;
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
        public void Update(Actor actor, ref int count)
        {
            if (actor.PendingMovement != null)
            {
                MoveEntity(actor, actor.PendingMovement, ref count);
                actor.PendingMovement = null;
            }
            if(actor.PendingAction != null)
            {
                DoAction(actor, actor.PendingAction);
                actor.PendingAction = null;
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
                            if (count >= blinkingtime) //if count is over the blinkingtime, draw the next entity
                            {
                                if(currententitytodraw > 0)
                                {
                                    currententitytodraw--;
                                }
                                else
                                {
                                    currententitytodraw=Map[i,j].Entities.Count-1;
                                }
                                count = 0;
                            }
                            line += Map[i, j].Color + Map[i, j].Entities[currententitytodraw].Color + Map[i, j].Entities[currententitytodraw].Symbol + " ";
                        }
                        else // no extra entities present on the tile
                        {
                            line += Map[i, j].Color + Map[i, j].Entities[0].Color + Map[i, j].Entities[0].Symbol + " ";
                        }
                    }
                    else line += Map[i, j].Color + Map[i, j].Symbol + " ";
                    
                }
                level.Add(line);
                //level.Add(Color.Background(Color.Black)+"\n"+"  ");
            }
            count++;
            return level;
        }
        #endregion


        #region UpdateLevel
        //tries to move the entity to the new position if the tile is empty
        public void MoveEntity(Entity entity, Position newposition, ref int count)
        {
            int oldX;
            count = 0;
            if (CheckCollision(newposition, entity))
            {
                UpdateEntityPosition(entity, newposition, ref count);
                return;
            }
            oldX = newposition.X;
            newposition.X = entity.Position.X;
            if(CheckCollision(newposition, entity))
            {
                UpdateEntityPosition(entity, newposition, ref count);
                return;
            }
            newposition.X = oldX;
            newposition.Y = entity.Position.Y;
            if (CheckCollision(newposition, entity))
            {
                UpdateEntityPosition(entity, newposition, ref count);
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
        public void UpdateEntityPosition(Entity entity, Position position, ref int count)
        {
            if (entity.Attributes.Contains("Phase"))
            {
                Map[entity.Position.X, entity.Position.Y].Entities.Remove(entity);
                entity.Position = position;
                Map[position.X, position.Y].Entities.Add(entity);
                currententitytodraw = Map[position.X, position.Y].Entities.Count - 1; //
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
                    //this.UpdateEntityPosition(entity, position);
                    return true;
                }
                return false;
            }
            else // no entity, can move in
            {
                //this.UpdateEntityPosition(entity, position);
                return true;
            }
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
        public void AddTile(Tile tile, Position position)
        {
            foreach (Entity entity in Map[position.X, position.Y].Entities)
            {
                if(entity!=null)
                tile.Entities.Add(entity);
            }
            Map[position.X, position.Y] = null;
            Map[position.X, position.Y] = Tile.Clone(tile);
        }
        public void RemoveTile(Position position)
        {
            Map[position.X, position.Y] = null;
            Map[position.X, position.Y] = Tile.Clone(Tile.tileIndex[0]);
        }
        #endregion
    }
}
