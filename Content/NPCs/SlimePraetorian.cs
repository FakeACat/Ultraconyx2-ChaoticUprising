using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ChaoticUprising.Common.Systems;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using ChaoticUprising.Content.Items.Placeables.Banners;

namespace ChaoticUprising.Content.NPCs
{
    public class SlimePraetorian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { PortraitScale = 0.75f });
        }

        public override void SetDefaults()
        {
            NPC.width = 174;
            NPC.height = 120;
            NPC.aiStyle = 1;
            NPC.damage = 150;
            NPC.defense = 60;
            NPC.lifeMax = 70000;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 30;
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            AnimationType = NPCID.KingSlime;
            Main.npcFrameCount[NPC.type] = 6;
            Banner = Type;
            BannerItem = ModContent.ItemType<SlimePraetorianBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement("A specialist slime created to protect the Slime Progenitor from the abominations brought into this world by the Abyssal Chaos' death."));
        }

        int age = 0;

        int frame = 0;
        int frameChange = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.aiStyle == -1)
            {
                frameChange++;
                if (frameChange >= 10)
                {
                    if (frame < 4)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 3;
                    }
                    frameChange = 0;
                }
                NPC.frame.Y = frame * frameHeight;
            }
        }

        public override void AI()
        {
            if ((!Main.player[NPC.target].active || Main.player[NPC.target].dead) && NPC.aiStyle != 1)
                NPC.aiStyle = 1;
            else if (NPC.life < NPC.lifeMax)
            {
                NPC.aiStyle = -1;
                float speed = 1.0f - (float)NPC.life / NPC.lifeMax / 2;
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -16;
                    NPC.velocity.X = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X * 20 * speed;

                    float num = 5;
                    for (int i = 0; i < num; i++)
                    {
                        float f = (float)NPC.width * i / num;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.position.X + f, NPC.position.Y + NPC.width - 48), new Vector2(Main.rand.NextFloat(-1, 1), -10), ProjectileID.GoldenShowerHostile, 80, 1.0f);
                    }
                }
                NPC.velocity.X += Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X * speed / 2;
                NPC.velocity *= 0.97f;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
                return SpawnCondition.Overworld.Chance * ChaosMode.EliteSpawnMultiplier() / 15;
            else
                return 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/SlimePraetorianAura");

            Vector2 frameOrigin = NPC.frame.Size() / 2f;
            Vector2 drawPos = NPC.Center - Main.screenPosition;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = age / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, NPC.frame, new Color(155, 100, 0, 50) * 0.5f, NPC.rotation, frameOrigin, NPC.scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, NPC.frame, new Color(155, 100, 0, 77) * 0.5f, NPC.rotation, frameOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
