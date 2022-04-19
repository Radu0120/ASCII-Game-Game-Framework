using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class SerializableMap
    {
        public int ID { get; set; }
        public int LevelID { get; set; }
        public Position LevelPosition { get; set; }
        public Position SpawnPoint { get; set; }
        public Position Bounds { get; set; }
        public List<List<Tile>> Tiles { get; set; }
        public SerializableMap(int id, int levelid, Position levelposition, Position spawnpoint, Position bounds, List<List<Tile>> tiles)
        {
            ID = id;
            LevelID = levelid;
            LevelPosition = levelposition;
            SpawnPoint = spawnpoint;
            Bounds = bounds;
            Tiles = tiles;
        }
    }
}
