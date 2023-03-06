using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using hyperFocus.Items;
using Terraria.GameContent;
using System.Collections.Generic;

namespace hyperFocus.Items.UIstuff {
	internal class blinkEffect : UIState {
		// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
		// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
		private UIText text;
		private UIElement area;
		private UIImage blackFrame;
        private UIImage whiteFrame;
		private Color gradientA;
		private Color gradientB;

		public override void OnInitialize() {
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding.
			area = new UIElement();
			area.Left.Set(0, 0f); // Place the resource bar to the left of the hearts.
			area.Top.Set(0, 0f); // Placing it just a bit below the top of the screen.
			area.Width.Set(0, 1f);
			area.Height.Set(0, 1f);
            area.HAlign = 0.5f;
			area.VAlign = 0.5f;

			blackFrame = new UIImage(ModContent.Request<Texture2D>("hyperFocus/Items/UIstuff/blackFrame")); // Frame of our resource black
			blackFrame.Left.Set(0, 0f);
			blackFrame.Top.Set(0, 0f);
			blackFrame.Width.Set(0, 1f);
			blackFrame.Height.Set(0, 1f);
            blackFrame.ScaleToFit = true;
            blackFrame.HAlign = 0.5f;
			blackFrame.VAlign = 0.5f;

            whiteFrame = new UIImage(ModContent.Request<Texture2D>("hyperFocus/Items/UIstuff/whiteFrame")); // Frame of our resource white
			whiteFrame.Left.Set(0, 0f);
			whiteFrame.Top.Set(0, 0f);
			whiteFrame.Width.Set(0, 1f);
			whiteFrame.Height.Set(0, 1f);
            whiteFrame.ScaleToFit = true;
            whiteFrame.HAlign = 0.5f;
			whiteFrame.VAlign = 0.5f;

			text = new UIText("LE FISHE", 0.8f); // text to show stat
            text.Top.Set(0, 0f);
			text.Left.Set(0, 0f);
			text.Width.Set(0, 0f);
			text.Height.Set(0, 0f);
            text.HAlign = 0.5f;
			text.VAlign = 0.5f;


            //area.Append(whiteFrame);
            //area.Append(blackFrame);
			//area.Append(text);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			// This prevents drawing unless we are using an ExampleCustomResourceWeapon
			if (Main.LocalPlayer.HeldItem.ModItem is not scabbard)
				return;
			base.Draw(spriteBatch);
		}

		// Here we draw our UI
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

		}
        int t = 0;
        public static bool Effect = false;
		bool firstFrame = false;
		bool secondFrame = false;

		public override void Update(GameTime gameTime) {

            

            Vector2 moving = Main.LocalPlayer.velocity;

            text.Top.Set(moving.Y, 0f);
            text.Left.Set(moving.X, 0f);

			if (Main.LocalPlayer.HeldItem.ModItem is not scabbard) {
                //text.Top.Set(-100, 0f);
                return;
            }
            if (Effect) {
				
				t++;

				if (!firstFrame && t == 3) {
					area.Append(blackFrame);
					t = 0;
					firstFrame = true;
					//Main.NewText("1");
				}
				else if (firstFrame && t == 3) {
					Effect = false;
					area.RemoveAllChildren();
					t = 0;
					firstFrame = false;
					secondFrame = false;
					//Main.NewText("2");
				}
            }

            
            base.Update(gameTime);
		}
	}

	class ExampleResourseUISystem : ModSystem {
		private UserInterface ExampleResourceBarUserInterface;

		internal blinkEffect blinkEffect;

		public override void Load() {
			// All code below runs only if we're not loading on a server
			if (!Main.dedServ) {
				blinkEffect = new();
				ExampleResourceBarUserInterface = new();
				ExampleResourceBarUserInterface.SetState(blinkEffect);
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			ExampleResourceBarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"ExampleMod: Example Resource Bar",
					delegate {
						ExampleResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}