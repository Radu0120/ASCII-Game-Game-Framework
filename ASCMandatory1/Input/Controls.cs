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
        public static List<Key> MenuKeys = new List<Key>() { Keybinds.MenuOption1, Keybinds.MenuOption2, Keybinds.MenuOption3, Keybinds.MenuOption4, Keybinds.Exit };
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
            if (Keyboard.IsKeyDown(Keybinds.MoveUp))
            {
                newposition.X--;
            }
            if (Keyboard.IsKeyDown(Keybinds.MoveDown))
            {
                newposition.X++;
            }
            if (Keyboard.IsKeyDown(Keybinds.MoveRight))
            {
                newposition.Y++;
            }
            if (Keyboard.IsKeyDown(Keybinds.MoveLeft))
            {
                newposition.Y--;
            }
            actor.PendingMovement = newposition;
        }
        #region PlayerMethods
        public static void DoAction(ref Actor player, ref bool playing)
        {
            Map map = Level.GetCurrentLevel().GetCurrentMap();
            if (Keyboard.IsKeyDown(Keybinds.PickUpItem))
            {
                Player.PickUpItem(player, map);
            }
            if (Keyboard.IsKeyDown(Keybinds.DropItem))
            {
                Player.DropItem(player, map);
            }
            if (Keyboard.IsKeyDown(Keybinds.UseItem))
            {
                Player.LaunchAttack(player, map);
                while (Keyboard.IsKeyDown(Keybinds.UseItem)) ;
            }
            if (Keyboard.IsKeyDown(Keybinds.SwapItemLeft))
            {
                Player.SwapItemLeft(player);
                while (Keyboard.IsKeyDown(Keybinds.SwapItemLeft)) { }
            }
            if (Keyboard.IsKeyDown(Keybinds.SwapItemRight)) 
            {
                Player.SwapItemRight(player);
                while (Keyboard.IsKeyDown(Keybinds.SwapItemRight)) { }
            }
            if (Keyboard.IsKeyDown(Keybinds.Exit))
            {
                playing = false;
                while (Keyboard.IsKeyDown(Keybinds.Exit)) { }
            }

        }
        #endregion
        #region DesignerMethods
        public static void DoDesignerAction(ref Actor actor, ref bool playing)
        {
            Action action = new Action();
            if (Keyboard.IsKeyDown(Keybinds.Delete))
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
                    if (Keyboard.IsKeyDown(Keybinds.Exit)) //exiting the game
                    {
                        playing = false;
                    }
                    else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S)) //saving
                    {
                        Save<Level>.SaveToJson(Level.GetCurrentLevel());
                        while (Keyboard.IsKeyDown(Key.S)) { }
                    }
                    break;
                default:
                    break;
            }
        }
        public static void Build(ref Actor actor, ref Action action)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Keybinds.Build))
            {
                Map map = Level.GetCurrentLevel().GetCurrentMap();

                Position startingposition = Clone<Position>.CloneObject(actor.Position);

                List<Position> alreadybuilt = new List<Position>();

                while(Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Keybinds.Build))
                {
                    foreach(Position position in Designer.DrawRectangle(startingposition, actor.Position))
                    {
                        if (!alreadybuilt.Contains(position))
                        {
                            alreadybuilt.Add(position);
                            switch (Designer.CurrentState)
                            {
                                case Designer.State.BuildMenu:
                                case Designer.State.Maps:
                                case Designer.State.MainMenu:
                                case Designer.State.Actor:
                                case Designer.State.Item:
                                    break;
                                case Designer.State.Tile:
                                    Designer.AddTile(map, position, Designer.Tile);
                                    break;
                                case Designer.State.WorldOject:
                                    Designer.AddEntity(map, position, Designer.Object as Entity);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

            }
            else if (Keyboard.IsKeyDown(Keybinds.Build))
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
                        while (Keyboard.IsKeyDown(Keybinds.Build)) { }
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
            if (Keyboard.IsKeyDown(Keybinds.BuildMapUp))
            {
                position.X--;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Keybinds.BuildMapDown))
            {
                position.X++;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Keybinds.BuildMapLeft))
            {
                position.Y--;
                Designer.AddMap(Level.levelIndex[Level.CurrentLevel], position);
            }
            else if (Keyboard.IsKeyDown(Keybinds.BuildMapRight))
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
