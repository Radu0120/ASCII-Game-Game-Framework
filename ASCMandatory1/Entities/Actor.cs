using System;
using System.Collections.Generic;

namespace ASCMandatory1
{
    public class Actor:Entity
    {
        public double HP { get; set; }
        public double MaxHP { get; set; }
        public int Speed { get; set; }
        public double Mana { get; set; }
        public double MaxMana { get; set; }
        public double PhysRes { get; set; }
        public double MagRes { get; set; }
        public bool isAlive { get; set; }
        public Item EquippedWeapon { get; set; }
        public List<Item> Inventory { get; set; }
        public Position PendingMovement { get; set; }
        public Action PendingAction { get; set; }
        public AI AI { get; set; }

        public static Dictionary<int, Actor> actorIndex { get; set; } = new Dictionary<int, Actor>();
        public Actor(int id, string name, char symbol, int[] color, double hp, double mana, int speed, double physres, double magres, Type type):base(id, name, symbol, color, type)
        {
            ObjectType = type;
            MaxHP = hp;
            HP = MaxHP;
            MaxMana = mana;
            Mana = MaxMana;
            Speed = speed;
            PhysRes = physres;
            MagRes = magres;
            Id = id;
            Name = name;
            Symbol = symbol;
            PendingAction = null;
            PendingMovement = null;
            Color = ASCMandatory1.Color.Foreground(color);
        }
        public Actor() { }
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
