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
using hyperFocus.Effects;
using ReLogic.Utilities;

namespace hyperFocus.Items {
	public class scabbardCyan : ModProjectile {

        public static SlotId musicSlot;
        //SoundStyle rageMusic = new SoundStyle("hyperFocus/Sounds/rageMusic") {
        //    IsLooped = true,
        //    Volume = 0.5f
		//};
        SoundStyle whenTheRageMusicIsAboutToStop = new SoundStyle("hyperFocus/Sounds/whenTheRageMusicIsAboutToStop");

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hyper Focus Blade");
		}
        public override void SetDefaults() {

            //Projectile.aiStyle = 0;
			Projectile.width = 77;
			Projectile.height = 11;
            Projectile.scale = 1.3f;
            Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            //Projectile.manualDirectionChange = true;   
            Projectile.netUpdate = true; 
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor) {
			return new Color(1f, 1f, 1f, 255) * Projectile.Opacity;
		}
        float breathing = 0;
        int i = 0;
        public override void AI() {
            

            //Main.NewText(volume);

            Projectile.ai[0]++;

            Projectile.timeLeft = 2;

            Player owner = Main.player[Projectile.owner];
            Projectile.spriteDirection = owner.direction;

            Projectile.velocity = (owner.Center - Projectile.Center);
            Projectile.rotation = (owner.Center - Main.MouseWorld).ToRotation() + MathHelper.ToRadians(15);
            //Projectile.position = owner.Center + new Vector2(-40f, 6f);

            switch (Projectile.ai[0] / 80 % 2) {
                case 0:
                    i = 0;
                    break;
                case 1:
                    i = 1;
                    break;
            }
            while (i == 0) {
                breathing++;
                break;
            }
            while (i == 1) {
                breathing--;
                break;
            }
            Projectile.Opacity = breathing/80;

            if (playerClass.stacksBreaker < 2) {
                scabbard.cyanKatana = true;
            }
            if (SoundEngine.TryGetActiveSound(musicSlot, out ActiveSound sound)) {
                sound.Position = Projectile.position;
            } 
            if (scabbard.t >= 50 && scabbard.rageCharge < scabbard.chargedCounter) {

                Projectile.Kill();

                if (SoundEngine.TryGetActiveSound(musicSlot, out ActiveSound sound1)) {
                    SoundEngine.PlaySound(whenTheRageMusicIsAboutToStop);
				    sound1.Stop();
			    }
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Top + new Vector2(0, -50), Projectile.velocity, ModContent.ProjectileType<deathMarkerBrokenDark>(), 1, 0f, Main.LocalPlayer.whoAmI);
                
                scabbard.cyanKatana = false;
            }

            Lighting.AddLight(Projectile.Center, 0, 0.8f, 0.8f);

        }
        int x = 0;
        public override bool PreDraw(ref Color lightColor) {

			SpriteEffects spriteEffects = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            if (Projectile.spriteDirection < 0) {
                x = 5;
            }
            else {
                x = -5;
            };

			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

			Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			Vector2 origin = sourceRectangle.Size() / 2f;

			Color drawColor = Projectile.GetAlpha(lightColor);

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(x, 6f),
				sourceRectangle, Color.White * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
        public override void OnSpawn(IEntitySource source) {
            //musicSlot = SoundEngine.PlaySound(rageMusic);
        }
	}
}