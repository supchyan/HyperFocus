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
	public class deathMarkerBroken : ModProjectile {
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Broken Death Marker");
            Main.projFrames[Projectile.type] = 2;
		}
        public override void SetDefaults() {

			Projectile.width = 18;
			Projectile.height = 18;
            Projectile.scale = 1f;
            //Projectile.damage = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
            Projectile.ai[0] = 0;
        }
        public void AnimeProjectile() {

			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
                
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
			
		}

        public override bool PreDraw(ref Color lightColor) {

			SpriteEffects spriteEffects = SpriteEffects.None;

			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;

			Color drawColor = Projectile.GetAlpha(lightColor);

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, Color.White * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale * 2f * scaleMultiplier, spriteEffects, 0);

			return false;
		}

        public static int projLifeTime = 10;

        float scaleMultiplier = 1;

        SoundStyle deathMarkerCyan = new SoundStyle("hyperFocus/Sounds/deathMarkerCyan");
        public override void OnSpawn(IEntitySource source) {
            scaleMultiplier = 1;
            Projectile.Opacity = 1f;
            SoundEngine.PlaySound(deathMarkerCyan);
        }
        public override void AI() {

            //Main.NewText(Projectile.ai[0]);

            //var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.LifeDrain, 0f, 1f, 255, Color.Pink, 0.5f);

            scaleMultiplier += 0.1f;
            Projectile.Opacity -= 0.1f;

            Projectile.ai[0]++;

            Player owner = Main.player[Projectile.owner];

            Projectile.velocity = owner.velocity;

            //Projectile.position += new Vector2(0, -20f);

            //Main.NewText(Projectile.ai[0]);

            if (Projectile.ai[0] >= projLifeTime) {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;

            Lighting.AddLight(Projectile.Center, 0.5f, 1, 0.7f);

            AnimeProjectile();

        }
    }
}