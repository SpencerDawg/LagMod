using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace LagMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class LagMod : Mod
	{

	}

	public class LagSystem : ModSystem
	{
		int lagspikeTimer;
		bool lagSpike = false;
		bool wasFixed;
		TimeSpan previousCap;

		public override void PostUpdateWorld()
		{
			base.PostUpdateWorld();
			if (lagSpike)
			{
				if (lagspikeTimer == 0)
				{
					lagspikeTimer = Main.rand.Next(1, 100);
				}

				lagspikeTimer--;

				if (lagspikeTimer <= 0)
				{
					lagSpike = false;
				}
			}
			else
			{
				lagSpike = Main.rand.NextBool(100);
				lagspikeTimer = 0;
			}
			int targetFPS = lagSpike ? Main.rand.Next(5, 20) : Main.rand.Next(5, 1000);
			// Set the target frame rate to a random value between 1 and 1000

			Main.instance.IsFixedTimeStep = true;
			Main.instance.TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / targetFPS));
		}

		public override void ModifyScreenPosition()
		{
			base.ModifyScreenPosition();
			if (Main.player[Main.myPlayer].velocity == Vector2.Zero)
			{
				// If the player is not moving, don't apply any lag effect
				return;
			}

			if (lagSpike && Main.rand.NextBool(10))
			{
				// Randomly adjust the screen position to create a lag effect
				// This is just an example, you can modify it to create different effects
				Main.screenPosition += new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
			}
			else if (lagSpike && Main.rand.NextBool(100))
			{
				// Randomly adjust the screen position to create a lag effect
				// This is just an example, you can modify it to create different effects
				{
					Main.screenPosition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
				}
			}
		}

		public override void OnWorldLoad()
		{
			base.OnWorldLoad();
			wasFixed = Main.instance.IsFixedTimeStep;
			previousCap = Main.instance.TargetElapsedTime;
		}

		public override void OnWorldUnload()
		{
			base.OnWorldUnload();
			Main.instance.IsFixedTimeStep = wasFixed;
			Main.instance.TargetElapsedTime = previousCap;
		}
	}
}
