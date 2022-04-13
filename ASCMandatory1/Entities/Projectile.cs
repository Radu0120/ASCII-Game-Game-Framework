using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Projectile:Actor
    {
        public int Range { get; set; }
        public Item Item { get; set; }
        public Actor Owner { get; set; }
        public Projectile(int id, string name, Item weapon, AI ai, Actor.Direction direction, Actor owner):base(id, name)
        {
            ObjectType = Type.Projectile;
            Speed = weapon.ProjectileSpeed;
            Owner = owner;
            Id = id;
            Name = name;
            Range = weapon.AttackRange;
            Symbol = weapon.ProjectileSymbol;
            Item = weapon;
            PendingMovement = null;
            CurrentDirection = direction;
            AI = ai;
            Color = weapon.ProjectileColor;
            Attributes = new List<string>();
            Attributes.Add("Phase");
        }
        public Projectile() { }
        public bool CheckCollision(Map map)
        {
            if(PendingMovement != null)
            {
                if (PendingMovement.X > map.Bounds.X - 1 || PendingMovement.X < 0 || PendingMovement.Y > map.Bounds.Y - 1 || PendingMovement.Y < 0)
                {
                    return true;
                }
            }
            List<Entity> list = map.GetEntitiesFromPosition(Position).Where(e => !(e is Projectile)).ToList();
            if (list.Count > 0)
            {
                if (list.Any(e => e.Attributes.Contains("Solid")))
                {
                    list.Where(e => e is Actor && e.Attributes.Contains("Solid")).ToList().ForEach(e => DealDamage(e as Actor));
                    return true;
                }
                else return false;
            }
            return false;
        }
        public void DealDamage(Actor target)
        {
            Logger.ts.TraceEvent(System.Diagnostics.TraceEventType.Information, 10, $"[{DateTime.Now.ToString("G")}] {Owner.Name} dealt {Item.Damage.Amount} {Item.Damage.DamageType.ToString()} damage to {target.Name}");
            Logger.ts.Flush();

            target.TakeDamage(Item.Damage);
        }
    }
}
