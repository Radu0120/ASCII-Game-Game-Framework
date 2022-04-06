using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCMandatory1
{
    public  class StateTable
    {
        private StateMachineEntry[,] _sm;
        // 0 = tile, 1 = worldobj, 2 = item, 3 = actor, 4 = buildmenu, 5 = mainmenu, 6 = maps
        public StateTable()
        {
            _sm = new StateMachineEntry[6, 7];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    _sm[i, j] = new StateMachineEntry() { NextState = Designer.State.MainMenu, Accepted = false };
                }
            }
            //state tile
            _sm[0, 0] = new StateMachineEntry() { NextState = Designer.State.Actor, Accepted = true }; // state tile, input z
            _sm[1, 0] = new StateMachineEntry() { NextState = Designer.State.Item, Accepted = true }; // state tile, input x
            _sm[2, 0] = new StateMachineEntry() { NextState = Designer.State.WorldOject, Accepted = true }; // state tile, input c
            _sm[3, 0] = new StateMachineEntry() { NextState = Designer.State.Tile, Accepted = true }; // state tile, input v
            _sm[4, 0] = new StateMachineEntry() { NextState = Designer.State.BuildMenu, Accepted = true }; // state tile, input esc

            //state worldobj
            _sm[0, 1] = new StateMachineEntry() { NextState = Designer.State.Actor, Accepted = true }; // state worldobj, input z
            _sm[1, 1] = new StateMachineEntry() { NextState = Designer.State.Item, Accepted = true }; // state worldobj, input x
            _sm[2, 1] = new StateMachineEntry() { NextState = Designer.State.WorldOject, Accepted = true }; // state worldobj, input c
            _sm[3, 1] = new StateMachineEntry() { NextState = Designer.State.Tile, Accepted = true }; // state worldobj, input v
            _sm[4, 1] = new StateMachineEntry() { NextState = Designer.State.BuildMenu, Accepted = true }; // state worldobj, input esc

            //state item
            _sm[0, 2] = new StateMachineEntry() { NextState = Designer.State.Actor, Accepted = true }; // state item, input z
            _sm[1, 2] = new StateMachineEntry() { NextState = Designer.State.Item, Accepted = true }; // state item, input x
            _sm[2, 2] = new StateMachineEntry() { NextState = Designer.State.WorldOject, Accepted = true }; // state item, input c
            _sm[3, 2] = new StateMachineEntry() { NextState = Designer.State.Tile, Accepted = true }; // state item, input v
            _sm[4, 2] = new StateMachineEntry() { NextState = Designer.State.BuildMenu, Accepted = true }; // state item, input esc

            //state actor
            _sm[0, 3] = new StateMachineEntry() { NextState = Designer.State.Actor, Accepted = true }; // state actor, input z
            _sm[1, 3] = new StateMachineEntry() { NextState = Designer.State.Item, Accepted = true }; // state actor, input x
            _sm[2, 3] = new StateMachineEntry() { NextState = Designer.State.WorldOject, Accepted = true }; // state actor, input c
            _sm[3, 3] = new StateMachineEntry() { NextState = Designer.State.Tile, Accepted = true }; // state actor, input v
            _sm[4, 3] = new StateMachineEntry() { NextState = Designer.State.BuildMenu, Accepted = true }; // state actor, input esc

            //state buildmenu
            _sm[0, 4] = new StateMachineEntry() { NextState = Designer.State.Actor, Accepted = true }; // state buildmenu, input z
            _sm[1, 4] = new StateMachineEntry() { NextState = Designer.State.Item, Accepted = true }; // state buildmenu, input x
            _sm[2, 4] = new StateMachineEntry() { NextState = Designer.State.WorldOject, Accepted = true }; // state buildmenu, input c
            _sm[3, 4] = new StateMachineEntry() { NextState = Designer.State.Tile, Accepted = true }; // state buildmenu, input v
            _sm[4, 4] = new StateMachineEntry() { NextState = Designer.State.MainMenu, Accepted = true }; // state buildmenu, input esc

            //state mainmenu
            _sm[0, 5] = new StateMachineEntry() { NextState = Designer.State.BuildMenu, Accepted = true }; // state mainmenu, input z
            _sm[1, 5] = new StateMachineEntry() { NextState = Designer.State.Maps, Accepted = true }; // state mainmenu, input x

            //state maps
            _sm[4, 6] = new StateMachineEntry() { NextState = Designer.State.MainMenu, Accepted = true }; // state buildmenu, input esc
        }
        public void ChangeState(Key input)
        {
            StateMachineEntry entry = _sm[ConvertInput(input), (int)Designer.CurrentState];
            if (entry.Accepted)
            {
                Designer.CurrentState = entry.NextState;
                if(input == Key.Escape) { Designer.RemoveDesignerObject(); }
            }
        }

        private int ConvertInput(Key key)
        {
            switch (key)
            {
                case Key.Z: return 0;
                case Key.X: return 1;
                case Key.C: return 2;
                case Key.V: return 3;
                case Key.Escape: return 4;
                default: return 5;
            }
        }
    }

    struct StateMachineEntry
    {
        public Designer.State NextState;
        public bool Accepted;
    }
}
