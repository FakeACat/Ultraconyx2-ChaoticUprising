using ChaoticUprising.Content.Items.Consumables;
using ChaoticUprising.Content.Items.Pets;
using ChaoticUprising.Content.Items.Placeables;
using ChaoticUprising.Content.Items.Vanity;
using ChaoticUprising.IDs;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.Systems
{
    public class BossChecklistCompat : ModSystem
    {
        private Mod bossChecklist;
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                this.bossChecklist = bossChecklist;

                RegisterBosses();
            }
        }

        private void RegisterBosses()
        {
            RegisterBoss(CUNPCID.AbyssalChaos,
                "AbyssalChaos",
                19.0f, 
                () => ChaosMode.chaosMode, 
                ModContent.ItemType<CorruptedSkull>(),
                new() 
                { 
                    ModContent.ItemType<AbyssalChaosRelic>(), 
                    ModContent.ItemType<AbyssalChaosTrophy>(), 
                    ModContent.ItemType<AbyssalChaosMask>(), 
                    ModContent.ItemType<AbyssalSkull>(), 
                    ModContent.ItemType<AbyssalChaosMusicBox>() 
                });

            RegisterMiniBoss(CUNPCID.NightmareReaper,
                "NightmareReaper",
                19.5f,
                () => DownedBosses.downedNightmareReaper,
                null,
                new()
                {
                    ModContent.ItemType<ReaperEssence>()
                });
        }

        private void RegisterBoss(int bossID,
            string internalName,
            float weight,
            Func<bool> downed,
            int? spawnItem,
            List<int> collectibles,
            int? customPortrait = null)
        {
            Register("LogBoss", internalName, bossID, weight, downed, spawnItem, collectibles, customPortrait);
        }

        private void RegisterMiniBoss(int bossID,
            string internalName,
            float weight,
            Func<bool> downed,
            int? spawnItem,
            List<int> collectibles,
            int? customPortrait = null)
        {
            Register("LogMiniBoss", internalName, bossID, weight, downed, spawnItem, collectibles, customPortrait);
        }

        private void Register(string type,
            string internalName,
            int bossID, 
            float weight, 
            Func<bool> downed,
            int? spawnItem,
            List<int> collectibles,
            int? customPortrait)
        {
            Dictionary<string, object> dictionary = new()
            {
                ["collectibles"] = collectibles
            };
            if (spawnItem != null)
            {
                dictionary.Add("spawnItems", spawnItem);
            }
            if (customPortrait != null)
            {
                dictionary.Add("customPortrait", customPortrait);
            }

            bossChecklist.Call(
                type,
                Mod,
                internalName,
                weight,
                downed,
                bossID,
                dictionary
            );
        }
    }
}
