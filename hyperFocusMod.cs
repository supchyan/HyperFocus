using Terraria;
using ReLogic.Content;
using hyperFocus;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using hyperFocus.Items;

namespace hyperFocus {
	public class hyperFocus : Mod {
		public override void Load() {
			if (Main.netMode != NetmodeID.Server) {
				Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("hyperFocus/Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value);
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();

				Ref<Effect> filterRef = new Ref<Effect>(ModContent.Request<Effect>("hyperFocus/Effects/FilterMyShader", AssetRequestMode.ImmediateLoad).Value);
				Filters.Scene["FilterMyShader"] = new Filter(new ScreenShaderData(filterRef, "FilterMyShader"), EffectPriority.Medium);
				Filters.Scene["FilterMyShader"].Load();
				
			}
		}
	}
}