using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Level
    {
        public static int CurrentLevel { get; set; }
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
        public static int MaxX = 10;
        public static int MaxY = 10;
        public string Name { get; set; }
        public bool Completed { get; set; }
        public Level(int id, string name, Actor player)
        {
            ID = id;
            Name = name;
            Completed = false;
            Maps = new Map[MaxX,MaxY];
            StartingMap = new Position();
            CurrentMap = new Position();
            this.Initialize(player);
            levelIndex.Add(id, this);
        }
        public Level() { }
        public void Initialize(Actor player)
        {
            for (int i = 0; i < MaxX; i++)
            {
                for (int j = 0; j < MaxY; j++)
                {
                    Maps[i, j] = null;
                }
            }
            Maps[0, MaxY/2] = new Map(MapBoundX, MapBoundY, 10, 10, player, this.ID);
            StartingMap.X = 0;
            StartingMap.Y = MaxY / 2;
            CurrentMap.X = 0;
            CurrentMap.Y = MaxY / 2;
            Maps[0, (MaxY / 2) +1] = new Map(MapBoundX, MapBoundY, this.ID);
            Maps[1, (MaxY / 2) + 1] = new Map(MapBoundX, MapBoundY, this.ID);
            Maps[0, (MaxY / 2) - 1] = new Map(MapBoundX, MapBoundY, this.ID);
        }
        public void AddMap(Position position)
        {
            if(position.X < 0 || position.X >= MaxX || position.Y <0 || position.Y >= MaxY)
            {
                return;
            }
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
                    try{
                        if (Maps[CurrentMap.X - 1, CurrentMap.Y] != null)
                        {
                            return true;
                        }
                        throw new Exception(message:"OutofBounds");
                    }
                    catch (Exception e) { }
                    break;
                case Direction.Down:

                    try
                    {
                        if (Maps[CurrentMap.X + 1, CurrentMap.Y] != null)
                        {
                            return true;
                        }
                        throw new Exception();
                    }
                    catch (Exception e) { }
                    break;
                case Direction.Left:
                    try{
                        if (Maps[CurrentMap.X, CurrentMap.Y - 1] != null)
                        {
                            return true;
                        }
                        throw new Exception();
                    }
                    catch (Exception e) { }
                    break;
                case Direction.Right:
                    try{
                        if (Maps[CurrentMap.X, CurrentMap.Y + 1] != null)
                        {
                            return true;
                        }
                        throw new Exception();
                    }
                    catch (Exception e) { }
                    break;
                default:
                    return false;
            }
            return false;
        }
        public Map GetCurrentMap()
        {
            return Maps[CurrentMap.X, CurrentMap.Y];
        }
    }
}
