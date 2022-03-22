using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Clone<T> where T : class
    {
        public static T CloneObject(T value)
        {
            string jsonstring = JsonSerializer.Serialize(value);
            T clone = JsonSerializer.Deserialize<T>(jsonstring);
            return clone;
        }
    }
}
