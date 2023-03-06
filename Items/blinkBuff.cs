using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using hyperFocus.Items;

namespace hyperFocus.Items {
	public class blinkBuff : ModBuff {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("noed");
			Description.SetDefault("no one escape from death");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(NPC target, ref int buffIndex) {

            var entitySource = target.GetSource_Buff(buffIndex);

		}
	}
}
