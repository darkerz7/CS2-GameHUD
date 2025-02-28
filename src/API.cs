using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_GameHUDAPI;
using System.Drawing;

namespace CS2_GameHUD
{
	internal class API : IGameHUDAPI
	{
		public void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, Vector vec, System.Drawing.Color color, int fontsize, string fontname, float units, PointWorldTextJustifyHorizontal_t justifyhorizontal, PointWorldTextJustifyVertical_t justifyvertical, PointWorldTextReorientMode_t reorientmode, float bgborderheight, float bgborderwidth)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].Params(vec, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
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

		public void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, Vector vec, Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25F, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0, float bgborderwidth = 0)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].UpdateParams(vec, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}

		public void Native_GameHUD_ShowPermanent(CCSPlayerController Player, byte channel, string message)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return;
			GameHUD.g_HUD[Player.Slot].Channel[channel].ShowPermanent(message);
		}
	}
}
