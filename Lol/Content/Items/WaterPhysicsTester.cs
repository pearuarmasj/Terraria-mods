using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lol.Content.Items
{
    public class WaterPhysicsTester : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }

        public override bool? UseItem(Player player)
        {
            Vector2 mousePosition = Main.MouseWorld;
            int tileX = (int)(mousePosition.X / 16f);
            int tileY = (int)(mousePosition.Y / 16f);

            // Create a floating water ball
            if (WorldGen.InWorld(tileX, tileY))
            {
                PlaceWaterBall(tileX, tileY);
            }

            return true;
        }

        private void PlaceWaterBall(int centerX, int centerY)
        {
            // Place a 3x1 row of water with nothing below it
            for (int x = centerX - 1; x <= centerX + 1; x++)
            {
                Tile tile = Main.tile[x, centerY];
                if (!tile.HasTile)
                {
                    tile.LiquidType = LiquidID.Water;
                    tile.LiquidAmount = 255;
                    WorldGen.SquareTileFrame(x, centerY);
                }

                // Clear any tiles below to ensure water is "in the air"
                for (int y = centerY + 1; y < centerY + 3; y++)
                {
                    if (WorldGen.InWorld(x, y) && Main.tile[x, y].HasTile)
                    {
                        WorldGen.KillTile(x, y);
                    }
                }
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, centerX, centerY, 5);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.Register();
        }
    }
}