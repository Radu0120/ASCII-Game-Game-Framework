using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Level
    {
        public static Dictionary<int, Level> levelIndex = new Dictionary<int, Level>();
        public const int MapBoundX = 80;
        public const int MapBoundY = 48;
        public enum Direction
        {
            Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight
        }
        public int ID { get; set; }
        public Map[,] Maps { get; set; }
        public Position CurrentMap { get; set; }
        public Position StartingMap { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public Level(int id, string name, Actor player)
        {
            ID = id;
            Name = name;
            Completed = false;
            this.Initialize(player);
            levelIndex.Add(id, this);
        }
        public void Initialize(Actor player)
        {
            for (int i = 0; i <= MaxX; i++)
            {
                for (int j = 0; j <= MaxY; j++)
                {
                    Maps[i, j] = null;
                }
            }
            Maps[0, MaxY/2] = new Map(MapBoundX, MapBoundY, 10, 10, player, this.ID);
            StartingMap.X = 0;
            StartingMap.Y = MaxY / 2;
            CurrentMap.X = 0;
            CurrentMap.Y = MaxY / 2;
        }
        public void AddMap(Position position)
        {
            if(Maps[position.X, position.Y] == null)
            Maps[position.X, position.Y] = new Map(MapBoundX, MapBoundY, this.ID);
        }
        public void RemoveMap(Position position)
        {
            Maps[position.X, position.Y] = null;
        }
        public bool CheckNextMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (Maps[CurrentMap.X - 1, CurrentMap.Y] != null)
                    {
                        CurrentMap.X--;
                        return true;
                    }
                    break;
                case Direction.Down:
                    if (Maps[CurrentMap.X + 1, CurrentMap.Y] != null)
                    {
                        CurrentMap.X++;
                        return true;
                    }
                    break;
                case Direction.Left:
                    if (Maps[CurrentMap.X, CurrentMap.Y - 1] != null)
                    {
                        CurrentMap.Y--;
                        return true;
                    }
                    break;
                case Direction.Right:
                    if (Maps[CurrentMap.X, CurrentMap.Y + 1] != null)
                    {
                        CurrentMap.Y++;
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }
    }
}
