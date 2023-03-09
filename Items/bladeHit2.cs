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
	public class bladeHit2 : ModProjectile {

        scabbard Scabbard = new scabbard();

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hyper Focus Blade");
            Main.projFrames[Projectile.type] = 5;
		}
        public override void SetDefaults() {

            AnimeProjectile();

            //Projectile.aiStyle = 0;
			Projectile.width = 245;
			Projectile.height = 88;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }
        public void AnimeProjectile() {

			if (++Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
                
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
			
		}
        public override void AI() {

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 19) {
                Projectile.Kill();
            }

            Player owner = Main.player[Projectile.owner];
            Projectile.spriteDirection = owner.direction;
            if (Projectile.spriteDirection == -1) {
                Projectile.velocity = owner.Center - Projectile.Center + new Vector2 (-50f, -20f);
            }
            else {
               Projectile.velocity = owner.Center - Projectile.Center + new Vector2 (50f, -20f);
            }
           
            

            AnimeProjectile();
        }
        SoundStyle onHit = new SoundStyle("hyperFocus/Sounds/onHit");
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            //Projectile.penetrate = 2;
            //Projectile.damage = 0;
            SoundEngine.PlaySound(onHit);
            scabbard.rageCharge += 40;
        }
	}
}