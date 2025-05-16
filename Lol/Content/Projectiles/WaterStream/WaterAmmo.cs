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

namespace Lol.Content.Projectiles.WaterStream
{
    public class WaterAmmo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Water Stream");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged; // Damage class projectile uses
            Projectile.scale = 1f; // Projectile scale multiplier
            Projectile.penetrate = 3; // How many hits projectile have to make before it dies. 3 means projectile will die on 3rd enemy. Setting this to 0 will make projectile die instantly
            Projectile.aiStyle = 1; // AI style of a projectile. 0 is default bullet AI
            Projectile.width = Projectile.height = 10; // Hitbox of projectile in pixels
            Projectile.friendly = true; // Can hit enemies?
            Projectile.hostile = false; // Can hit player?
            Projectile.timeLeft = 600; // Time in ticks before projectile dies
            Projectile.light = 0f; // How much light projectile provides
            Projectile.ignoreWater = true; // Does the projectile ignore water (doesn't slow down in it)
            Projectile.tileCollide = true; // Does the projectile collide with tiles, like blocks?
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Convert the projectile's center to tile coordinates
            int x = (int)(Projectile.Center.X / 16f);
            int y = (int)(Projectile.Center.Y / 16f);

            // Check that the tile exists and isn't some forbidden zone
            if (WorldGen.InWorld(x, y))
            {
                Tile tile = Main.tile[x, y]; // Get a reference to the tile
                if (tile != null)
                {
                    tile.LiquidType = LiquidID.Water;
                    tile.LiquidAmount = 255; // Full
                    WorldGen.SquareTileFrame(x, y);

                    // Sync with clients in multiplayer
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, x, y, 1);
                }
            }

            // Kill the projectile so you don't get infinite water
            Projectile.Kill();

            // Return false to prevent vanilla collision logic (like bouncing)
            return false;
        }
    }
}