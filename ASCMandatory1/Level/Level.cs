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
        public static Actor Player { get; set; }

        public const int MapBoundHeight = 48;

        public const int MapBoundWidth = 80;
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
            Maps[0, MaxY/2] = new Map(0, MapBoundWidth, MapBoundHeight, 10, 10, player, this.ID, new Position(0, MaxY/2));
            StartingMap.X = 0;
            StartingMap.Y = MaxY / 2;
            CurrentMap.X = 0;
            CurrentMap.Y = MaxY / 2;
            Maps[0, (MaxY / 2) +1] = new Map(1, MapBoundWidth, MapBoundHeight, this.ID, new Position(0, (MaxY / 2) + 1));
            Maps[1, (MaxY / 2) + 1] = new Map(2, MapBoundWidth, MapBoundHeight, this.ID, new Position(1, (MaxY / 2) + 1));
            Maps[0, (MaxY / 2) - 1] = new Map(3, MapBoundWidth, MapBoundHeight, this.ID, new Position(0, (MaxY / 2) - 1));
        }
        public void AddMap(Position position)
        {
            if(position.X < 0 || position.X >= MaxX || position.Y <0 || position.Y >= MaxY)
            {
                return;
            }
            if(Maps[position.X, position.Y] == null)
            Maps[position.X, position.Y] = new Map(Maps.Length, MapBoundWidth, MapBoundHeight, this.ID, position);
        }
        public void RemoveMap(Position position)
        {
            Maps[position.X, position.Y] = null;
        }
        public bool CheckNextMap(Position position)
        {
            if(position.X < 0 && position.Y < 0)
            {
                return false;
            }
            else if(position.X > MapBoundHeight - 1 && position.Y > MapBoundWidth - 1)
            {
                return false;
            }
            else if(position.X < 0 && position.Y > MapBoundWidth - 1)
            {
                return false;
            }
            else if(position.X > MapBoundHeight-1 && position.Y < 0)
            {
                return false;
            }
            if (position.X < 0) //up
            {
                try
                {
                    if (Maps[CurrentMap.X - 1, CurrentMap.Y] != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch(IndexOutOfRangeException e) { }
            }
            if(position.X > MapBoundHeight-1) // down
            {
                try
                {
                    if (Maps[CurrentMap.X + 1, CurrentMap.Y] != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch { }
            }
            if (position.Y < 0) //left
            {
                try
                {
                    if (Maps[CurrentMap.X, CurrentMap.Y - 1] != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch { }
            }
            if (position.Y > MapBoundWidth-1) // right
            {
                try
                {
                    if (Maps[CurrentMap.X, CurrentMap.Y + 1] != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch { }
            }
            return false;
        }
        public Map GetCurrentMap()
        {
            try
            {
                return Maps[CurrentMap.X, CurrentMap.Y];
            }
            catch (IndexOutOfRangeException e) { return null; }
        }
        public static Level GetCurrentLevel()
        {
            return levelIndex[CurrentLevel];
        }
        public Actor GetPlayer()
        {
            foreach (var entity in GetCurrentMap().GetEntitiesFromPlayableMap())
            {
                if(entity.Attributes.Contains("Player"))
                {
                    return (Actor)entity;
                }
            }
            return null;
        }
    }
}
