using ASCMandatory1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Save<T> where T : class
    {
        public static void WriteToJsonSingle(T value)
        {
            string JsonFileName = "";
            switch (value)
            {
                case Actor:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Actors.json";
                    break;
                case Item:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Items.json";
                    break;
                case WorldObject:
                    JsonFileName = @"ASCMandatory1\Game\Assets\WorldObjects.json";
                    break;
                case Tile:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Tiles.json";
                    break;
                case Level:
                    JsonFileName = @"ASCMandatory1\Game\Assets\Levels.json";
                    break;
            }
            string output = JsonSerializer.Serialize(value);

            File.WriteAllText(JsonFileName, output);
        }
    }
}
