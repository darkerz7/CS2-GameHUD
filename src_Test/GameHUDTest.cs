using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CS2_GameHUDAPI;
using CounterStrikeSharp.API.Core.Capabilities;

namespace CS2_GameHUDTest
{
    public class GameHUDTest : BasePlugin
	{
		public override string ModuleName => "GameHUD Test";
		public override string ModuleAuthor => "DarkerZ [RUS]";
		public override string ModuleVersion => "0.DZ.3";

		public static IGameHUDAPI? _api;

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
			_api.Native_GameHUD_SetParams(player, 1, new CounterStrikeSharp.API.Modules.Utils.Vector(20, 20, 80), System.Drawing.Color.Red);
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
			_api.Native_GameHUD_SetParams(player, 2, new CounterStrikeSharp.API.Modules.Utils.Vector(0, 0, 80), System.Drawing.Color.Aqua, 24, "Arial", 0.28f, PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_CENTER, PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_BOTTOM, PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, 0.3f, 0.15f);
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
			_api.Native_GameHUD_UpdateParams(player, 2, new CounterStrikeSharp.API.Modules.Utils.Vector(-30, -30, 80), System.Drawing.Color.Indigo, 16, "Verdana", 0.2f, PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_RIGHT, PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, 5.0f, 10.0f);
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
