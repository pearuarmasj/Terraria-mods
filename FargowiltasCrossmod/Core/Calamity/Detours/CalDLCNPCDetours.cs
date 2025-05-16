﻿using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.Projectiles;
using CalamityMod.World;
using CalamityMod;
using FargowiltasSouls.Core.Systems;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityMod.Items;
using FargowiltasSouls.Content.Items.Weapons.Challengers;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls;
using Luminance.Core.Hooking;
using Fargowiltas.NPCs;
using CalamityMod.Projectiles.Boss;
using FargowiltasSouls.Content.Projectiles;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using Fargowiltas.Items;
using CalamityMod.Items.Potions;
using FargowiltasSouls.Content.Bosses.Champions.Nature;
using FargowiltasSouls.Content.Bosses.Champions.Terra;
using FargowiltasSouls.Content.Bosses.VanillaEternity;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CalamityMod.CalPlayer;
using FargowiltasSouls.Core.Toggler;
using FargowiltasCrossmod.Content.Calamity.Items.Accessories;
using FargowiltasCrossmod.Content.Calamity.Items.Accessories.Enchantments;
using FargowiltasCrossmod.Content.Calamity.Items.Accessories.Souls;
using FargowiltasCrossmod.Content.Calamity.Toggles;
using CalamityMod.Systems;
using CalamityMod.Enums;
using Fargowiltas.Items.Vanity;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using FargowiltasSouls.Content.Items;
using FargowiltasSouls.Content.UI.Elements;
using Terraria.Localization;
using CalamityMod.Skies;
using Terraria.Graphics.Effects;
using FargowiltasSouls.Content.UI;
using CalamityMod.NPCs.Perforator;
using FargowiltasCrossmod.Content.Calamity.Bosses.Perforators;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Common.Utilities;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Pets;
using FargowiltasSouls.Content.Items.Misc;
using Fargowiltas.Items.Explosives;
using FargowiltasSouls.Content.Items.Accessories.Masomode;
using Fargowiltas.Items.Tiles;
using CalamityMod.Walls;
using CalamityMod.Tiles.Abyss;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using CalamityMod.Tiles.FurnitureAcidwood;
using CalamityMod.Tiles.FurnitureVoid;
using CalamityMod.Tiles.FurnitureAbyss;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.FurnitureMonolith;
using CalamityMod.Tiles.Crags;
using CalamityMod.Tiles.FurnitureAshen;
using CalamityMod.Tiles.FurnitureEutrophic;
using CalamityMod.Tiles.SunkenSea;
using FargowiltasCrossmod.Core.Calamity.Globals;
using Terraria.GameContent;
using CalamityMod.Items.Accessories;

