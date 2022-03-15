using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Designer
    {
        public static void AddSpawnPoint(Level level, Position position)
        {
            level.SpawnPoint = Position.Create(position.X, position.Y);
        }
        public static void AddEntity(Level level, Position position, Entity entity)
        {
            level.AddEntity(entity, position);
        }
        public static void RemoveEntity(Level level, Position position)
        {
            if(level.Map[position.X, position.Y].Entities.Count > 1)
            {
                level.RemoveEntity(position);
            }
        }
    }
}
