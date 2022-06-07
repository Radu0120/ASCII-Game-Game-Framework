using System;
using System.Collections.Generic;
using System.Threading;
using GameFramework;

namespace Game
{
    internal class Program
    {
        public static bool playing = true;
        public static MouseState Mouse { get; set; }
        [STAThread]
        static void Main(string[] args)
        {
            Console.Clear();
            Catalog.Populate();
            Configuration.ReadConfiguration();

            Unmanaged.SetWindowTextA(Unmanaged.ThisConsole, "Game");

            int result = Unmanaged.MessageBoxA(Unmanaged.ThisConsole, "Start the program in fullscreen?", "Fullscreen",
                            Unmanaged.MB_YESNO | Unmanaged.MB_ICONQUESTION);

            if (result == Unmanaged.IDYES)
            {

                Unmanaged.SetWindowLongPtr(Unmanaged.ThisConsole, Unmanaged.GWL_STYLE, Unmanaged.WS_POPUP);

                Unmanaged.ShowWindow(Unmanaged.ThisConsole, 3);

                Console.BufferWidth = Console.WindowWidth;
                Console.BufferHeight = Console.WindowHeight;
            }
            else
            {
                Console.WindowWidth = 208;
                Console.WindowHeight = 50;
            }

            int middleOfScreenX = Console.WindowWidth / 2;

            Button buttonGame = new Button(new Position(middleOfScreenX-15, 4), ConsoleColor.White, ConsoleColor.Black, "Start Game", 1);
            Button buttonDesigner = new Button(new Position(middleOfScreenX - 15 + buttonGame.Lenght + 10, 4), ConsoleColor.White, ConsoleColor.Black, "Start Designer", 1);

            var handle = Unmanaged.GetStdHandle(Unmanaged.STD_INPUT_HANDLE);

            int mode = 0;
            Unmanaged.GetConsoleMode(handle, ref mode);

            mode |= Unmanaged.ENABLE_MOUSE_INPUT;
            mode &= ~Unmanaged.ENABLE_QUICK_EDIT_MODE;
            mode |= Unmanaged.ENABLE_EXTENDED_FLAGS;

            Unmanaged.SetConsoleMode(handle, mode);

            var record = new INPUT_RECORD();
            uint recordLen = 0;
            Console.CursorVisible = false;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Unmanaged.ReadConsoleInput(handle, ref record, 1, ref recordLen);

                switch (record.EventType)
                {
                    case Unmanaged.MOUSE_EVENT:
                        {
                            Mouse = new MouseState(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y, record.MouseEvent.dwButtonState);
                        }
                        break;

                    case Unmanaged.KEY_EVENT:
                        {
                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
                        }
                        break;
                }
                buttonGame.DrawButton();
                buttonDesigner.DrawButton();
                if(Mouse != null)
                {
                    if (buttonGame.IsPressed(Mouse.X, Mouse.Y, (int)Mouse.PressedButton))
                    {
                        StartGame();
                    }
                    if (buttonDesigner.IsPressed(Mouse.X, Mouse.Y, (int)Mouse.PressedButton))
                    {
                        StartDesigner();
                    }
                }

                Thread.Sleep(16);
            }
            Console.Clear();
            Console.WriteLine("Thank you for playing, press any key to exit.");
            Console.ReadKey();
            Logger.ts.Close();
            Environment.Exit(0);
        }
        public static void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Actor player = new Actor(1, "player", 'P', Color.Red, 105, 100, 15, 0, 0);
            player.Attributes.Add("Player");
            player.isAlive = true;
            player.EquippedWeapon = Item.itemIndex[0];
            string input = "";
            Level chosenlevel = null;

            var handle = Unmanaged.GetStdHandle(Unmanaged.STD_INPUT_HANDLE);
            int mode = 0;
            Unmanaged.GetConsoleMode(handle, ref mode);
            mode |= Unmanaged.ENABLE_MOUSE_INPUT;
            mode &= ~Unmanaged.ENABLE_QUICK_EDIT_MODE;
            mode |= Unmanaged.ENABLE_EXTENDED_FLAGS;
            Unmanaged.SetConsoleMode(handle, mode);

            var record = new INPUT_RECORD();
            uint recordLen = 0;
            Console.CursorVisible = false;

            int buttonpositionY = 6;
            int buttonpositionX = Console.WindowWidth / 2;

            List<Button> buttons = new List<Button>();

            for (int i = 0; i < Level.levelIndex.Count; i++)
            {
                Button button = new Button(new Position(buttonpositionX, buttonpositionY), ConsoleColor.White, ConsoleColor.Black, Level.levelIndex[i].Name, 1);
                buttons.Add(button);
                buttonpositionY += 4;
            }

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Unmanaged.ReadConsoleInput(handle, ref record, 1, ref recordLen);

