using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Physics;

namespace Lol.Content.FluidPhysics
{
    public class WaterPhysics : ModSystem
    {
        // Toggle for enabling/disabling the horizontal water physics
        public static bool EnableHorizontalWaterPhysics = true;

        // Previous state of the toggle key to detect when it's first pressed
        private bool previousKeyState = false;

        public override void PostUpdateWorld()
        {
            // Check for toggle key press (F7 key)
            if (Main.keyState.IsKeyDown(Keys.F7) && !previousKeyState)
            {
                EnableHorizontalWaterPhysics = !EnableHorizontalWaterPhysics;
                string status = EnableHorizontalWaterPhysics ? "ON" : "OFF";
                Main.NewText($"Horizontal Water Physics: {status}", new Color(0, 150, 255));

                // Play a sound effect for feedback
                SoundEngine.PlaySound(status == "ON" ? SoundID.Splash : SoundID.MenuClose);
            }

            // Update previous key state
            previousKeyState = Main.keyState.IsKeyDown(Keys.F7);

            // Only run the physics if enabled
            if (EnableHorizontalWaterPhysics)
            {
                ProcessHorizontalWaterMovement();
            }
        }

        // Rest of your code remains the same...
        private void ProcessHorizontalWaterMovement()
        {
            // Only process water near the player to keep things manageable
            Player player = Main.LocalPlayer;
            if (!player.active)
                return;

            // Get tile coordinates around the player
            int playerTileX = (int)(player.Center.X / 16f);
            int playerTileY = (int)(player.Center.Y / 16f);
            int scanRadius = 1200; // Small radius to prevent overwhelming the engine, up from 15
            int maxTilesToProcess = 30000; // Very small number of tiles per update, up from 3
            int tilesProcessed = 0;

            for (int i = -scanRadius; i <= scanRadius && tilesProcessed < maxTilesToProcess; i++)
            {
                for (int j = -scanRadius; j <= scanRadius && tilesProcessed < maxTilesToProcess; j++)
                {
                    int x = playerTileX + i;
                    int y = playerTileY + j;

                    if (!WorldGen.InWorld(x, y))
                        continue;

                    Tile tile = Main.tile[x, y];

                    // Check if this tile contains water
                    if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0)
                    {
                        // Check if the tile is "in the air"
                        if (IsWaterTileInAir(x, y))
                        {
                            // Try to move water horizontally
                            MoveWaterHorizontally(x, y);
                            tilesProcessed++;
                        }
                    }
                }
            }
        }

        private bool IsWaterTileInAir(int x, int y)
        {
            // Check below for solid tile or water-filled tile
            if (y < Main.maxTilesY - 1)
            {
                Tile tileBelow = Main.tile[x, y + 1];
                if (tileBelow.HasTile && Main.tileSolid[tileBelow.TileType])
                    return false;
                if (tileBelow.LiquidAmount == 255) // Full water below
                    return false;
            }

            // Check sides and above for solid tiles
            if ((x > 0 && Main.tile[x - 1, y].HasTile && Main.tileSolid[Main.tile[x - 1, y].TileType]) ||
                (x < Main.maxTilesX - 1 && Main.tile[x + 1, y].HasTile && Main.tileSolid[Main.tile[x + 1, y].TileType]) ||
                (y > 0 && Main.tile[x, y - 1].HasTile && Main.tileSolid[Main.tile[x, y - 1].TileType]))
                return false;

            return true; // No adjacent solid tiles
        }

        private void MoveWaterHorizontally(int x, int y)
        {
            Tile sourceTile = Main.tile[x, y];

            // Store flow direction in AI to keep it consistent
            // This creates a more natural flowing effect instead of random jittering
            int flowDirection = 0;

            // Calculate which direction makes more sense for water to flow
            // Check water levels on both sides
            byte leftWaterAmount = 0;
            byte rightWaterAmount = 0;

            if (x > 0 && !Main.tile[x - 1, y].HasTile)
                leftWaterAmount = Main.tile[x - 1, y].LiquidAmount;

            if (x < Main.maxTilesX - 1 && !Main.tile[x + 1, y].HasTile)
                rightWaterAmount = Main.tile[x + 1, y].LiquidAmount;

            // Natural water flow preference - go where there's less water
            if (leftWaterAmount < rightWaterAmount)
                flowDirection = -1; // Flow left
            else if (rightWaterAmount < leftWaterAmount)
                flowDirection = 1;  // Flow right
            else
            {
                // If water levels are equal, look for a "slope" - check if one side drops lower
                int leftDepth = 0;
                int rightDepth = 0;

                // Check how far down we need to go to find solid ground on left
                if (x > 0)
                {
                    for (int checkY = y + 1; checkY < y + 6 && checkY < Main.maxTilesY; checkY++)
                    {
                        if (!WorldGen.InWorld(x - 1, checkY) ||
                            (Main.tile[x - 1, checkY].HasTile && Main.tileSolid[Main.tile[x - 1, checkY].TileType]))
                        {
                            leftDepth = checkY - y;
                            break;
                        }
                    }
                }

                // Check how far down we need to go to find solid ground on right
                if (x < Main.maxTilesX - 1)
                {
                    for (int checkY = y + 1; checkY < y + 6 && checkY < Main.maxTilesY; checkY++)
                    {
                        if (!WorldGen.InWorld(x + 1, checkY) ||
                            (Main.tile[x + 1, checkY].HasTile && Main.tileSolid[Main.tile[x + 1, checkY].TileType]))
                        {
                            rightDepth = checkY - y;
                            break;
                        }
                    }
                }

                // Prefer flowing toward deeper side (downhill)
                if (leftDepth > rightDepth)
                    flowDirection = -1;
                else if (rightDepth > leftDepth)
                    flowDirection = 1;
                else
                    // If completely equal, use a weighted random based on previous flow in area
                    flowDirection = Main.rand.NextBool(3) ? -1 : 1;
            }

            int targetX = x + flowDirection;

            if (targetX < 0 || targetX >= Main.maxTilesX)
                return;

            // Rest of the method stays the same
            Tile targetTile = Main.tile[targetX, y];

            // Ensure target can accept water
            if (targetTile.HasTile && Main.tileSolid[targetTile.TileType])
                return;

            // Calculate water amount to move
            byte amountToMove = (byte)Math.Min(20, (int)sourceTile.LiquidAmount / 2);

            // Don't mix different liquids
            if (targetTile.LiquidAmount > 0 && targetTile.LiquidType != LiquidID.Water)
                return;

            // Move the water
            if (amountToMove > 0)
            {
                sourceTile.LiquidAmount -= amountToMove;

                if (targetTile.LiquidAmount == 0)
                    targetTile.LiquidType = LiquidID.Water;

                targetTile.LiquidAmount = (byte)Math.Min(255, targetTile.LiquidAmount + amountToMove);

                // Update frames and sync
                WorldGen.SquareTileFrame(x, y);
                WorldGen.SquareTileFrame(targetX, y);

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, x, y, 1);
                    NetMessage.SendTileSquare(-1, targetX, y, 1);
                }
            }
        }
    }
}