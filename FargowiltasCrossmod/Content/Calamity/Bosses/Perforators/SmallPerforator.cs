﻿using System;
using System.Collections.Generic;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Particles;
using FargowiltasCrossmod.Core;
using FargowiltasCrossmod.Core.Calamity;
using FargowiltasCrossmod.Core.Calamity.Globals;
using FargowiltasSouls;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasCrossmod.Content.Calamity.Bosses.Perforators
{
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    public class SmallPerforator : CalDLCEmodeExtraGlobalNPC
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchTypeRange(
        ModContent.NPCType<PerforatorHeadSmall>(),
        ModContent.NPCType<PerforatorBodySmall>(),
        ModContent.NPCType<PerforatorTailSmall>()
        );
        public override void SetDefaults(NPC entity)
        {
            if (!WorldSavingSystem.EternityMode)
                return;
            entity.CalamityDLC().ImmuneToAllDebuffs = true;
            entity.damage = 60;
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (!WorldSavingSystem.EternityMode) 
                return base.CanHitPlayer(npc, target, ref cooldownSlot);

            if (npc.type == ModContent.NPCType<PerforatorHeadSmall>())
            {
                if (npc.ai[3] < 60)
                    return false;
            }
            else
            {
                NPC owner = npc.realLife >= 0 ? Main.npc[npc.realLife] : null;
                if (owner == null || owner.ai[3] < 60)
                    return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!WorldSavingSystem.EternityMode)
                return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
            if (npc.type == ModContent.NPCType<PerforatorHeadSmall>())
            {
                if (npc.ai[3] < 60)
                {
                    Main.spriteBatch.UseBlendState(BlendState.Additive);

                    Texture2D tex = TextureAssets.Npc[npc.type].Value;
                    Color glowColor = Color.Red;
                    SpriteEffects spriteEffects = SpriteEffects.None;
                    if (npc.spriteDirection == 1)
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    for (int j = 0; j < 12; j++)
                    {
                        Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 4f;
                        Main.EntitySpriteDraw(tex, npc.Center + afterimageOffset - Main.screenPosition, null, glowColor, npc.rotation, tex.Size() * 0.5f, npc.scale, spriteEffects, 0f);
                    }

                    Main.spriteBatch.ResetToDefault();
                }
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override bool SafePreAI(NPC npc)
        {
            if (!WorldSavingSystem.EternityMode) 
                return true;
            if (npc.type == ModContent.NPCType<PerforatorBodySmall>() || npc.type == ModContent.NPCType<PerforatorTailSmall>())
            {
                NPC owner = null;
                if (npc.realLife >= 0)
                {
                    owner = Main.npc[npc.realLife];

                }
                if (owner != null && owner.active && owner.ai[2] == 1)
                {
                    npc.velocity = (owner.Center - npc.Center).SafeNormalize(Vector2.Zero) * 15;
                    if (npc.type == ModContent.NPCType<PerforatorTailSmall>() && npc.Distance(owner.Center) <= 20)
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n != null && n.active && new List<int>() { ModContent.NPCType<PerforatorHeadSmall>(), ModContent.NPCType<PerforatorBodySmall>(), ModContent.NPCType<PerforatorTailSmall>() }
                            .Contains(n.type) && (n.realLife == owner.whoAmI || n.whoAmI == owner.whoAmI))
                            {
                                n.active = false;
                            }
                        }
                        owner.active = false;
                    }
                    return false;
                }
                if (owner != null && owner.active && owner.ai[3] < 60)
                {
                    NPC perf = null;
                    foreach (NPC n in Main.npc)
                    {
                        if (n != null && n.active && n.type == ModContent.NPCType<PerforatorHive>())
                        {
                            perf = n;
                        }
                    }
                    npc.TargetClosest();
                    NetSync(npc);
                    if (perf != null && npc.target >= 0)
                    {
                        Player target = Main.player[npc.target];
                        npc.Center = perf.Center + (target.Center - perf.Center).SafeNormalize(Vector2.Zero) * 40;
                        npc.netUpdate = true;
                        return false;
                    }
                }

            }
            if (npc.type == ModContent.NPCType<PerforatorHeadSmall>())
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<PerforatorHive>()))
                {
                    npc.active = false;
                    return false;
                }
                npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver2;
                if (npc.ai[3] >= 10)
                {
                    if (npc.ai[3] < 60)
                    {
                        NPC perf = null;
                        foreach (NPC n in Main.npc)
                        {
                            if (n != null && n.active && n.type == ModContent.NPCType<PerforatorHive>())
                            {
                                perf = n;
                            }
                        }
                        Player target = null;
                        npc.TargetClosest();
                        if (npc.target >= 0)
                        {
                            target = Main.player[npc.target];
                        }
                        if (perf != null && target != null)
                        {
                            Vector2 offsetDir = (target.Center - perf.Center).SafeNormalize(Vector2.Zero);
                            npc.Center = perf.Center + offsetDir * 80;
                            npc.rotation = npc.AngleTo(target.Center) + MathHelper.PiOver2;
                            npc.netUpdate = true;

                            int count = Main.rand.NextBool(3) ? 1 : 0;
                            if (npc.ai[3] == 10) // right when spawning ish
                            {
                                count = 25;
                                npc.netUpdate = true;
                            }
                            for (int i = 0; i < count; i++)
                            {
                                Color color = Main.rand.NextBool(10)  ? Color.Gold : Color.Red;
                                float distance = 80;
                                Vector2 offset = offsetDir.RotatedByRandom(MathHelper.PiOver2 * 0.3f);
                                Particle p = new BloodParticle(perf.Center + offset * distance, offset.SafeNormalize(-Vector2.UnitY) * Main.rand.NextFloat(7, 14), Main.rand.Next(15, 25), Main.rand.NextFloat(0.7f, 1.3f), color);
                                GeneralParticleHandler.SpawnParticle(p);
                            }
                        }
                    }
                    if (npc.ai[3] == 60)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath23, npc.Center);
                        npc.TargetClosest();
                        if (npc.target >= 0)
                        {
                            Player target = Main.player[npc.target];
                            npc.velocity = (target.Center - npc.Center).SafeNormalize(Vector2.Zero) * 20;

                        }
                        npc.netUpdate = true;
                        NetSync(npc);
                    }
                    if (npc.ai[3] >= 120 && npc.ai[2] == 0)
                    {
                        NPC perf = null;
                        foreach (NPC n in Main.npc)
                        {
                            if (n != null && n.active && n.type == ModContent.NPCType<PerforatorHive>())
                            {
                                perf = n;
                            }
                        }
                        if (perf != null && perf.active && perf.type == ModContent.NPCType<PerforatorHive>())
                        {
                            float vel = 20; // 16 + perf.velocity.Length();
                            npc.velocity = Vector2.Lerp(npc.velocity, (perf.Center - npc.Center).SafeNormalize(Vector2.Zero) * vel, 0.1f);
                            if (npc.Distance(perf.Center) <= 20)
                            {
                                npc.ai[2] = 1;
                                npc.netUpdate = true;
                            }
                        }

                    }
                    if (npc.ai[2] == 1)
                    {
                        NPC perf = null;
                        foreach (NPC n in Main.npc)
                        {
                            if (n != null && n.active && n.type == ModContent.NPCType<PerforatorHive>())
                            {
                                perf = n;
                            }
                        }
                        if (perf != null && perf.active && perf.type == ModContent.NPCType<PerforatorHive>())
                        {
                            float vel = 16 + perf.velocity.Length();
                            npc.velocity = Vector2.Lerp(npc.velocity, (perf.Center - npc.Center).SafeNormalize(Vector2.Zero) * vel, 0.03f);
                            if (npc.Distance(perf.Center) <= 20)
                            {
                                npc.ai[2] = 1;
                                npc.netUpdate = true;
                                NetSync(npc);
                            }
                        }
                        npc.Center = perf.Center;
                    }
                    npc.ai[3]++;
                    return false;
                }
                else
                {
                    //???
                }
                npc.ai[3]++;

            }
            return true;
        }
    }
}
