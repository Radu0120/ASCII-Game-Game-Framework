using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Damage
    {
        public enum Type
        {
            Physical, Magical
        }
        public Type DamageType { get; set; }
        public double Amount { get; set; }
    }
}
