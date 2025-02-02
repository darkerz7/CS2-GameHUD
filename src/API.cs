using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_GameHUDAPI;

namespace CS2_GameHUD
{
	internal class API : IGameHUDAPI
	{
		public void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, Vector vec, System.Drawing.Color color, int fontsize, string fontname, float units, PointWorldTextJustifyHorizontal_t justifyhorizontal, PointWorldTextJustifyVertical_t justifyvertical, PointWorldTextReorientMode_t reorientmode)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].Params(vec, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode);
		}

		public void Native_GameHUD_Show(CCSPlayerController Player, byte channel, string message, float time)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].Show(message, time);
		}

		public void Native_GameHUD_Remove(CCSPlayerController Player, byte channel)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].RemoveHUD();
		}
	}
}
