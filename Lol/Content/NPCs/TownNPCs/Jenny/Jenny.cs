using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using System.Security.Cryptography.X509Certificates;

namespace Lol.Content.NPCs.TownNPCs.Jenny
{
    [AutoloadHead]
    public class Jenny : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.townNPCCanSpawn[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 21;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 7;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 1;
            NPCID.Sets.AttackTime[NPC.type] = 15;
            NPCID.Sets.CannotSitOnFurniture[NPC.type] = false;
            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, ProjectileID.Bullet, 0, 0f, Main.myPlayer);
            NPCID.Sets.AttackAverageChance[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.AllowDoorInteraction[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 50;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.PlayerHit;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            // Don't use AnimationType, we'll control frames manually
        }

        // Custom frame logic to restrict walking frames
        public override void FindFrame(int frameHeight)
        {
            // Frame constants - adjust these based on your sprite sheet
            const int StandingFrame = 0;
            const int WalkingFrameStart = 1;
            const int WalkingFrameEnd = 13;
            const int TalkingFrameStart = 14;
            const int TalkingFrameEnd = 15;
            const int SittingFrame = 16; // Assuming frame 16 is the single sitting frame

            NPC.spriteDirection = NPC.direction; // Make sure NPC faces the direction it's moving or interacting

            // Determine NPC's current state
            bool isSitting = NPC.ai[1] == 1f || (NPC.velocity.X == 0 && NPC.velocity.Y == 0 && NPC.ai[0] == 5f); // NPC.ai[1] == 1f for sitting on furniture, NPC.ai[0] == 5f for general sitting state in aiStyle 7
            if (NPC.IsABestiaryIconDummy) // Don't force sitting animation for bestiary icon unless intended
            {
                isSitting = false;
            }

            // Check if the NPC is talking using methods that actually exist in this dogshit API
            bool isTalking = Main.player[Main.myPlayer].talkNPC == NPC.whoAmI || NPC.ai[0] == 23f;

            if (isTalking && !isSitting) // Talking takes priority if not sitting
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 8) // Adjust animation speed for talking (e.g., 8 ticks per frame)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                    // Loop between talking frames 14 and 15
                    if (NPC.frame.Y < TalkingFrameStart * frameHeight || NPC.frame.Y > TalkingFrameEnd * frameHeight)
                    {
                        NPC.frame.Y = TalkingFrameStart * frameHeight;
                    }
                }
            }
            else if (isSitting) // NPC is sitting
            {
                NPC.frame.Y = SittingFrame * frameHeight; // Set to the single sitting frame
                NPC.frameCounter = 0; // Reset frame counter as sitting is a static frame
            }
            else // Not talking and not sitting: NPC is either walking or standing
            {
                if (NPC.velocity.X == 0f && NPC.velocity.Y == 0f) // Standing still
                {
                    NPC.frame.Y = StandingFrame * frameHeight;
                    NPC.frameCounter = 0;
                }
                else // Walking
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 4) // Animation speed for walking (4 ticks per frame)
                    {
                        NPC.frameCounter = 0;

                        // If current frame is not a walking frame (e.g., was standing or talking), start walking animation from the first walking frame.
                        if (NPC.frame.Y < WalkingFrameStart * frameHeight || NPC.frame.Y > WalkingFrameEnd * frameHeight)
                        {
                            NPC.frame.Y = WalkingFrameStart * frameHeight;
                        }
                        else
                        {
                            NPC.frame.Y += frameHeight; // Advance to the next walking frame
                                                        // If it goes past the last walking frame, loop back to the first.
                            if (NPC.frame.Y > WalkingFrameEnd * frameHeight)
                            {
                                NPC.frame.Y = WalkingFrameStart * frameHeight;
                            }
                        }
                    }
                }
            }
        }
    }
}