                switch (record.EventType)
                {
                    case Unmanaged.MOUSE_EVENT:
                        {
                            Mouse = new MouseState(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y, record.MouseEvent.dwButtonState);
                        }
                        break;

                    case Unmanaged.KEY_EVENT:
                        {
                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
                        }
                        break;
                }

                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 2);
                Console.Write("Pick one of the levels below to play");
                
                foreach(Button button in buttons)
                {
                    button.DrawButton();
                }
                
                foreach(Button button1 in buttons)
                {
                    if (button1.IsPressed(Mouse.X, Mouse.Y, (int)Mouse.PressedButton))
                    {
                        foreach (Level level in Level.levelIndex.Values)
                        {
                            if (button1.Text == level.Name)
                            {
                                chosenlevel = level;
                                player.Position = new Position(5,5);
                                level.GetCurrentMap().AddEntity(player, player.Position);
                                Level.Player = player;
                                RunGameLogic(player, false, chosenlevel);
                            }
                        }
                    }
                }
                Thread.Sleep(16);
            }

            //while (input != "close" && chosenlevel == null)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Pick one of the levels below to play, or 'close' to exit the game");
            //    for (int i = 0; i < Level.levelIndex.Count; i++)
            //    {
            //        Console.WriteLine(Level.levelIndex[i].Name);
            //    }
            //    input = Console.ReadLine();
            //    foreach (Level level in Level.levelIndex.Values)
            //    {
            //        if (input == level.Name)
            //        {
            //            chosenlevel = level;
            //            player.Position = level.GetCurrentMap().SpawnPoint;
            //            level.GetCurrentMap().AddEntity(player, player.Position);
            //            Level.Player = player;
            //            break;
            //        }
            //    }
            //}
            //if (input == "close")
            //{
            //    return;
            //}
            //RunGameLogic(player, false, chosenlevel);
        }
        public static void StartDesigner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            string input = "";
            Level chosenlevel = null;

            Actor cursor = new Actor(1, "player", 'X', Color.Red, 105, 100, 20, 0, 0);
            cursor.isAlive = true;
            cursor.Attributes.Add("Phase");
            cursor.Attributes.Add("Designer");
            cursor.Attributes.Add("Player");

            var handle = Unmanaged.GetStdHandle(Unmanaged.STD_INPUT_HANDLE);
            int mode = 0;
            Unmanaged.GetConsoleMode(handle, ref mode);
            mode |= Unmanaged.ENABLE_MOUSE_INPUT;
            mode &= ~Unmanaged.ENABLE_QUICK_EDIT_MODE;
            mode |= Unmanaged.ENABLE_EXTENDED_FLAGS;
            Unmanaged.SetConsoleMode(handle, mode);

            var record = new INPUT_RECORD();
            uint recordLen = 0;
            Console.CursorVisible = false;

            int buttonpositionY = 6;
            int buttonpositionX = Console.WindowWidth / 2;

            List<Button> buttons = new List<Button>();

            Button buttonnew = new Button(new Position(buttonpositionX, buttonpositionY), ConsoleColor.White, ConsoleColor.Black, "New", 1);
            buttonpositionY += 4;
            buttons.Add(buttonnew);

            for (int i = 0; i < Level.levelIndex.Count; i++)
            {
                Button button = new Button(new Position(buttonpositionX, buttonpositionY), ConsoleColor.White, ConsoleColor.Black, Level.levelIndex[i].Name, 1);
                buttons.Add(button);
                buttonpositionY += 4;
            }

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Unmanaged.ReadConsoleInput(handle, ref record, 1, ref recordLen);

                switch (record.EventType)
                {
                    case Unmanaged.MOUSE_EVENT:
                        {
                            Mouse = new MouseState(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y, record.MouseEvent.dwButtonState);
                        }
                        break;

                    case Unmanaged.KEY_EVENT:
                        {
                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
                        }
                        break;
                }

                Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 2);
                Console.Write("Pick one of the levels below to edit, or new to create a new one");

                foreach (Button button in buttons)
                {
                    button.DrawButton();
                }

                foreach (Button button1 in buttons)
                {
                    if (button1.IsPressed(Mouse.X, Mouse.Y, (int)Mouse.PressedButton))
                    {
                        foreach (Level level in Level.levelIndex.Values)
                        {
                            if(button1.Text == "New")
                            {
                                Console.Clear();
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 2);
                                Console.WriteLine("What should your level be named?");
                                Console.SetCursorPosition(Console.WindowWidth / 2, 4);
                                input = Console.ReadLine();
                                chosenlevel = new Level(Level.levelIndex.Count, input, cursor);
                                Level.CurrentLevel = chosenlevel.ID;
                                Level.Player = chosenlevel.GetPlayer();
                                Designer.CurrentState = Designer.State.MainMenu;
                                RunGameLogic(cursor, true, chosenlevel);
                            }
                            else if (button1.Text == level.Name)
                            {
                                chosenlevel = level;
                                cursor.Position = new Position(5, 5);
                                level.GetCurrentMap().AddEntity(cursor, cursor.Position);
                                Level.Player = cursor;
                                Designer.CurrentState = Designer.State.MainMenu;
                                RunGameLogic(cursor, true, chosenlevel);
                            }
                        }
                    }
                }
                Thread.Sleep(16);
            }

            //while (input != "close" && chosenlevel==null)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Pick one of the levels below to edit, or type 'new' to create a new level, or 'close' to exit the game");
            //    for(int i = 0; i < Level.levelIndex.Count; i++)
            //    {
            //        Console.WriteLine(Level.levelIndex[i].Name);
            //    }
            //    input = Console.ReadLine();
            //    if(input == "new")
            //    {

            //        Console.WriteLine("What should your level be named?");
            //        input = Console.ReadLine();
            //        chosenlevel = new Level(Level.levelIndex.Count, input, cursor);
            //        Level.CurrentLevel = chosenlevel.ID;
            //        Level.Player = chosenlevel.GetPlayer();
            //        break;
            //    }
            //    else
            //    {
            //        foreach (Level level in Level.levelIndex.Values)
            //        {
            //            if (input == level.Name)
            //            {
            //                chosenlevel = level;
            //                chosenlevel.AddPlayer(cursor);
            //            }
            //        }
            //    }
            //}
            //if (input == "close")
            //{
            //    return;
            //}
            //Designer.CurrentState = Designer.State.MainMenu;
            //RunGameLogic(Level.Player, true, chosenlevel);
        }
        private static void RunGameLogic(Actor player, bool designer, Level level)
        {
            Level.CurrentLevel = level.ID;
            level.StartAI();

            Thread ActionsThread = new Thread(() =>
                        Controls.CheckInput(ref player, ref playing)
                );
            ActionsThread.SetApartmentState(ApartmentState.STA);
            ActionsThread.Start();
            Thread MovementThread = new Thread(() =>
                        Controls.CheckMovement(ref player, ref playing)
                );
            MovementThread.SetApartmentState(ApartmentState.STA);
            MovementThread.Start();
            Console.Clear();

            if (!designer)
            {
                Thread AIThread = new Thread(() =>
                        AI.Think()
                );
                AIThread.Start();
            }

            //int WindowHeight = level.GetCurrentMap().Bounds.X + 2;
            //int WindowWidth = level.GetCurrentMap().Bounds.Y * 2 + 4 + 45;
            //Console.WindowHeight = WindowHeight;
            //Console.WindowWidth = WindowWidth;

            while (playing)
            {
                //if (Console.WindowHeight != WindowHeight || Console.WindowWidth != WindowWidth)
                //{
                //    WindowHeight = Console.WindowHeight;
                //    WindowWidth = Console.WindowWidth;
                //    Console.Clear();
                //}
                level.GetCurrentMap().Update();
                DrawGame(level.GetCurrentMap(), designer, player);
                Thread.Sleep(4);
            }
        }
        private static void DrawGame(Map map, bool designer, Actor player)
        {
            //Console.CursorVisible = false;
            //Console.SetCursorPosition(2, 1);
            //Dictionary<int, string> UI = GameFramework.UI.DrawUI(player, designer, map.Bounds.X);
            //int UILine = 0;
            //foreach (string line in map.DrawMap(designer))
            //{
            //    Console.Write(line);
            //    if (UILine <= UI.Count - 1)
            //    {
            //        Console.Write(UI[UILine] + "   ");
            //        UILine++;
            //    }
            //    Console.Write(Color.Background(Color.Black) + "\n" + "  ");
            //}

            int height = 48;
            int width = 80;

            Console.CursorVisible = false;
            List<List<Pixel>> frame = map.DrawMap(designer); //gets the frame
            CharInfo[] image = new CharInfo[2*height*width];
            int counter = 0;
            for(int i = 0; i < frame.Count; i++)
            {
                for(int j = 1; j<frame[i].Count; j++)
                {
                    CharInfo charInfo = new CharInfo
                    {
                        Char = frame[i][j].Char,
                        Attributes = (short)(frame[i][j].B_Color | frame[i][j].F_Color)
                    };
                    image[counter++] = charInfo;
                }
            }
            Unmanaged.RegionWrite(image, 1, 1, 2*80-1, 48);

            counter = 0;
            Console.CursorVisible = false;
            Dictionary<int, string> UI = GameFramework.UI.DrawUI(player, designer, map.Bounds.X);
            int UILine = 0;
            while (UILine < UI.Count - 1)
            {
                Console.SetCursorPosition(2 * 80 + 1, UILine);
                while (UI[UILine].Length < 70)
                {
                    UI[UILine] += "   ";
                }
                Console.Write(UI[UILine] + " ");
                UILine++;
            }
        }

    }
}
