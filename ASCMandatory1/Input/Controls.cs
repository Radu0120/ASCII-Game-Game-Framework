using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCMandatory1
{
    public class Controls
    {
        public static List<Key> MenuKeys = new List<Key>() { Key.Z, Key.X, Key.C, Key.V, Key.Escape };
        public static void CheckInput(ref Actor player, ref bool playing)
        {
            if (player.Attributes.Contains("Designer"))
            {
                while (playing)
                {
                    if (IsAnyKeyDown())
                    {
                        DoDesignerAction(ref player, ref playing);

                        if(GetPressedKeys().Where(k => MenuKeys.Contains(k)).FirstOrDefault() != Key.None)
                        {
                            NavigateMenu(GetPressedKeys().Where(k => MenuKeys.Contains(k)).FirstOrDefault());
                        }
                        Thread.Sleep(16);
                    }
                }
            }
            else
            {
                while (true)
                {
                    if (IsAnyKeyDown())
                    {
                        DoAction(ref player, ref playing);
                        Thread.Sleep(16);
                    }
                }
            }
        }
        public static void CheckMovement(ref Actor player, ref bool playing)
        {
            while (playing)
            {
                if (IsAnyKeyDown())
                {
                    Move(ref player);
                    int wait = Convert.ToInt32(1000 / player.Speed);
                    Thread.Sleep(wait);
                }
            }
        }
        public static void Move(ref Actor actor)
        {
            Position newposition = new Position() { X = actor.Position.X, Y = actor.Position.Y};
            if (Keyboard.IsKeyDown(Key.W))
            {
                newposition.X--;
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                newposition.X++;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                newposition.Y++;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                newposition.Y--;
            }
            actor.PendingMovement = newposition;
        }
        #region PlayerMethods
        public static void DoAction(ref Actor player, ref bool playing)
        {
            Map map = Level.GetCurrentLevel().GetCurrentMap();
            if (Keyboard.IsKeyDown(Key.E))
            {
                Player.PickUpItem(player, map);
            }
            if (Keyboard.IsKeyDown(Key.Q))
            {
                Player.DropItem(player, map);
            }
            if (Keyboard.IsKeyDown(Key.I))
            {
                Player.LaunchAttack(player, map);
                while (Keyboard.IsKeyDown(Key.I)) ;
            }
            if (Keyboard.IsKeyDown(Key.J))
            {
                Player.SwapItemLeft(player);
                while (Keyboard.IsKeyDown(Key.J)) { }
            }
            if (Keyboard.IsKeyDown(Key.L)) 
            {
                Player.SwapItemRight(player);
                while (Keyboard.IsKeyDown(Key.L)) { }
            }
            

        }
        #endregion
        #region DesignerMethods
        public static void DoDesignerAction(ref Actor actor, ref bool playing)
        {
            Action action = new Action();
            if (Keyboard.IsKeyDown(Key.Back))
            {
                action.Type = Action.ActionType.Destroy;
                action.Position = Clone<Position>.CloneObject(actor.Position);
                actor.PendingAction = action;
                return;
            }
            switch (Designer.CurrentState)
            {
                case Designer.State.Actor:
                case Designer.State.Item:
                case Designer.State.WorldOject:
                case Designer.State.Tile:
                    ChooseDesignerItem();
                    Build(ref actor, ref action);
                    break;
                case Designer.State.Maps:
                    BuildMap();
                    break;
                case Designer.State.MainMenu:
                    if (Keyboard.IsKeyDown(Key.Escape)) //exiting the game
                    {
                        playing = false;
                    }
                    else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S)) //saving
                    {
                        Save<Level>.SaveToJson(Level.GetCurrentLevel());
                        while (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.S)) { }
                    }
                    break;
                default:
                    break;
            }
        }
        public static void Build(ref Actor actor, ref Action action)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                action.Type = Action.ActionType.Build;
                switch (Designer.CurrentState)
                {
                    case Designer.State.BuildMenu:
                    case Designer.State.Maps:
                    case Designer.State.MainMenu:
                        return;
                    case Designer.State.Tile:
                        action.Tile = Clone<Tile>.CloneObject(Designer.Tile);
                        action.Position = actor.Position;
                        actor.PendingAction = action;
                        return;
                    case Designer.State.WorldOject:
                        action.Entity = Designer.Object;
                        action.Position = actor.Position;
                        actor.PendingAction = action;
                        return;
                    case Designer.State.Item:
                    case Designer.State.Actor:
                        action.Entity = Designer.Object;
                        action.Position = actor.Position;
                        actor.PendingAction = action;
                        while (Keyboard.IsKeyDown(Key.Enter)) { }
                        return;
                }
            }
        }
        public static void NavigateMenu(Key key)
        {
            Designer.StateTable.ChangeState(key);
            while (Keyboard.IsKeyDown(key)) { }
        }
        public static void BuildMap()
        {
            Position position = Clone<Position>.CloneObject(Level.levelIndex[Level.CurrentLevel].CurrentMap);
            if (Keyboard.IsKeyDown(Key.I))
            {
                position.X--;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Key.K))
            {
                position.X++;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Key.J))
            {
                position.Y--;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Key.L))
            {
                position.Y++;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
        }
        public static void ChooseDesignerItem()
        {
            switch (Designer.CurrentState)
            {
                case Designer.State.Actor:
                    SelectDesignerItem(Actor.actorIndex);
                    break;
                case Designer.State.Item:
                    SelectDesignerItem(Item.itemIndex);
                    break;
                case Designer.State.WorldOject:
                    SelectDesignerItem(WorldObject.worldobjectIndex);
                    break;
                case Designer.State.Tile:
                    SelectDesignerItem(Tile.tileIndex);
                    break;
                default:
                    break;
            }
        }
        public static void SelectDesignerItem<T>(Dictionary<int, T> index)
        {
            int end = index.Count;
            for (int i = 1; i <= end; i++)
            {
                Key key = (Key)(i + 34);
                if (Keyboard.IsKeyDown(key))
                {
                    switch (index[0])
                    {
                        case Actor:
                            Designer.AddDesignerObject(Actor.actorIndex[i - 1]);
                            break;
                        case Item:
                            Designer.AddDesignerObject(Item.itemIndex[i - 1]);
                            break;
                        case WorldObject:
                            Designer.AddDesignerObject(WorldObject.worldobjectIndex[i - 1]);
                            break;
                        case Tile:
                            Designer.AddDesignerObject(Tile.tileIndex[i - 1]);
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
        }
        #endregion
        public static bool IsAnyKeyDown()
        {
            var values = Enum.GetValues(typeof(Key));

            foreach (var v in values)
                if (((Key)v) != Key.None && Keyboard.IsKeyDown((Key)v))
                    return true;

            return false;
        }
        public static List<Key> GetPressedKeys()
        {
            List<Key> keys = new List<Key>();
            foreach (var key in Enum.GetValues(typeof(Key)))
            {
                if (((Key)key) != Key.None && Keyboard.IsKeyDown((Key)key))
                {
                    keys.Add((Key)key);
                }  
            }
            return keys;
        }
    }
}
