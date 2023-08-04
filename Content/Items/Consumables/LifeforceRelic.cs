using ChaoticUprising.Content.Rarities;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class LifeforceRelic : ModItem
    {
        public const int Life = 20;
        public const int Max = 20;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			bool full = Main.LocalPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics == Max;
			tooltips[2].Text = tooltips[2].Text.FormatWith(Life);
			tooltips[1].Text = (full ? "" : "Consumable\n") + Main.LocalPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics + "/" + Max;
			tooltips[1].OverrideColor = full ? new Color(0, 255, 0) : Color.White;
		}

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeFruit);
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.width = 42;
            Item.height = 40;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLifeMax >= 500 && player.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics < Max;
        }

        public override bool? UseItem(Player player)
        {
            player.statLifeMax2 += Life;
            player.statLife += Life;
            if (Main.myPlayer == player.whoAmI)
                player.HealEffect(Life);
            player.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics++;
            return true;
        }
    }

    public class LifeforceRelicPlayer : ModPlayer
    {
        public int lifeforceRelics;

        public override void ResetEffects()
        {
            Player.statLifeMax2 += lifeforceRelics * LifeforceRelic.Life;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)ChaoticUprising.PacketType.LifeforceRelicsSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write(lifeforceRelics);
            packet.Send(toWho, fromWho);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["lifeforceRelics"] = lifeforceRelics;
        }

        public override void LoadData(TagCompound tag)
        {
            lifeforceRelics = (int)tag["lifeforceRelics"];
        }
    }

    public class LifeforceRelicUI : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBars = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars")) + 1;
            if (resourceBars != -1) layers.Insert(resourceBars, new LegacyGameInterfaceLayer("ChaoticUprising: Lifeforce Relics", delegate { Draw(Main.spriteBatch); return true; }, InterfaceScaleType.UI));
        }

        private void Draw(SpriteBatch spriteBatch)
		{
			string type = Main.ResourceSetsManager.ActiveSetKeyName;

			Player localPlayer = Main.LocalPlayer;
			if (localPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics > 0)
            {
				if (type == "HorizontalBars")
				{
					bool isHovered = false;
					int num = 16;
					int num2 = 18;
					int num3 = Main.screenWidth - 300 - 22 + num;
					Vector2 vector = new Vector2(num3, num2);
					PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(Main.LocalPlayer);
					ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
					resourceDrawSettings.ElementCount = 20;
					resourceDrawSettings.ElementIndexOffset = 0;
					resourceDrawSettings.TopLeftAnchor = vector + new Vector2(6f, 6f);
					resourceDrawSettings.GetTextureMethod = LifeFillingDrawer;
					resourceDrawSettings.OffsetPerDraw = new Vector2(12f, 0f);
					resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
					resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
					resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
					resourceDrawSettings.Draw(spriteBatch, ref isHovered);
				}
				else if (type == "New")
				{
					bool isHovered = false;
					Vector2 vector = new Vector2(Main.screenWidth - 300 + 4, 15f);
					ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
					resourceDrawSettings = default(ResourceDrawSettings);
					resourceDrawSettings.ElementCount = 10;
					resourceDrawSettings.ElementIndexOffset = 0;
					resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f);
					resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
					resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
					resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
					resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
					resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
					resourceDrawSettings.Draw(spriteBatch, ref isHovered);
					resourceDrawSettings = default(ResourceDrawSettings);
					resourceDrawSettings.ElementCount = 10;
					resourceDrawSettings.ElementIndexOffset = 10;
					resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f) + new Vector2(0f, 28f);
					resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
					resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
					resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
					resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
					resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
					resourceDrawSettings.Draw(spriteBatch, ref isHovered);
				}
				else
				{
					Texture2D lifeforceHeartTexture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/Heart_Fill_C");
					int UI_ScreenAnchorX = Main.screenWidth - 800;
					float UIDisplay_LifePerHeart = 20f;
					if (localPlayer.ghost)
					{
						return;
					}
					int num = localPlayer.statLifeMax / 20;
					int num2 = (localPlayer.statLifeMax - 400) / 5;
					if (num2 < 0)
					{
						num2 = 0;
					}
					if (num2 > 0)
					{
						num = localPlayer.statLifeMax / (20 + num2 / 4);
						UIDisplay_LifePerHeart = (float)localPlayer.statLifeMax / 20f;
					}
					int num3 = localPlayer.statLifeMax2 - localPlayer.statLifeMax;
					UIDisplay_LifePerHeart += num3 / num;
					int lifeforceRelics = localPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics;
					for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart) + 1; i++)
					{
						int num5;
						float num6 = 1f;
						bool flag = false;
						if ((float)localPlayer.statLife >= (float)i * UIDisplay_LifePerHeart)
						{
							num5 = 255;
							if ((float)localPlayer.statLife == (float)i * UIDisplay_LifePerHeart)
							{
								flag = true;
							}
						}
						else
						{
							float num7 = ((float)localPlayer.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
							num5 = (int)(30f + 225f * num7);
							if (num5 < 30)
							{
								num5 = 30;
							}
							num6 = num7 / 4f + 0.75f;
							if ((double)num6 < 0.75)
							{
								num6 = 0.75f;
							}
							if (num7 > 0f)
							{
								flag = true;
							}
						}
						if (flag)
						{
							num6 += Main.cursorScale - 1f;
						}
						int num8 = 0;
						int num9 = 0;
						if (i > 10)
						{
							num8 -= 260;
							num9 += 26;
						}
						int a = (int)((double)num5 * 0.9);
						if (!localPlayer.ghost)
						{
							if (lifeforceRelics > 0)
							{
								lifeforceRelics--;
								spriteBatch.Draw(lifeforceHeartTexture, new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2, 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, a), 0f, new Vector2(TextureAssets.Heart.Width() / 2, TextureAssets.Heart.Height() / 2), num6, SpriteEffects.None, 0f);
							}
							else
							{
								spriteBatch.Draw((Texture2D)TextureAssets.Heart2, new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2, 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, a), 0f, new Vector2(TextureAssets.Heart.Width() / 2, TextureAssets.Heart.Height() / 2), num6, SpriteEffects.None, 0f);
							}
						}
					}
				}
			}
        }

		private void LifeFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/HP_Fill_Honey");
			if (elementIndex >= 20 - Main.LocalPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics)
			{
				sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/HP_Fill_LifeForce");
			}
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(Main.LocalPlayer);

			FillBarByValues(elementIndex, sprite, 20, (float)playerStatsSnapshot.Life / (float)playerStatsSnapshot.LifeMax, out offset, out drawScale, out sourceRect);
		}

		private static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			float num = 1f;
			float num2 = 1f / (float)segmentsCount;
			float t = 1f - fillPercent;
			float lerpValue = Utils.GetLerpValue(num2 * (float)elementIndex, num2 * (float)(elementIndex + 1), t, clamped: true);
			num = 1f - lerpValue;
			drawScale = 1f;
			Rectangle value = sprite.Frame();
			int num3 = (int)((float)value.Width * (1f - num));
			offset.X += num3;
			value.X += num3;
			value.Width -= num3;
			sourceRect = value;
		}

		private void HeartFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			if (elementIndex < Main.LocalPlayer.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics)
			{
				sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/Heart_Fill_C");
			}
			else
			{
				sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/Heart_Fill_B");
			}
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(Main.LocalPlayer);
			float _lifePerHeart = playerStatsSnapshot.LifePerSegment;
			int _currentPlayerLife = playerStatsSnapshot.Life;
			int _lastHeartFillingIndex = (int)((float)playerStatsSnapshot.Life / _lifePerHeart);
			float num = (drawScale = Utils.GetLerpValue(_lifePerHeart * (float)elementIndex, _lifePerHeart * (float)(elementIndex + 1), _currentPlayerLife, clamped: true));
			if (elementIndex == _lastHeartFillingIndex && num > 0f)
			{
				drawScale += Main.cursorScale - 1f;
			}
		}
	}
}
