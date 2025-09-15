﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_GameHUDAPI;

namespace CS2_GameHUD
{
	internal class API : IGameHUDAPI
	{
		public void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, Vector vec, System.Drawing.Color color, int fontsize, string fontname, float units, PointWorldTextJustifyHorizontal_t justifyhorizontal, PointWorldTextJustifyVertical_t justifyvertical, PointWorldTextReorientMode_t reorientmode, float bgborderheight, float bgborderwidth)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.Params((System.Numerics.Vector3)vec, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}
		public void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, System.Numerics.Vector3 vec3, System.Drawing.Color color, int fontsize, string fontname, float units, PointWorldTextJustifyHorizontal_t justifyhorizontal, PointWorldTextJustifyVertical_t justifyvertical, PointWorldTextReorientMode_t reorientmode, float bgborderheight, float bgborderwidth)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.Params(vec3, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}
		public void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, float x, float y, float z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25F, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0, float bgborderwidth = 0)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.Params(x, y, z, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}

		public void Native_GameHUD_Show(CCSPlayerController Player, byte channel, string message, float time)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.Show(message, time);
		}

		public void Native_GameHUD_Remove(CCSPlayerController Player, byte channel)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].RemoveChannel(channel);
		}

		public void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25F, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0, float bgborderwidth = 0)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.UpdateParams((System.Numerics.Vector3)vec, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}
		public void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, System.Numerics.Vector3 vec3, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25F, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0, float bgborderwidth = 0)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.UpdateParams(vec3, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}
		public void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, float x, float y, float z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25F, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0, float bgborderwidth = 0)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.UpdateParams(x, y, z, color, fontsize, fontname, units, justifyhorizontal, justifyvertical, reorientmode, bgborderheight, bgborderwidth);
		}

		public void Native_GameHUD_ShowPermanent(CCSPlayerController Player, byte channel, string message)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.ShowPermanent(message);
		}

		// Added example: for setting Owner, KeyValue, Target, etc.
		public void Native_GameHUD_SetOwner(CCSPlayerController Player, byte channel, CCSPlayerPawn owner)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.SetOwner(owner);
		}

		public void Native_GameHUD_SetKeyValue(CCSPlayerController Player, byte channel, string key, string value)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.SetKeyValue(key, value);
		}

		public void Native_GameHUD_SetTarget(CCSPlayerController Player, byte channel, string target)
		{
			if (!Player.IsValid) return;
			GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.SetTarget(target);
		}

		//Getters
		/*public CCSPlayerPawn? Native_GameHUD_GetOwner(CCSPlayerController Player, byte channel)
		{
			if (!Player.IsValid || channel < 0 || channel >= GameHUD.MAXHUDCHANNELS) return null;
			var hud = GameHUD.g_HUD[Player.Slot];
			var hudChannel = hud.Channel[channel];
			return hudChannel.GetOwner();
		}*/

		public string? Native_GameHUD_GetKeyValue(CCSPlayerController Player, byte channel, string key)
		{
			if (!Player.IsValid) return null;
			return GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.GetKeyValue(key);
		}

		public string? Native_GameHUD_GetTarget(CCSPlayerController Player, byte channel)
		{
			if (!Player.IsValid) return null;
			return GameHUD.g_HUD[Player.Slot].CreateorGetChannel(channel)?.GetTarget();
		}
	}
}
