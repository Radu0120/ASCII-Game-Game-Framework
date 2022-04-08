using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Player
    {
        public static void LaunchAttack(Actor actor, Map map)
        {
            AI projectileAI = new AI(3, "ProjAI", 0, false, 0, AI.Type.Projectile);
            Projectile projectile = new Projectile(0, $"{actor.Name}'s projectile", 'o', Color.White, 10, 10, Entity.Type.Projectile, projectileAI, actor.CurrentDirection);
            projectile.isAlive = true;
            switch (projectile.CurrentDirection)
            {
                case Actor.Direction.Up:
                    projectile.Position = Position.Create(actor.Position.X-1, actor.Position.Y);
                    break;
                case Actor.Direction.Down:
                    projectile.Position = Position.Create(actor.Position.X+1, actor.Position.Y);
                    break;
                case Actor.Direction.Left:
                    projectile.Position = Position.Create(actor.Position.X, actor.Position.Y-1);
                    break;
                case Actor.Direction.Right:
                    projectile.Position = Position.Create(actor.Position.X, actor.Position.Y+1);
                    break;
                case Actor.Direction.UpLeft:
                    projectile.Position = Position.Create(actor.Position.X-1, actor.Position.Y-1);
                    break;
                case Actor.Direction.UpRight:
                    projectile.Position = Position.Create(actor.Position.X-1, actor.Position.Y+1);
                    break;
                case Actor.Direction.DownLeft:
                    projectile.Position = Position.Create(actor.Position.X+1, actor.Position.Y-1);
                    break;
                case Actor.Direction.DownRight:
                    projectile.Position = Position.Create(actor.Position.X+1, actor.Position.Y+1);
                    break;
            }
            map.AddEntity(projectile, projectile.Position);
        }
        public static void PickUpItem(Actor player, Map map)
        {
            List<Item> items = new List<Item>();
            map.GetEntitiesFromPosition(player.Position).Where(i => i is Item).ToList().ForEach(i => items.Add(i as Item));
            if (items.Count > 0 && player.HasSpaceInInventory())
            {
                player.EquippedWeapon = items[0];
                player.AddToInventory(items[0]);
                map.RemoveEntity(player.Position, items[0]);
            }
        }
        public static void DropItem(Actor player, Map map)
        {
            if (player.EquippedWeapon != null && player.EquippedWeapon != Item.itemIndex[0])
            {
                map.AddEntity(player.EquippedWeapon, player.Position);
                player.RemoveFromInventory(player.EquippedWeapon);
            }
        }
        public static void SwapItemLeft(Actor player)
        {
            if (player.Inventory.Count > 0)
            {
                int itemcount = player.Inventory.Count;
                int itemindex = 0;
                for (int i = 0; i < itemcount; i++)
                {
                    if (player.Inventory[i] == player.EquippedWeapon)
                    {
                        itemindex = i;
                    }
                }
                itemindex--;
                if (itemindex < 0)
                {
                    itemindex = itemcount - 1;
                }
                player.EquippedWeapon = player.Inventory[itemindex];
            }
            
        }
        public static void SwapItemRight(Actor player)
        {
            if (player.Inventory.Count > 0)
            {
                int itemcount = player.Inventory.Count;
                int itemindex = 0;
                for (int i = 0; i < itemcount; i++)
                {
                    if (player.Inventory[i] == player.EquippedWeapon)
                    {
                        itemindex = i;
                    }
                }
                itemindex++;
                if (itemindex > itemcount - 1)
                {
                    itemindex = 0;
                }
                player.EquippedWeapon = player.Inventory[itemindex];
            }
        }
    }
}
