using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Lol.Content.Projectiles.Cum;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Lol.Content.Items
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class Impregnator9000 : ModItem
    {

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.Lol.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 50000;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9001;
            Item.shootSpeed = 20f;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CumProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += Vector2.Normalize(velocity) * 80f; // Adjust the position of the projectile spawn point
        }
    }
}