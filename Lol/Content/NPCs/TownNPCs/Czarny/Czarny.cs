using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Lol.Content.NPCs.TownNPCs.Czarny
{
    [AutoloadHead]
    internal class Czarny : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Czarny");
            Main.townNPCCanSpawn[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 26;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 15;
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
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
        }
        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = 40;
            itemHeight = 40;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 1000;
            knockback = 10f;
        }
    }
}