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
	public class blinkAddMarkerProjectile : ModProjectile {

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("invisible hitbox");
		}
        public override void SetDefaults() {

			Projectile.width = 64;
			Projectile.height = 64;
            Projectile.scale = 1f;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }

        SoundStyle deathMarker = new SoundStyle("hyperFocus/Sounds/deathMarker") {
			Volume = 50f
		};

        public override Color? GetAlpha(Color lightColor) {
			return new Color(1f, 1f, 1f, 255) * Projectile.Opacity;
		}
        public override void AI() {

            Projectile.ai[0]++;

            Player owner = Main.player[Projectile.owner];

            if (Projectile.ai[0] >= 50) {
                Projectile.Kill();
            }
            Projectile.direction = owner.direction;
			Projectile.spriteDirection = -Projectile.direction;
            Projectile.velocity = owner.Center - Projectile.Center;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            Player owner = Main.player[Projectile.owner];
            
            if(!target.HasBuff<blinkBuff>()) {

                target.AddBuff(ModContent.BuffType<blinkBuff>(), 60, false);

                //SoundEngine.PlaySound(deathMarker);
                
                Projectile.NewProjectile(Entity.GetSource_FromThis(), target.position, target.velocity, ModContent.ProjectileType<deathMarker>(), 1, 0f, owner.whoAmI);
            }
        
            //Main.NewText("Cock");

            Projectile.penetrate = 2;
        }
        //SoundStyle blinkSound = new SoundStyle("hyperFocus/Sounds/blinkSound") {Volume = 10f};
        public override void OnSpawn(IEntitySource source) {
            //SoundEngine.PlaySound(blinkSound, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor) {
            return false;
        }
	}
}