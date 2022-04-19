using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class Actor:Entity
    {
        public enum Direction
        {
            Up, Down, Left, Right, UpLeft, DownLeft, UpRight, DownRight
        }
        public Direction CurrentDirection { get; set; }
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
        public List<StatusEffect> StatusEffects { get; set; }

        public static Dictionary<int, Actor> actorIndex { get; set; } = new Dictionary<int, Actor>();
        public Actor(int id, string name, char symbol, int[] color, double hp, double mana, int speed, double physres, double magres):base(id, name, symbol, color)
        {
            ObjectType = Type.Actor;
            isAlive = true;
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
            StatusEffects = new List<StatusEffect>();
            PendingAction = null;
            PendingMovement = null;
            Color = GameFramework.Color.Foreground(color);
            Inventory = new List<Item>();
        }
        protected Actor(int id, string name) : base(id, name)
        {
            ObjectType = Type.Actor;
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
            else if (damage.DamageType == Damage.Type.Magical)
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
        public bool HasStatusEffect(string name)
        {
            return StatusEffects.Find(s => s.Name == name) != null;
        }
        public bool HasStatusEffectExpired(string name)
        {
            if(HasStatusEffect(name)) return StatusEffects.Find(s => s.Name == name).HasExpired();
            else return true;
        }
        public void AddStatusEffect(string name, int duration)
        {
            if (!this.HasStatusEffect(name))
            {
                StatusEffects.Add(StatusEffect.Create(name, duration));
            }
            else
            {
                StatusEffects.Find(s => s.Name == name).Duration = DateTime.Now.AddMilliseconds(duration);
            }
        }
        public void AddToInventory(Item item)
        {
            if(Inventory.Count < 6)
            Inventory.Add(item);
        }
        public void RemoveFromInventory(Item item)
        {
            if (EquippedWeapon == item) EquippedWeapon = Item.itemIndex[0];
            if(Inventory.Contains(item)) Inventory.Remove(item);
        }
        public bool HasSpaceInInventory()
        {
            return Inventory.Count < 6;
        }
        public static void SetDirection(Position newposition, Actor actor)
        {
            int changeX = newposition.X - actor.Position.X;
            int changeY = newposition.Y - actor.Position.Y;

            if(changeX > 0 && changeY == 0)
            {
                actor.CurrentDirection = Direction.Down;
            }
            else if (changeX < 0 && changeY == 0)
            {
                actor.CurrentDirection = Direction.Up;
            }
            else if(changeX == 0 && changeY < 0)
            {
                actor.CurrentDirection = Direction.Left;
            }
            else if (changeX == 0 && changeY > 0)
            {
                actor.CurrentDirection = Direction.Right;
            }
            else if (changeX > 0 && changeY < 0)
            {
                actor.CurrentDirection = Direction.DownLeft;
            }
            else if (changeX > 0 && changeY > 0)
            {
                actor.CurrentDirection = Direction.DownRight;
            }
            else if (changeX < 0 && changeY > 0)
            {
                actor.CurrentDirection = Direction.UpRight;
            }
            else if (changeX < 0 && changeY < 0)
            {
                actor.CurrentDirection = Direction.UpLeft;
            }
        }
    }
}
