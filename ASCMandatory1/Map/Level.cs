using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        public Level(int maxX, int maxY, int spawnX, int spawnY)
        {
            Map = new Tile[maxX, maxY];
            Bounds.X=maxX;
            Bounds.Y=maxY;
            SpawnPoint.X=spawnX;
            SpawnPoint.Y=spawnY;
            this.Create();
        }
        private void Create()
        {
            for (int i=0; i < Bounds.X; i++)
            {
                for (int j=0; j < Bounds.Y; j++)
                {
                    if(i == SpawnPoint.X && j == SpawnPoint.Y)
                    {
                        Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                        Map[i, j].Symbol = 'P';
                    }
                    else Map[i, j] = Tile.Clone(Tile.tileIndex[0]);
                }
            }
        }
        public void DrawLevel()
        {
            for (int i = 0; i < Bounds.X; i++)
            {
                for (int j = 0; j < Bounds.Y; j++)
                {
                    if(Map[i, j].Entity!=null) Console.Write(Map[i, j].Color + Map[i, j].Entity.Color + Map[i,j].Entity.Color + " ");
                    else Console.Write(Map[i, j].Color + Map[i, j].Symbol + " ");
                }
                Console.WriteLine();
            }
        }
        public void MoveEntity(Entity entity, Position newposition)
        {
            if (this.GetEntityFromPosition(newposition) == null)
            {
                this.UpdateEntityPosition(entity, newposition);
            }
            else return;

        }
        public Entity GetEntityFromPosition(Position position)
        {
            if (Map[position.X, position.Y].Entity != null) return Map[position.X, position.Y].Entity;
            else return null;
        }
        public void UpdateEntityPosition(Entity entity, Position position)
        {
            Map[entity.Position.X, entity.Position.Y].Entity = null; //setting old tile's entity to null
            Map[position.X, position.Y].Entity = entity;
            Map[position.X, position.Y].Entity.Position = position;
        }
    }
}
