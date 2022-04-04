using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class StatusEffect
    {
        public static Dictionary<string, StatusEffect> statusEffectsIndex = new Dictionary<string, StatusEffect>()
        {
            { "Searching", new StatusEffect("Searching", "Searching for the enemy")},
            { "Waiting", new StatusEffect("Waiting", "This character is waiting") }
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Duration { get; set; }

        private StatusEffect(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public StatusEffect() { }
        public static StatusEffect Create(string name, int time)
        {
            if (statusEffectsIndex[name] != null)
            {
                StatusEffect statuseffect = Clone<StatusEffect>.CloneObject(statusEffectsIndex[name]);
                statuseffect.Duration = DateTime.Now.AddMilliseconds(time);
                return statuseffect;
            }
            else return null;
        }
        public bool HasExpired()
        {
            return DateTime.Now > Duration;
        }
        
    }
}
