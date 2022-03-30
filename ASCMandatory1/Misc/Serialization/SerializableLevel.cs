using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class SerializableLevel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Position CurrentMap { get; set; }
        public Position StartingMap { get; set; }
        public List<List<int>> Maps { get; set; }
        public SerializableLevel(int id, string name, Position currentmap, Position startingmap, List<List<int>>maps)
        {
            ID = id;
            Name = name;
            CurrentMap = currentmap;
            StartingMap = startingmap;
            Maps = maps;
        }
    }
}
