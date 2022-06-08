using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Catalog
    {
        public static string FolderFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        public static void Populate()
        {
            CreateActors();
            CreateItems();
            CreateTiles();
            CreateWorldItems();
            ReadLevels();
            SetColors();

            GameFramework.Logger logger = new GameFramework.Logger();
        }
        public static void CreateActors()
        {
            Actor bandit = new Actor(0, "Bandit", 'B', Color.Yellow, 50, 10, 7, 2, 2);
            Actor goblin = new Actor(1, "Goblin", 'G', Color.LawnGreen, 30, 20, 8, 1, 1);
            Actor chimera = new Actor(2, "Chimera", 'C', Color.Cyan, 150, 100, 9, 10, 20);

            goblin.Attributes.Add("Phase");
            bandit.Attributes.Add("Solid");
            chimera.Attributes.Add("Solid");

            AI ai = new AI(0, "basic", 20, true, 5, AI.Type.Enemy);

            //ai.isActive = true;

            goblin.AI=ai;
            bandit.AI=ai;
            chimera.AI=ai;

            Actor.actorIndex.Add(bandit.Id, bandit);
            Actor.actorIndex.Add(goblin.Id, goblin);
            Actor.actorIndex.Add(chimera.Id, chimera);
        }
        public static void CreateItems()
        {
            Item unarmed = new Item(0, "Unarmed", ' ', ' ', Color.White, Color.White, 50, new Damage() { Amount = 3, DamageType = Damage.Type.Physical }, 1);
            Item sword = new Item(1, "Sword", 'S', ',', Color.White, Color.White, 50, new Damage() { Amount = 10, DamageType = Damage.Type.Physical }, 1);
            Item wand = new Item(2, "Wand", '/', 'o', Color.White, Color.Yellow, 14, new Damage() { Amount = 8, DamageType = Damage.Type.Magical }, 10);

            Item.itemIndex.Add(unarmed.Id, unarmed);
            Item.itemIndex.Add(sword.Id, sword);
            Item.itemIndex.Add(wand.Id, wand);
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
            Map.mapIndex = Save<Map>.ReadJsonMap(FolderFilePath + "/Game/Assets/Maps.json");
            Level.levelIndex = Save<Level>.ReadJsonLevel(FolderFilePath + "/Game/Assets/Levels.json");
        }

        public static void SetColors()
        {
            
            Unmanaged.SetColor(ConsoleColor.Black, (uint)GameFramework.Color.Black[0], (uint)GameFramework.Color.Black[1], (uint)GameFramework.Color.Black[2]);
            Unmanaged.SetColor(ConsoleColor.White, (uint)GameFramework.Color.White[0], (uint)GameFramework.Color.White[1], (uint)GameFramework.Color.White[2]);
            Unmanaged.SetColor(ConsoleColor.Red, (uint)GameFramework.Color.Red[0], (uint)GameFramework.Color.Red[1], (uint)GameFramework.Color.Red[2]);
            Unmanaged.SetColor(ConsoleColor.DarkBlue, (uint)GameFramework.Color.Blue[0], (uint)GameFramework.Color.Blue[1], (uint)GameFramework.Color.Blue[2]);
            Unmanaged.SetColor(ConsoleColor.Green, (uint)GameFramework.Color.LawnGreen[0], (uint)GameFramework.Color.LawnGreen[1], (uint)GameFramework.Color.LawnGreen[2]);
            Unmanaged.SetColor(ConsoleColor.DarkGreen, (uint)GameFramework.Color.Green[0], (uint)GameFramework.Color.Green[1], (uint)GameFramework.Color.Green[2]);
            Unmanaged.SetColor(ConsoleColor.Gray, (uint)GameFramework.Color.Gray[0], (uint)GameFramework.Color.Gray[1], (uint)GameFramework.Color.Gray[2]);
            Unmanaged.SetColor(ConsoleColor.Cyan, (uint)GameFramework.Color.Cyan[0], (uint)GameFramework.Color.Cyan[1], (uint)GameFramework.Color.Cyan[2]);
            Unmanaged.SetColor(ConsoleColor.Yellow, (uint)GameFramework.Color.Yellow[0], (uint)GameFramework.Color.Yellow[1], (uint)GameFramework.Color.Yellow[2]);
            Unmanaged.SetColor(ConsoleColor.Blue, (uint)GameFramework.Color.LightBlue[0], (uint)GameFramework.Color.LightBlue[1], (uint)GameFramework.Color.LightBlue[2]);
        }
    }
}
