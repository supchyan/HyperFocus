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
using hyperFocus.Items;

namespace hyperFocus.Items {
	public class bladeChargedHit1 : ModProjectile {

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hyper Focus Blade");
            Main.projFrames[Projectile.type] = 5;
		}
        public override void SetDefaults() {

            AnimeProjectile();

            //Projectile.aiStyle = 0;
			Projectile.width = 193;
			Projectile.height = 139;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            //Projectile.penetrate = 2;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }
        public void AnimeProjectile() {

			if (++Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
				// Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
			
		}
        public override Color? GetAlpha(Color lightColor) {
			return new Color(1f, 1f, 1f, 255) * Projectile.Opacity;
		}
        public override void AI() {

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 19) {
                Projectile.Kill();
            }

            Player owner = Main.player[Projectile.owner];
            Projectile.spriteDirection = owner.direction;
            if (Projectile.spriteDirection == -1) {
                Projectile.velocity = owner.Center - Projectile.Center + new Vector2 (-20f, -30f);
            }
            else {
               Projectile.velocity = owner.Center - Projectile.Center + new Vector2 (20f, -30f);
            }

            AnimeProjectile();
            //Main.NewText(Projectile.oldPos.Length);
        }
        SoundStyle onHit = new SoundStyle("hyperFocus/Sounds/onHit");
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Projectile.penetrate = 2;
            Projectile.damage -= 5;
        }
	}
}