using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Designer
    {
        public enum State
        {
            Tile, WorldOject, Item, Actor, Menu
        }
        public static Entity Object { get; set; }
        public static Tile Tile { get; set; }
        public static State CurrentState { get; set; }
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
        public static void AddTile(Level level, Position position, Tile tile)
        {
            level.AddTile(tile, position);
        }
        public static void RemoveTile(Level level, Position position)
        {
            level.RemoveTile(position);
        }
        //designer object = equipped item to build copies of
        public static void AddDesignerObject(Entity entity)
        {
            Object = Clone<Entity>.CloneObject(entity);
        }
        public static void AddDesignerObject(Tile tile)
        {
            Tile = Clone<Tile>.CloneObject(tile);
        }
        public static void RemoveDesignerObject()
        {
            Object = null;
            Tile = null;
        }
    }
}
