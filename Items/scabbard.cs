using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.ModLoader;
using System.Collections.Generic;
using hyperFocus.Items.UIstuff;
using ReLogic.Utilities;

namespace hyperFocus.Items {
	public class scabbard : ModItem {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hyper Focus");
			Tooltip.SetDefault("This weapon has two different forms of damage. The first one is activated since you picked up the weapon." + 
			"\nYou can charge your weapon while holding it with no getting any damage or you can hit enemies to get the [c/00ffcb:Spirit Form]." + 
			"\n[c/00ffcb:Spirit Form]: Weapon does more damage while in [c/00ffcb:Spirit Form]. Getting damage resets the [c/00ffcb:Spirit Form]." +
			"\nRight click when in [c/00ffcb:Spirit Form] to do a dash through the enemies by splitting the space." +
			"\nEnemies after getting a hit receives a [c/c12120:Death Mark]. This mark will hit an enemies until it breaks." +
			"\nWhen [c/c12120:Death Mark] breaks, it explodes and does additional damage to owner. Also, it charges the [c/00ffcb:Spirit Form]." +
			"\nIf [c/c12120:Death Mark] kills an enemy, it replenishes [c/00ffcb:Spirit Form]." +
			"\n[c/00cca2:So, you call this true melee, right?]");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			foreach (TooltipLine line2 in tooltips) {
				if (line2.Mod == "Terraria" && line2.Name == "ItemName" ) {
					line2.Text = "[c/ff004e:Hyper] [c/00ffcb:Focus]";
					//line2.OverrideColor = Main.DiscoColor;
				}
			}
		}
		int scabbardFrames = 6;
		public override void SetDefaults() {

			Item.damage = 128;
			Item.DamageType = DamageClass.Melee;
			Item.width = 77;
			Item.height = 11;
			Item.scale = 1.3f;
			Item.useTime = 20;
			Item.noMelee = true;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.crit = 33;
			Item.rare = 10;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Grenade;
			Item.value = Item.sellPrice(gold: 5);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(int.MaxValue, scabbardFrames));
		}

		
		int inventoryAnimationFrames = 0;
		int invTimerAnimation = 0;
		public void AnimeInventoryItem() {

			if (++invTimerAnimation >= 12) {
				invTimerAnimation = 0;
                
				if (++inventoryAnimationFrames >= 12)
					inventoryAnimationFrames = 0;
			}
			
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {

			Texture2D texture = ModContent.Request<Texture2D>("hyperFocus/Items/hyperFocus").Value;
			int frameHeight = texture.Height / 12;
			int startY = frameHeight * inventoryAnimationFrames;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin1 = sourceRectangle.Size() / 2f;
			spriteBatch.Draw(texture, position + new Vector2(15, 2), sourceRectangle, drawColor, 0, origin1, scale * 2, SpriteEffects.None, 0f);
			return false;
		}

		SoundStyle blinkSound = new SoundStyle("hyperFocus/Sounds/blinkSound") {
			Volume = 1f,
			Pitch = 2f,
		};
		SoundStyle rageIsReady = new SoundStyle("hyperFocus/Sounds/rageIsReady") {
			Volume = 1f
		};
		public static SoundStyle rageIsLost = new SoundStyle("hyperFocus/Sounds/rageIsLost") {
			Volume = 1f
		};
		SoundStyle bladeHit1 = new SoundStyle("hyperFocus/Sounds/bladeHit1") {
			MaxInstances = 4,
			Volume = 1f
		};
		SoundStyle bladeHit2 = new SoundStyle("hyperFocus/Sounds/bladeHit2") {
			MaxInstances = 4,
			Volume = 1f
		};
		SoundStyle bladeChargedHit1 = new SoundStyle("hyperFocus/Sounds/bladeChargedHit1") {
			MaxInstances = 4,
			Volume = 1f
		};
		SoundStyle bladeChargedHit2 = new SoundStyle("hyperFocus/Sounds/bladeChargedHit2") {
			MaxInstances = 4,
			Volume = 1f
		};
		SoundStyle endOfBlink = new SoundStyle("hyperFocus/Sounds/endOfBlink") {
			Volume = 1f
		};
		public override Vector2? HoldoutOffset() {
			return new Vector2(-40f,6f);
		}

		public static int t = 0;

		public static int rageCharge = 0;
		public static int chargedCounter = 3600; //this paramter says, how many hits or time player need to do before charged attacks and ult capability
		int dashCoolDown = 0;
		public static int blinkDelay = 0;
		int sparksTime = 0;
		int shockWaveTimer = 90;
		bool stopTrigger = true;
		bool blinkTrigger = false;
		bool rageIsLostSoundTrigger = true;
		bool rageIsReadySoundTrigger = true;
		public static bool cyanKatana = false;
		bool deathMarkerBrokenTrigger = false;
		bool endOfBlinkSoundTrigger = false;
		 
		public override void UpdateInventory(Player player) {

		}
		public override bool CanUseItem(Player player) {
			if (dashCoolDown <= 0) {
				return true;
			}
			else {
				return false;
			}
		}
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		public override void HoldItem(Player player) {

			playerClass.stacksBreaker = 0;
						
			t++;
			invTimerAnimation++;
			rageCharge++;

			shockWaveTimer--;
			blinkDelay--;
			sparksTime--;
			dashCoolDown--;

			AnimeInventoryItem();

			//playerClass greatPlayer = player.GetModPlayer<playerClass>();
			//Main.NewText(t);

			if (inventoryAnimationFrames >= 12) {
				inventoryAnimationFrames = 0;
			}

			if (dashCoolDown < 0) {
				Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(int.MaxValue, scabbardFrames));
				endOfBlinkSoundTrigger = true;
			}
			if (dashCoolDown == 0 && endOfBlinkSoundTrigger) {
				SoundEngine.PlaySound(endOfBlink, player.position);
				Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<shockWaveProjectile>(), 0, 0, player.whoAmI);
				endOfBlinkSoundTrigger = false;
			}

			if (blinkDelay == 0 && blinkTrigger) {

				t = 0;

				sparksTime = 5;
				blinkTrigger = false;
				stopTrigger = false;
				rageCharge = 0;
				deathMarkerBrokenTrigger = true;

				Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<blackAndWhiteProjectile>(), 0, 0, player.whoAmI);
				Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<shockWaveProjectile>(), 0, 0, player.whoAmI);
				Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<blinkAddMarkerProjectile>(), 1, 0, player.whoAmI);
				//Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position + new Vector2(-40f, 6f), player.velocity, ModContent.ProjectileType<scabbardGlowMask>(), Item.damage, default, player.whoAmI);

				player.velocity = player.DirectionTo(Main.MouseWorld) * 75f;

				SoundEngine.PlaySound(blinkSound, player.position);
				
			}
			while (t <= 60) {
				player.immune = true;
				break;
			}
			

			while (t > 5 && !stopTrigger) {
				player.velocity = Vector2.Zero;
				stopTrigger = true;
				break;
			}
			//player.immune = false;
			
			while (sparksTime > 0) {
				var blinkDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Clentaminator_Red, 10f*player.direction, 1f*player.direction, 255, Color.White, 1f);
				break;
			}
			
			while (rageCharge >= chargedCounter) {
				//var rageDust = Dust.NewDustDirect(player.Center, 0, 0, DustID.Clentaminator_Red, 0f, 0f, 255, default, 0.5f);
				if (playerClass.stacksBreaker < 2 && !cyanKatana) {
					Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position + new Vector2(-40f, 6f), player.velocity, ModContent.ProjectileType<scabbardCyan>(), Item.damage, default, player.whoAmI);
					cyanKatana = true;
					//Main.NewText("КАТАНААА");
				}
				player.eocDash = int.MaxValue;
				break;
			}
			if (rageCharge == chargedCounter && !rageIsReadySoundTrigger) {
				SoundEngine.PlaySound(rageIsReady, player.position);
				rageIsReadySoundTrigger = true;
			}
			else if (rageCharge != chargedCounter) {
				rageIsReadySoundTrigger = false;
			}
			
			if (playerClass.stacksBreaker > 1) { //when player removes weapon, stacks are going out

				rageCharge = 0;
			}
		}
		public override void Update(ref float gravity, ref float maxFallSpeed) {
			Main.LocalPlayer.eocDash = 0;
			rageCharge = 0;
			playerClass.stacksBreaker = 2;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {		

			//Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<deathMarker>(), 1, 0, player.whoAmI);	

			blinkTrigger = true;

			if (player.altFunctionUse == 2 && dashCoolDown <= 0 && rageCharge >= chargedCounter) {

				blinkDelay = 15;
				dashCoolDown = 60;

				Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(blinkDelay, scabbardFrames));

			}
			else if (player.altFunctionUse == 0) {
				
				switch (Main.GameUpdateCount / 30 % 2) {
					case 0:
						if (rageCharge < chargedCounter) {
							Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<bladeHit1>(), damage + 30, knockback, player.whoAmI);
							SoundEngine.PlaySound(bladeHit1, player.position);
							SoundEngine.PlaySound(SoundID.Item15, player.position);
						}
						else {
							Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<bladeChargedHit1>(), damage, knockback, player.whoAmI);
							SoundEngine.PlaySound(bladeChargedHit1, player.position);
							//SoundEngine.PlaySound(SoundID.NPCDeath39, player.position);
						}
						break;
					case 1:
						if (rageCharge < chargedCounter) {
							Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<bladeHit2>(), damage, knockback, player.whoAmI);
							SoundEngine.PlaySound(bladeHit2, player.position);
							SoundEngine.PlaySound(SoundID.Item15 with {Volume = 2f}, player.position);
						}
						else {
							Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<bladeChargedHit2>(), damage, knockback, player.whoAmI);
							SoundEngine.PlaySound(bladeChargedHit2, player.position);
							//SoundEngine.PlaySound(SoundID.NPCDeath39, player.position);
						}
						break;
				}
				
			}
			return false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.WarriorEmblem)
				.AddIngredient(ItemID.DemonHeart)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class playerClass : ModPlayer {

		//scabbard Scabbard = new scabbard();

		public static int stacksBreaker = 0;
		public override void PostUpdate() {
			stacksBreaker += 1;
			//Main.NewText(stacksBreaker);
			if (stacksBreaker >= 2) {
				scabbard.rageCharge = 0;
				
			}
			if (scabbard.rageCharge == 0) {
				Main.LocalPlayer.eocDash = 0;
			}
		}
		public override void Hurt (bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter) {
			
			scabbard.rageIsLost = new SoundStyle("hyperFocus/Sounds/rageIsLost") {
			Volume = 1f
			};
			SoundEngine.PlaySound(scabbard.rageIsLost, Main.LocalPlayer.position);
			scabbard.rageCharge = 0;
		}
	}
}