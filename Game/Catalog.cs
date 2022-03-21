﻿using ASCMandatory1;
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
        }
        public static void CreateActors()
        {
            Actor bandit = new Actor(0, "Bandit", 'B', Color.Yellow, 50, 10, 7, 2, 2);
            Actor goblin = new Actor(1, "Goblin", 'G', Color.LawnGreen, 30, 20, 8, 1, 1);
            Actor chimera = new Actor(2, "Chimera", 'C', Color.Cyan, 150, 100, 9, 10, 20);

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

        }
    }
}