namespace FargowiltasCrossmod.Core.Calamity.Systems
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class CalDLCNPCDetours : ModSystem, ICustomDetourProvider
    {
        #pragma warning disable CS8601

        public override void Load()
        {
            
        }
        void ICustomDetourProvider.ModifyMethods()
        {
            HookHelper.ModifyMethodWithDetour(CalamityPreAIMethod, CalamityPreAI_Detour);
            HookHelper.ModifyMethodWithDetour(CalamityAIMethod, CalamityAI_Detour);
            HookHelper.ModifyMethodWithDetour(CalamityOtherStatChangesMethod, CalamityOtherStatChanges_Detour);
            HookHelper.ModifyMethodWithDetour(CalamityPreDrawMethod, CalamityPreDraw_Detour);
            HookHelper.ModifyMethodWithDetour(CalamityPostDrawMethod, CalamityPostDraw_Detour);
            HookHelper.ModifyMethodWithDetour(CalamityBossHeadSlotMethod, CalamityBossHeadSlot_Detour);

            HookHelper.ModifyMethodWithDetour(CalamityGetNPCDamageMethod, CalamityGetNPCDamage_Detour);

            HookHelper.ModifyMethodWithDetour(NatureChampAIMethod, NatureChampAI_Detour);
            HookHelper.ModifyMethodWithDetour(TerraChampAIMethod, TerraChampAI_Detour);
            HookHelper.ModifyMethodWithDetour(CheckTempleWallsMethod, CheckTempleWalls_Detour);
            HookHelper.ModifyMethodWithDetour(DukeFishronPreAIMethod, DukeFishronPreAI_Detour);
            HookHelper.ModifyMethodWithDetour(MoonLordIsProjectileValid_Method, MoonLordIsProjectileValid_Detour);

            HookHelper.ModifyMethodWithDetour(MediumPerforatorHeadOnKill_Method, MediumPerforatorHeadOnKill_Detour);
            HookHelper.ModifyMethodWithDetour(MediumPerforatorBodyOnKill_Method, MediumPerforatorBodyOnKill_Detour);
            HookHelper.ModifyMethodWithDetour(MediumPerforatorTailOnKill_Method, MediumPerforatorTailOnKill_Detour);

            HookHelper.ModifyMethodWithDetour(EmodeEditSpawnPool_Method, EmodeEditSpawnPool_Detour);
        }
        private static readonly MethodInfo CalamityPreAIMethod = typeof(CalamityGlobalNPC).GetMethod("PreAI", LumUtils.UniversalBindingFlags);
        public delegate bool Orig_CalamityPreAI(CalamityGlobalNPC self, NPC npc);
        internal static bool CalamityPreAI_Detour(Orig_CalamityPreAI orig, CalamityGlobalNPC self, NPC npc)
        {
            bool wasRevenge = CalamityWorld.revenge;
            bool wasDeath = CalamityWorld.death;
            bool wasBossRush = BossRushEvent.BossRushActive;
            bool shouldDisable = CalDLCWorldSavingSystem.E_EternityRev;

            int defDamage = npc.defDamage; // do not fuck with defDamage please

            if (shouldDisable)
            {
                CalamityWorld.revenge = false;
                CalamityWorld.death = false;
                BossRushEvent.BossRushActive = false;
            }
            bool result = orig(self, npc);
            if (shouldDisable)
            {
                CalamityWorld.revenge = wasRevenge;
                CalamityWorld.death = wasDeath;
                BossRushEvent.BossRushActive = wasBossRush;
                npc.defDamage = defDamage; // do not fuck with defDamage please
            }
            return result;
        }
        private static readonly MethodInfo CalamityAIMethod = typeof(CalamityGlobalNPC).GetMethod("AI", LumUtils.UniversalBindingFlags);
        public delegate void Orig_CalamityAI(CalamityGlobalNPC self, NPC npc);

        internal static void CalamityAI_Detour(Orig_CalamityAI orig, CalamityGlobalNPC self, NPC npc)
        {
            bool wasRevenge = CalamityWorld.revenge;
            bool wasDeath = CalamityWorld.death;
            bool wasBossRush = BossRushEvent.BossRushActive;
            bool shouldDisable = CalDLCWorldSavingSystem.E_EternityRev;

            int defDamage = npc.defDamage; // do not fuck with defDamage please

            if (shouldDisable)
            {
                CalamityWorld.revenge = false;
                CalamityWorld.death = false;
                BossRushEvent.BossRushActive = false;
            }
            orig(self, npc);
            if (shouldDisable)
            {
                CalamityWorld.revenge = wasRevenge;
                CalamityWorld.death = wasDeath;
                BossRushEvent.BossRushActive = wasBossRush;
                npc.defDamage = defDamage; // do not fuck with defDamage please
            }
        }
        private static readonly MethodInfo CalamityOtherStatChangesMethod = typeof(CalamityGlobalNPC).GetMethod("OtherStatChanges", LumUtils.UniversalBindingFlags);
        public delegate void Orig_CalamityOtherStatChanges(CalamityGlobalNPC self, NPC npc);

        internal static void CalamityOtherStatChanges_Detour(Orig_CalamityOtherStatChanges orig, CalamityGlobalNPC self, NPC npc)
        {
            orig(self, npc);
            if (!CalDLCWorldSavingSystem.E_EternityRev)
                return;
            switch (npc.type)
            {
                case NPCID.DetonatingBubble:
                    if (NPC.AnyNPCs(NPCID.DukeFishron))
                        npc.dontTakeDamage = false;
                    break;
                default:
                    break;
            }
        }
        private static readonly MethodInfo CalamityPreDrawMethod = typeof(CalamityGlobalNPC).GetMethod("PreDraw", LumUtils.UniversalBindingFlags);
        public delegate bool Orig_CalamityPreDraw(CalamityGlobalNPC self, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor);

        internal static bool CalamityPreDraw_Detour(Orig_CalamityPreDraw orig, CalamityGlobalNPC self, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            bool wasRevenge = CalamityWorld.revenge;
            bool wasBossRush = BossRushEvent.BossRushActive;
            bool shouldDisableNPC = CalamityLists.DestroyerIDs.Contains(npc.type) || npc.type == NPCID.SkeletronPrime;
            bool shouldDisable = CalDLCWorldSavingSystem.E_EternityRev && shouldDisableNPC;

            if (shouldDisable)
            {
                CalamityWorld.revenge = false;
                BossRushEvent.BossRushActive = false;
            }
            bool result = orig(self, npc, spriteBatch, screenPos, drawColor);
            if (shouldDisable)
            {
                CalamityWorld.revenge = wasRevenge;
                BossRushEvent.BossRushActive = wasBossRush;
            }
            return result;
        }

        private static readonly List<int> DisablePostDrawNPCS = new(CalamityLists.DestroyerIDs)
            {
            NPCID.WallofFleshEye,
            NPCID.Creeper,
            NPCID.SkeletronPrime
            };
        private static readonly MethodInfo CalamityPostDrawMethod = typeof(CalamityGlobalNPC).GetMethod("PostDraw", LumUtils.UniversalBindingFlags);
        public delegate void Orig_CalamityPostDraw(CalamityGlobalNPC self, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor);
        internal static void CalamityPostDraw_Detour(Orig_CalamityPostDraw orig, CalamityGlobalNPC self, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            bool shouldDisableNPC = DisablePostDrawNPCS.Contains(npc.type);
            bool shouldDisable = CalDLCWorldSavingSystem.E_EternityRev && shouldDisableNPC;
            if (shouldDisable)
            {
                return;
            }
            orig(self, npc, spriteBatch, screenPos, drawColor);
        }
        private static readonly MethodInfo CalamityBossHeadSlotMethod = typeof(CalamityGlobalNPC).GetMethod("BossHeadSlot", LumUtils.UniversalBindingFlags);
        public delegate void Orig_CalamityBossHeadSlot(CalamityGlobalNPC self, NPC npc, ref int index);
        internal static void CalamityBossHeadSlot_Detour(Orig_CalamityBossHeadSlot orig, CalamityGlobalNPC self, NPC npc, ref int index)
        {
            bool shouldDisable = CalDLCWorldSavingSystem.E_EternityRev;
            if (shouldDisable)
                return;
            orig(self, npc, ref index);
        }

        private static readonly MethodInfo CalamityGetNPCDamageMethod = typeof(NPCStats).GetMethod("GetNPCDamage", LumUtils.UniversalBindingFlags);
        public delegate void Orig_CalamityGetNPCDamage(NPC npc);
        internal static void CalamityGetNPCDamage_Detour(Orig_CalamityGetNPCDamage orig, NPC npc)
        {
            // Prevent vanilla bosses and their segments from having their damage overriden by Calamity
            bool countsAsBoss = npc.boss || NPCID.Sets.ShouldBeCountedAsBoss[npc.type];
            if (npc.type < NPCID.Count && (countsAsBoss || CalamityLists.bossHPScaleList.Contains(npc.type)))
                return;
            orig(npc);
        }
        private static readonly MethodInfo NatureChampAIMethod = typeof(NatureChampion).GetMethod("AI", LumUtils.UniversalBindingFlags);
        public delegate void Orig_NatureChampAI(NatureChampion self);
        internal static void NatureChampAI_Detour(Orig_NatureChampAI orig, NatureChampion self)
        {
            NPC npc = self.NPC;
            double originalSurface = Main.worldSurface;
            if (BossRushEvent.BossRushActive)
            {
                Main.worldSurface = 0;
            }
            orig(self);
            if (BossRushEvent.BossRushActive)
            {
                Main.worldSurface = originalSurface;
            }
        }

        private static readonly MethodInfo TerraChampAIMethod = typeof(TerraChampion).GetMethod("AI", LumUtils.UniversalBindingFlags);
        public delegate void Orig_TerraChampAI(TerraChampion self);
        internal static void TerraChampAI_Detour(Orig_TerraChampAI orig, TerraChampion self)
        {
            NPC npc = self.NPC;
            double originalSurface = Main.worldSurface;
            if (BossRushEvent.BossRushActive)
            {
                Main.worldSurface = 0;
            }
            orig(self);
            if (BossRushEvent.BossRushActive)
            {
                Main.worldSurface = originalSurface;
            }
        }
        private static readonly MethodInfo CheckTempleWallsMethod = typeof(Golem).GetMethod("CheckTempleWalls", LumUtils.UniversalBindingFlags);
        public delegate bool Orig_CheckTempleWalls(Vector2 pos);
        internal static bool CheckTempleWalls_Detour(Orig_CheckTempleWalls orig, Vector2 pos)
        {

            if (BossRushEvent.BossRushActive)
            {
                return true;
            }
            return orig(pos);
        }

        private static readonly MethodInfo DukeFishronPreAIMethod = typeof(DukeFishron).GetMethod("SafePreAI", LumUtils.UniversalBindingFlags);
        public delegate bool Orig_DukeFishronPreAI(DukeFishron self, NPC npc);
        internal static bool DukeFishronPreAI_Detour(Orig_DukeFishronPreAI orig, DukeFishron self, NPC npc)
        {
            if (BossRushEvent.BossRushActive && npc.HasValidTarget)
            {
                Main.player[npc.target].ZoneBeach = true;
            }
            bool result = orig(self, npc);
            if (BossRushEvent.BossRushActive && npc.HasValidTarget)
            {
                Main.player[npc.target].ZoneBeach = false;
            }
            return result;
        }
        private static readonly MethodInfo MoonLordIsProjectileValid_Method = typeof(MoonLord).GetMethod("IsProjectileValid", LumUtils.UniversalBindingFlags);
        public delegate bool Orig_MoonLordIsProjectileValid(MoonLord self, NPC npc, Projectile projectile);
        internal static bool MoonLordIsProjectileValid_Detour(Orig_MoonLordIsProjectileValid orig, MoonLord self, NPC npc, Projectile projectile)
        {
            bool ret = orig(self, npc, projectile);
            if (!Main.player[projectile.owner].buffImmune[ModContent.BuffType<NullificationCurseBuff>()])
            {

                switch (self.GetVulnerabilityState(npc))
                {
                    case 0: //if (!projectile.CountsAsClass(DamageClass.Melee)) return false; break; melee
                        if (projectile.CountsAsClass<RogueDamageClass>())
                            ret = true;
                        break;
                    //case 1: if (!projectile.CountsAsClass(DamageClass.Ranged)) return false; break;
                    //case 2: if (!projectile.CountsAsClass(DamageClass.Magic)) return false; break;
                    //case 3: if (!FargoSoulsUtil.IsSummonDamage(projectile)) return false; break;
                    default: break;
                }
            }
            return ret;
        }
        private static readonly MethodInfo MediumPerforatorHeadOnKill_Method = typeof(PerforatorHeadMedium).GetMethod("OnKill", LumUtils.UniversalBindingFlags);
        public delegate void Orig_MediumPerforatorHeadOnKill(PerforatorHeadMedium self);
        internal static void MediumPerforatorHeadOnKill_Detour(Orig_MediumPerforatorHeadOnKill orig, PerforatorHeadMedium self)
        {
            if (CalDLCWorldSavingSystem.E_EternityRev)
                return;
            orig(self);
        }
        private static readonly MethodInfo MediumPerforatorBodyOnKill_Method = typeof(PerforatorBodyMedium).GetMethod("OnKill", LumUtils.UniversalBindingFlags);
        public delegate void Orig_MediumPerforatorBodyOnKill(PerforatorBodyMedium self);

        internal static void MediumPerforatorBodyOnKill_Detour(Orig_MediumPerforatorBodyOnKill orig, PerforatorBodyMedium self)
        {
            if (CalDLCWorldSavingSystem.E_EternityRev)
                return;
            orig(self);
        }
        private static readonly MethodInfo MediumPerforatorTailOnKill_Method = typeof(PerforatorTailMedium).GetMethod("OnKill", LumUtils.UniversalBindingFlags);
        public delegate void Orig_MediumPerforatorTailOnKill(PerforatorTailMedium self);

        internal static void MediumPerforatorTailOnKill_Detour(Orig_MediumPerforatorTailOnKill orig, PerforatorTailMedium self)
        {
            if (CalDLCWorldSavingSystem.E_EternityRev)
                return;
            orig(self);
        }

        private static readonly MethodInfo EmodeEditSpawnPool_Method = typeof(EModeGlobalNPC).GetMethod("EditSpawnPool", LumUtils.UniversalBindingFlags);
        public delegate void Orig_EmodeEditSpawnPool(EModeGlobalNPC self, IDictionary<int, float> pool, NPCSpawnInfo spawnInfo);

        internal static void EmodeEditSpawnPool_Detour(Orig_EmodeEditSpawnPool orig, EModeGlobalNPC self, IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            var cal = spawnInfo.Player.Calamity();

            bool calamityBiomeZone = cal.ZoneAbyss ||
                cal.ZoneCalamity ||
                cal.ZoneSulphur ||
                cal.ZoneSunkenSea ||
                cal.ZoneAstral && !spawnInfo.Player.PillarZone();
            if (calamityBiomeZone || pool[0] == 0)
                return;
            orig(self, pool, spawnInfo);

        }
    }
}
