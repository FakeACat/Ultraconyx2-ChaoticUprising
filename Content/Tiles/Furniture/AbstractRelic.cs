﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChaoticUprising.Content.Tiles.Furniture
{
    public abstract class AbstractRelic : ModTile
    {
        public const int FrameWidth = 18 * 3;
        public const int FrameHeight = 18 * 4;

		public Asset<Texture2D> RelicTexture;

		public abstract string RelicTextureName();

		public abstract int ItemType();

		public override string Texture => "ChaoticUprising/Content/Tiles/Furniture/RelicPedestal";

		public override void Load()
		{
			if (!Main.dedServ)
			{
				RelicTexture = ModContent.Request<Texture2D>(RelicTextureName());
			}
		}

		public override void Unload()
		{
			RelicTexture = null;
		}

		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 400;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemType());
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
				Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 offScreen = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
				offScreen = Vector2.Zero;

			Point p = new Point(i, j);
			Tile tile = Main.tile[p.X, p.Y];
			if (tile == null || !tile.HasTile)
				return;

			Texture2D texture = RelicTexture.Value;

			Rectangle frame = texture.Frame(1, 1, 0, 0);

			Vector2 origin = frame.Size() / 2f;
			Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

			Color color = Lighting.GetColor(p.X, p.Y);

			bool direction = tile.TileFrameY / FrameHeight != 0;
			SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			const float TwoPi = (float)Math.PI * 2f;
			float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
			Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -40f) + new Vector2(0f, offset * 4f);

			spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);

			float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
			Color effectColor = color;
			effectColor.A = 0;
			effectColor = effectColor * 0.1f * scale;
			for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
				spriteBatch.Draw(texture, drawPos + (TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1f, effects, 0f);
		}
	}
}
