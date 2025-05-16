using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using Lol.Content.Projectiles.WaterStream;

namespace Lol.Content.Items.Ammo.WaterBullet
{
    public class WaterBullet : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<WaterAmmo2>();
            Item.shootSpeed = 16f;
            Item.knockBack = 2f;
            Item.ammo = ModContent.ItemType<WaterBullet>();
        }
    }
}
