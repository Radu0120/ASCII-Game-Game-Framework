using ASCMandatory1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ASCMandatory1
{
    public class Save<T> where T : class
    {
        public static void SaveToJson(T value)
        {
            string JsonFileName = "";
            switch (value)
            {
                case Actor:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Actors.json";
                    break;
                case Item:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Items.json";
                    break;
                case WorldObject:
                    JsonFileName = @"ASCMandatory1\Game\Assets\WorldObjects.json";
                    break;
                case Tile:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Tiles.json";
                    break;
                case Level:
                    SaveLevel(value as Level);
                    break;
                case Map:
                    SaveMap(value as Map);
                    break;
            }
            //string output = JsonSerializer.Serialize(value);

            //File.WriteAllText(JsonFileName, output);
        }
        public static void SaveMap(Map map)
        {
            List<List<Tile>> tiles = new List<List<Tile>>();
            for (int i = 0; i < map.Bounds.X; i++)
            {
                List<Tile> tileList = new List<Tile>();
                for (int j = 0; j < map.Bounds.Y; j++)
                {
                    tileList.Add(map.PlayableMap[i, j]);
                }
                tiles.Add(tileList);
            }

            var serializablemap = new { ID = map.ID, LevelID = map.LevelID, LevelPosition = map.LevelPosition, SpawnPoint = map.SpawnPoint, Bounds = map.Bounds, Tiles = tiles };

            string output = JsonSerializer.Serialize(serializablemap);

            File.AppendAllText(@"C:\Users\radue\source\repos\ASCMandatory1\Game\Assets\Maps.json", output);
        }
        public static void SaveLevel(Level level)
        {
            List<List<int>> mapslist = new List<List<int>>();
            for (int i = 0; i < Level.MaxX; i++)
            {
                List<int> maplist = new List<int>();
                for (int j = 0; j < Level.MaxY; j++)
                {
                    if (level.Maps[i, j] != null)
                    {
                        maplist.Add(level.Maps[i, j].ID);
                    }
                    else maplist.Add(-1);
                }
                mapslist.Add(maplist);
            }

            var serializablelevel = new { ID = level.ID, Name = level.Name, CurrentMap = level.CurrentMap, StartingMap = level.StartingMap, Maps = mapslist };

            string output = JsonSerializer.Serialize(serializablelevel);

            File.AppendAllText(@"C:\Users\radue\source\repos\ASCMandatory1\Game\Assets\Levels.json", output);
        }

        public static Dictionary<int, T> ReadJson(string JsonFileName)
        {
            string jsonString = File.ReadAllText(JsonFileName);
            return JsonSerializer.Deserialize<Dictionary<int, T>>(jsonString);
        }
        public static Dictionary<int, Map> ReadJsonMap(string JsonFileName)
        {
            string jsonString = File.ReadAllText(JsonFileName);
            Dictionary<int, dynamic> intermediaryindex =  JsonSerializer.Deserialize<Dictionary<int, dynamic>>(jsonString);
            Dictionary<int, Map> mapIndex = new Dictionary<int,Map>();
            foreach(var map in intermediaryindex.Values)
            {
                Map newmap = new Map();
                newmap.Bounds = map.Bounds;
                newmap.ID = map.ID;
                newmap.LevelID = map.LevelID;
                newmap.LevelPosition = map.LevelPosition;
                newmap.SpawnPoint = map.SpawnPoint;
                newmap.PlayableMap = new Tile[newmap.Bounds.X,newmap.Bounds.Y];
                for(int i = 0; i < map.Tiles.Count; i++)
                {
                    for(int j = 0; j < map.Tiles[i].Count; j++)
                    {
                        newmap.PlayableMap[i,j] = map.Tiles[i][j];
                    }
                }
                mapIndex.Add(newmap.ID, newmap);
            }
            return mapIndex;
        }
        public static Dictionary<int, Level> ReadJsonLevel(string JsonFileName)
        {
            string jsonString = File.ReadAllText(JsonFileName);
            Dictionary<int, dynamic> intermediaryindex = JsonSerializer.Deserialize<Dictionary<int, dynamic>>(jsonString);
            Dictionary<int, Level> levelIndex = new Dictionary<int, Level>();

            foreach(var level in intermediaryindex.Values)
            {
                Level newlevel = new Level();
                newlevel.ID = level.ID;
                newlevel.Name = level.Name;
                newlevel.StartingMap = level.StartingMap;
                newlevel.CurrentMap = level.CurrentMap;
                newlevel.Maps = new Map[Level.MaxX, Level.MaxY];
                for (int i = 0; i < level.Maps.Count; i++)
                {
                    for (int j = 0; j < level.Maps[i].Count; j++)
                    {
                        if (level.Maps[i][j] != -1)
                        {
                            newlevel.Maps[i, j] = Map.mapIndex[level.Maps[i][j]];
                        }
                    }
                }
                levelIndex.Add(newlevel.ID, newlevel);
            }
            return levelIndex;
        }
    }
}
