﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Commands;
using CS2_GameHUDAPI;

namespace CS2_GameHUDTest
{
	[MinimumApiVersion(330)]
	public class GameHUDTest : BasePlugin
	{
		public override string ModuleName => "GameHUD Test";
		public override string ModuleAuthor => "DarkerZ [RUS]";
		public override string ModuleVersion => "1.DZ.2";

		static IGameHUDAPI? _api;

		public override void OnAllPluginsLoaded(bool hotReload)
		{
			try
			{
				PluginCapability<IGameHUDAPI> CapabilityCP = new("gamehud:api");
				_api = IGameHUDAPI.Capability.Get();
			}
			catch (Exception)
			{
				_api = null;
				PrintToConsole("API Failed!");
			}
		}

		[ConsoleCommand("css_hudtest", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_Show(player, 0, "TestMessage1", 10.0f);
		}

		[ConsoleCommand("css_hudtest2", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest2(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_SetParams(player, 1, new System.Numerics.Vector3(20, 20, 80), System.Drawing.Color.Red);
			_api.Native_GameHUD_Show(player, 1, "TestMessage2", 30.0f);
		}

		[ConsoleCommand("css_hudtest3", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest3(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_Remove(player, 1);
		}

		[ConsoleCommand("css_hudtest4", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest4(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_SetParams(player, 2, new System.Numerics.Vector3(0, 0, 7), System.Drawing.Color.Aqua, 24, "Arial", 0.03f, PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_CENTER, PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_BOTTOM, PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, 0.3f, 0.15f);
			_api.Native_GameHUD_Show(player, 2, "TestMessage3", 10.0f);
		}

		[ConsoleCommand("css_hudtest5", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest5(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_ShowPermanent(player, 2, "TestMessage4");
		}

		[ConsoleCommand("css_hudtest6", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest6(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_UpdateParams(player, 2, new System.Numerics.Vector3(-30, -30, 80), System.Drawing.Color.Indigo, 16, "Verdana", 0.2f, PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_RIGHT, PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, 5.0f, 10.0f);
		}

		[ConsoleCommand("css_hudtest7", "")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTest7(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;
			_api.Native_GameHUD_UpdateParams(player, 2, -6.5f, 2.0f, 7.0f, System.Drawing.Color.Indigo, 32, "Verdana", 0.02f, PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_RIGHT, PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, 5.0f, 10.0f);
		}

		// added command: test getters
		[ConsoleCommand("css_hudgetters", "Test HUD API getters")]
		[CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
		public void OnCommandTestGetters(CCSPlayerController? player, CommandInfo command)
		{
			if (_api == null || player == null || !player.IsValid) return;

			// Example using channel 2
			// var owner = _api.Native_GameHUD_GetOwner(player, 2);
			var target = _api.Native_GameHUD_GetTarget(player, 2);
			var keyValue = _api.Native_GameHUD_GetKeyValue(player, 2, "customkey");

			//PrintToConsole($"[GetOwner] {owner?.ToString() ?? "null"}");
			PrintToConsole($"[GetTarget] {target ?? "null"}");
			PrintToConsole($"[GetKeyValue:customkey] {keyValue ?? "null"}");
		}

		public static void PrintToConsole(string sMessage)
		{
			Console.ForegroundColor = (ConsoleColor)8;
			Console.Write("[");
			Console.ForegroundColor = (ConsoleColor)6;
			Console.Write("GameHUD:TestAPI");
			Console.ForegroundColor = (ConsoleColor)8;
			Console.Write("] ");
			Console.ForegroundColor = (ConsoleColor)13;
			Console.WriteLine(sMessage);
			Console.ResetColor();
		}
	}
}
