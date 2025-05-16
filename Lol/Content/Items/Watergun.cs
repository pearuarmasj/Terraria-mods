using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.Modules;
using Lol.Content.Items.Ammo.WaterBullet;
using Lol.Content.Projectiles.WaterStream;

namespace Lol.Content.Items
{
    public class Watergun : ModItem
    {
        public override void SetDefaults()
        {
            // Visual properties
            Item.width = 60;
            Item.height = 20;
            Item.scale = 1.05f;
            Item.useStyle = ItemUseStyleID.Shoot; // Use style for guns
            Item.rare = ItemRarityID.Blue;

            // Combat properties
            Item.damage = 50; // Gun damage + bullet damage = final damage
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 1; // Delay between shots.
            Item.useAnimation = 10; // How long shoot animation lasts in ticks. We made it 3 times larger than useTime to make gun fire thrice
            Item.consumeAmmoOnLastShotOnly = true; // Gun will consume only one ammo per 3 shots
            Item.knockBack = 4.5f; // Gun knockback + bullet knockback = final knockback
            Item.autoReuse = true;

            // Other properties
            Item.value = 10000;
            Item.UseSound = SoundID.Item11; // Gun use sound

            // Gun properties
            Item.noMelee = true; // Item not dealing damage while held, we don’t hit mobs in the head with a gun
            Item.shootSpeed = 16f; // Speed of a projectile. Mainly measured by eye
            Item.useAmmo = ModContent.ItemType<WaterBullet>(); // Ammo type. We made a custom ammo type for this gun
            Item.shoot = ModContent.ProjectileType<WaterAmmo2>(); // Projectile type. We made a custom projectile for this gun
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(101) <= 33; // Chance in % to not consume ammo

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0f); // Offset in pixels at which the player will hold the gun. -Y is up

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += Vector2.Normalize(velocity) * 40f; // Adjust the position of the projectile spawn point
        }
    }
}