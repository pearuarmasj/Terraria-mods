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
using ReLogic.Content;
using Terraria.Physics;

namespace Lol.Content.Items
{
    public class MegaSponge : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mega Sponge");
            // Tooltip.SetDefault("Soaks up all the water in the world.");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.UltraAbsorbantSponge, 100)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X += (float)(player.direction * 2);
            player.itemRotation = (float)Math.PI / 2 * player.direction;
        }
        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText("Mega Sponge used!", new Color(255, 255, 255));
                return true;
            }
            return null;
        }
        public override void HoldItem(Player player)
        {
            // Only trigger once per use, matching the item's useTime
            if (player.itemAnimation == player.itemAnimationMax - 1)
            {
                Main.NewText("Soaking up water...", new Color(0, 255, 0));
                RemoveWater(player.position, 100);
            }
        }

        private void RemoveWater(Vector2 position, int radius)
        {
            int tileRadius = radius / 16; // Convert radius to tile units
            int startX = (int)(position.X / 16) - tileRadius;
            int startY = (int)(position.Y / 16) - tileRadius;
            int endX = (int)(position.X / 16) + tileRadius;
            int endY = (int)(position.Y / 16) + tileRadius;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (WorldGen.InWorld(x, y) && Main.tile[x, y].LiquidAmount > 0)
                    {
                        Main.tile[x, y].LiquidAmount = 0;
                        WorldGen.SquareTileFrame(x, y, true);
                    }
                }
            }
        }
    }
}
