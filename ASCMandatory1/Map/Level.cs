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
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        public Tile[,] Map { get; set; }
        public Position SpawnPoint { get; set; } = new Position();
        public List<Actor> Actors { get; set; }
        public Position Bounds { get; set; } = new Position();
        public Level(int maxX, int maxY, int spawnX, int spawnY, Actor player)
        {
            Map = new Tile[maxX, maxY];
            Bounds.X=maxX;
            Bounds.Y=maxY;
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create(player);
        }
        private void Create(Actor player)
        {
            for (int i=0; i < Bounds.X; i++)
            {
                for (int j=0; j < Bounds.Y; j++)
                {
                    //adding the player
                    if(i == SpawnPoint.X && j == SpawnPoint.Y)
                    {
                        Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                        AddEntity(player, Map[i, j]);
                    }
                    //adding walls
                    else if (i == Bounds.X - 1 || i == 0 ||  j == Bounds.Y - 1 || j == 0)
                    {
                        Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                        Map[i, j].Entity = Entity.Clone(Entity.entityIndex[0]);
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
        }
        public void DrawLevel()
        {
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if(Map[i, j].Entity!=null) Console.Write(Map[i, j].Color + Map[i, j].Entity.Color + Map[i,j].Entity.Symbol + " ");
                    else Console.Write(Map[i, j].Color + Map[i, j].Symbol + " ");
                    
                }
                Console.WriteLine();
            }
        }
        //tries to move the entity to the new position if the tile is empty
        public void MoveEntity(Entity entity, Position newposition)
        {
            if (this.GetEntityFromPosition(newposition) == null)
            {
                this.UpdateEntityPosition(entity, newposition);
            }
            else return;

        }
        //check if the specific position has an entity
        public Entity GetEntityFromPosition(Position position)
        {
            if (Map[position.X, position.Y].Entity != null) return Map[position.X, position.Y].Entity;
            else return null;
        }
        public void UpdateEntityPosition(Entity entity, Position position)
        {
            Map[entity.Position.X, entity.Position.Y].Entity = null; //setting old tile's entity to null
            entity.Position = position;
            Map[position.X, position.Y].Entity = entity;
            Map[position.X, position.Y].Entity.Position = position;
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
        public void AddEntity(Entity entity, Tile tile)
        {
            entity.Position = GetPositionFromTile(tile);
            tile.Entity = entity;
        }
    }
}
