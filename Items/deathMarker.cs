using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using System;

namespace hyperFocus.Items {
	public class deathMarker : ModProjectile {

        scabbard Scabbard = new scabbard();

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Death Marker");
		}
        public override void SetDefaults() {

			Projectile.width = 18;
			Projectile.height = 18;
            Projectile.scale = 1f;
            //Projectile.damage = 0;
            Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }


        public override bool PreDraw(ref Color lightColor) {

			SpriteEffects spriteEffects = SpriteEffects.None;

			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

			Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			Vector2 origin = sourceRectangle.Size() / 2f;

			Color drawColor = Projectile.GetAlpha(lightColor);

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, Color.White * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale * 2f * scaleMultiplier, spriteEffects, 0);

			return false;
		}

        public static int projLifeTime = 50;

        float scaleMultiplier = 1;

        public override void OnSpawn(IEntitySource source) {
            scaleMultiplier = 1;
            Projectile.Opacity = 1f;
        }

        public override void AI() {

            var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.LifeDrain, 0f, 1f, 255, Color.Pink, 0.5f);

            scaleMultiplier += 0.018f;
            Projectile.Opacity -= 0.018f;

            Projectile.ai[0]++;

            Player owner = Main.player[Projectile.owner];

            //Projectile.position += new Vector2(0, -20f);
            //Main.NewText(Projectile.ai[0]);

            if (Projectile.ai[0] >= projLifeTime) {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), owner.Top + new Vector2(0, -30), owner.velocity, ModContent.ProjectileType<deathMarkerBroken>(), 1, 0f, owner.whoAmI);
                Projectile.Kill();
            }
            if (Projectile.ai[0] >= projLifeTime - 5) {
                dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.LifeDrain, 0f, -8f, 255, Color.Pink, 1f);
            }
            if (Projectile.ai[0] == projLifeTime - 5) {
                Projectile.CritChance = 100;
                Projectile.damage = 2048;
            }
            if (Projectile.ai[0] == projLifeTime - 4) {
                Projectile.CritChance = 0;
                Projectile.damage = 1;
            }
            Projectile.timeLeft = 2;

            Lighting.AddLight(Projectile.Center, 255/255, 43/255, 83/255);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            scabbard.rageCharge += 10;
            Projectile.penetrate = -1;
            Projectile.velocity = target.velocity;
            Projectile.position = target.TopLeft;
            if (!target.active) {
                Projectile.Kill();
                scabbard.rageCharge += 1800;
            }
        }
    }
}