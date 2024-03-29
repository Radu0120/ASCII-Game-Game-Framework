﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Action
    {
        public enum ActionType
        {
            Attack, Block, Build, Use, Destroy, Move
        }
        public ActionType Type { get; set; }
        public Item Item { get; set; }
        public Damage Damage { get; set; }
        public Position Position { get; set; }
        public object Entity { get; set; }
        public Tile Tile { get; set; }
    }
}
