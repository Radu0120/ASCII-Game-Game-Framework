using ASCMandatory1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Catalog
    {
        public static void Populate()
        {
            CreateActors();
            CreateItems();
            CreateTiles();
            CreateWorldItems();
            ReadLevels();
        }
        public static void CreateActors()
        {
            Actor bandit = new Actor(0, "Bandit", 'B', Color.Yellow, 50, 10, 7, 2, 2, Entity.Type.Actor);
            Actor goblin = new Actor(1, "Goblin", 'G', Color.LawnGreen, 30, 20, 8, 1, 1, Entity.Type.Actor);
            Actor chimera = new Actor(2, "Chimera", 'C', Color.Cyan, 150, 100, 9, 10, 20, Entity.Type.Actor);

            goblin.Attributes.Add("Phase");
            bandit.Attributes.Add("Solid");
            chimera.Attributes.Add("Solid");

            AI ai = new AI(0, "basic", 10, true, 5);

            ai.isActive = true;

            goblin.AI=ai;
            bandit.AI=ai;
            chimera.AI=ai;

            Actor.actorIndex.Add(bandit.Id, bandit);
            Actor.actorIndex.Add(goblin.Id, goblin);
            Actor.actorIndex.Add(chimera.Id, chimera);
        }
        public static void CreateItems()
        {

        }
        public static void CreateWorldItems()
        {

        }
        public static void CreateTiles()
        {
            Tile redtile = new Tile(1, "Red", Color.Red);
            Tile yellowtile = new Tile(2, "Yellow", Color.Yellow);
            Tile greentile = new Tile(3, "Green", Color.Green);

            Tile.tileIndex.Add(redtile.Id, redtile);
            Tile.tileIndex.Add(yellowtile.Id, yellowtile);
            Tile.tileIndex.Add(greentile.Id, greentile);
        }
        public static void ReadLevels()
        {
            Map.mapIndex = Save<Map>.ReadJsonMap(@"C:\Users\radue\source\repos\ASCMandatory1\Game\Assets\Maps.json");
            Level.levelIndex = Save<Level>.ReadJsonLevel(@"C:\Users\radue\source\repos\ASCMandatory1\Game\Assets\Levels.json");
        }
    }
}
