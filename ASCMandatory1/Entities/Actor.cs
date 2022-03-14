using System;
using System.Collections.Generic;

namespace ASCMandatory1
{
    public class Actor:Entity
    {
        public double HP { get; set; }
        public int Speed { get; set; }
        public double Mana { get; set; }
        public double PhysRes { get; set; }
        public double MagRes { get; set; }
        public bool isAlive { get; set; }
        public Item EquippedWeapon { get; set; }
        public List<Item> Inventory { get; set; }
        public Position PendingMovement { get; set; }
        public Actor(int id, string name, char symbol, string color, double hp, double mana, int speed, double physres, double magres):base(id, name, symbol, color)
        {
            HP = hp;
            Mana = mana;
            Speed = speed;
            PhysRes = physres;
            MagRes = magres;
            Id = id;
            Name = name;
            Symbol = symbol;
            Color = color;
        }
        public double ComputeDamage(Damage damage)
        {
            if(damage.DamageType == Damage.Type.Physical)
            {
                if(damage.Amount > PhysRes)
                    return damage.Amount - PhysRes;
                else
                    return 0;
            }
            else if (damage.DamageType == Damage.Type.Physical)
            {
                if (damage.Amount > MagRes)
                    return damage.Amount - MagRes;
                else
                    return 0;
            }
            else return 0;
        }
        public void TakeDamage(Damage attackerdamage)
        {
            HP -= ComputeDamage(attackerdamage);
        }
        public Damage DealDamage()
        {
            return EquippedWeapon.Damage;
        }
    }
}
