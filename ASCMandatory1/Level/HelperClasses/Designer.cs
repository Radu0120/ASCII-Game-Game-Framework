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
            Tile, WorldOject, Item, Actor, BuildMenu, MainMenu, Maps
        }
        public static object Object { get; set; }
        public static Tile Tile { get; set; }
        public static State CurrentState { get; set; }
        public static StateTable StateTable = new StateTable();
        public static void AddSpawnPoint(Map map, Position position)
        {
            map.SpawnPoint = Position.Create(position.X, position.Y);
        }
        public static void AddEntity(Map map, Position position, Entity entity)
        {
            map.AddEntity(entity, position);
        }
        public static void RemoveEntity(Map map, Position position)
        {
            List<Entity> entitytoremove = map.GetEntitiesFromPosition(position).Where(e => !e.Attributes.Contains("Player")).ToList(); 
            if(entitytoremove.Count>0)
            map.RemoveEntity(position,entitytoremove[0]);
        }
        public static void AddTile(Map map, Position position, Tile tile)
        {
            map.AddTile(tile, position);
        }
        public static void RemoveTile(Map map, Position position)
        {
            map.RemoveTile(position);
        }
        public static void AddMap(Level level, Position position)
        {
            level.AddMap(position);
        }
        public static void RemoveMap(Level level, Position position)
        {
            level.RemoveMap(position);
        }
        //designer object = equipped item to build copies of
        public static void AddDesignerObject<T>(T entity) where T : class
        {
            Object = Clone<T>.CloneObject(entity);
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
