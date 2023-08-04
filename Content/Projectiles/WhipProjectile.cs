using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    // Abstract version of ExampleMod's ExampleWhipProjectile (without the charging mechanic) to serve as a base for CU's whips
    public abstract class WhipProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = Segments();
            Projectile.WhipSettings.RangeMultiplier = RangeMultiplier();
        }

        public abstract int Segments();
        public abstract float RangeMultiplier();
        public abstract Color Colour();
        public abstract float DamageDecreasePerHit();
        public abstract int Tag();

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(Tag(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * (1.0f - DamageDecreasePerHit()));
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Colour());
                Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new();
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(list);

            Main.DrawWhip_WhipBland(Projectile, list);

            return false;
        }
    }
}
