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
using ReLogic.Utilities;
using hyperFocus.Items;


namespace hyperFocus.Systems {
    public class hyperFocusSystem : ModSystem {
        scabbard Scabbard = new scabbard();
        public override void OnWorldLoad() {
            scabbard.cyanKatana = false;
            scabbard.rageCharge = 0;
            playerClass.stacksBreaker = 0;
        }
        public override void OnWorldUnload() {
            if (SoundEngine.TryGetActiveSound(scabbardCyan.musicSlot, out ActiveSound sound2)) {
				    sound2.Stop();
			    }
        }
    }
}