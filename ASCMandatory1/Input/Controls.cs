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
        public static void CheckInput(ref Actor player, ref bool playing)
        {
            if (player.Attributes.Contains("Designer"))
            {
                while (playing)
                {
                    if (IsAnyKeyDown())
                    {
                        if(Designer.CurrentState == Designer.State.MainMenu)
                        {
                            NavigateMainMenu(ref playing);
                        }
                        else if(Designer.CurrentState != Designer.State.Maps)
                        {
                            NavigateBuildMenu();
                            ChooseDesignerItem();
                        }
                        DoDesignerAction(ref player);
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
        public static void DoDesignerAction(ref Actor actor)
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
                    Build(ref actor, ref action);
                    break;
                case Designer.State.Maps:
                    BuildMap();
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
                if (Designer.CurrentState == Designer.State.BuildMenu)
                {
                    return;
                }
                if (Designer.Object == null)
                {
                    if (Designer.Tile == null)
                    {
                        return;
                    }
                }
                if (Designer.CurrentState == Designer.State.Tile)
                {
                    action.Tile = Clone<Tile>.CloneObject(Designer.Tile);
                    action.Position = actor.Position;
                    actor.PendingAction = action;
                    return;
                }
                else
                {
                    action.Entity = Designer.Object;
                    action.Position = actor.Position;
                    actor.PendingAction = action;
                    return;
                }
            }
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
            else if (Keyboard.IsKeyDown(Key.Escape))
            {
                while (Keyboard.IsKeyDown(Key.Escape)) { }
                Designer.CurrentState = Designer.State.MainMenu;
            }
        }
        public static void NavigateBuildMenu()
        {
            if(Designer.CurrentState != Designer.State.BuildMenu)
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    Designer.CurrentState = Designer.State.BuildMenu;
                    Designer.RemoveDesignerObject();
                    while (Keyboard.IsKeyDown(Key.Escape)) { }
                    return;
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    Designer.CurrentState = Designer.State.MainMenu;
                    Designer.RemoveDesignerObject();
                    while (Keyboard.IsKeyDown(Key.Escape)) { }
                    return;
                }
            }
            if (Keyboard.IsKeyDown(Key.Z))
            {
                Designer.CurrentState = Designer.State.Actor;
                Designer.RemoveDesignerObject();
                return;
            }
            else if (Keyboard.IsKeyDown(Key.X))
            {
                Designer.CurrentState = Designer.State.Item;
                Designer.RemoveDesignerObject();
                return;
            }
            else if (Keyboard.IsKeyDown(Key.C))
            {
                Designer.CurrentState = Designer.State.WorldOject;
                Designer.RemoveDesignerObject();
                return;
            }
            else if (Keyboard.IsKeyDown(Key.V))
            {
                Designer.CurrentState = Designer.State.Tile;
                Designer.RemoveDesignerObject();
                return;
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
        public static void NavigateMainMenu(ref bool playing)
        {
            if(Designer.CurrentState == Designer.State.MainMenu)
            {
                if (Keyboard.IsKeyDown(Key.Z))
                {
                    Designer.CurrentState = Designer.State.BuildMenu;
                    Designer.RemoveDesignerObject();
                    while (Keyboard.IsKeyDown(Key.Z)) { }
                    return;
                }
                else if (Keyboard.IsKeyDown(Key.X))
                {
                    Designer.CurrentState = Designer.State.Maps;
                    Designer.RemoveDesignerObject();
                    while (Keyboard.IsKeyDown(Key.X)) { }
                    return;
                }
                else if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Save<Level>.SaveToJson(Level.GetCurrentLevel());
                    while (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.LeftCtrl)) { }
                    return;
                }
                else if (Keyboard.IsKeyDown(Key.Escape))
                {
                    playing = false;
                    return;
                }
            }
        }
        public static bool IsAnyKeyDown()
        {
            var values = Enum.GetValues(typeof(Key));

            foreach (var v in values)
                if (((Key)v) != Key.None && Keyboard.IsKeyDown((Key)v))
                    return true;

            return false;
        }
    }
}
