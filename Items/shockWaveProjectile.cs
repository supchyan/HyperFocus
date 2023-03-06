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
	public class shockWaveProjectile : ModProjectile {

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hyper Focus Blade");
            Main.projFrames[Projectile.type] = 5;
		}
        public override void SetDefaults() {

            AnimeProjectile();

            Projectile.aiStyle = 0;
			Projectile.width = 2;
			Projectile.height = 2;
            Projectile.scale = 1f;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }
        public void AnimeProjectile() {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frame++;
				Projectile.frame %= 5;
				Projectile.frameCounter = 0;
			}
			
		}
        Vector2 spawnPosition = Main.LocalPlayer.position;
        public override Color? GetAlpha(Color lightColor) {
			return new Color(1f, 1f, 1f, 255) * Projectile.Opacity;
		}
        int t = 0;
        private int rippleCount = 1;
        private int rippleSize = 100;
        private int rippleSpeed = 15;
        private float distortStrength = 1000f;
        
        public override void AI() {

            Projectile.ai[0]++;
            //Main.NewText(Projectile.ai[0]);
            if (Projectile.ai[0] >= 20) {
                Projectile.Kill();
                Filters.Scene["Shockwave"].Deactivate();
            }

            //Main.NewText(Projectile.ai[0]);

            Player owner = Main.player[Projectile.owner];

            Projectile.direction = owner.direction;
			Projectile.spriteDirection = -Projectile.direction;
            Projectile.velocity = owner.Center - Projectile.Center;

            AnimeProjectile();

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive()) {
                Filters.Scene.Activate("Shockwave", spawnPosition).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(spawnPosition);
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive()) {
                float progress = Projectile.ai[0]/20f;
                Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress));
            }
            if (Projectile.ai[0] >= 20) {
                Filters.Scene["Shockwave"].Deactivate();
            }
        }
	}
}