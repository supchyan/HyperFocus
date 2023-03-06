using Terraria;
using ReLogic.Content;
using hyperFocus;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace hyperFocus {
	public class hyperFocus : Mod {
		public override void Load() {
			if (Main.netMode != NetmodeID.Server) {
				Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("hyperFocus/Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value);
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}
		}
	}
}