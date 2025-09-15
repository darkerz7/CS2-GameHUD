using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Cvars.Validators;
using CS2_GameHUDAPI;
using static CounterStrikeSharp.API.Core.Listeners;

namespace CS2_GameHUD
{
	[MinimumApiVersion(330)]
	public class GameHUD : BasePlugin
	{
		public override string ModuleName => "GameHUD";
		public override string ModuleDescription => "Shows text to the player using static point_worldtext";
		public override string ModuleAuthor => "DarkerZ [RUS], Oz_Lin";
		public override string ModuleVersion => "1.DZ.3.1";

		public static HUD[] g_HUD = new HUD[65];
		static IGameHUDAPI? _api;
		public static bool g_bMethod = false;

		public FakeConVar<bool> Cvar_Method = new("css_gamehud_method", "true - point_orient, false - teleport", false, flags: ConVarFlags.FCVAR_NOTIFY, new RangeValidator<bool>(false, true));
		public override void Load(bool hotReload)
		{
			for (int i = 0; i < g_HUD.Length; i++) g_HUD[i] = new HUD();

			try
			{
				_api = new API();
				Capabilities.RegisterPluginCapability(IGameHUDAPI.Capability, () => _api);
			}
			catch (Exception)
			{
				_api = null;
				PrintToConsole("API Failed!");
			}

			g_bMethod = Cvar_Method.Value;
			Cvar_Method.ValueChanged += (sender, value) =>
			{
				g_bMethod = value;
				PrintToConsole($"Cvar 'css_hud_method' has been changed to '{value}'");
			};

			RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
			RegisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
			RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawnPost);
			RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeathPost);
			RegisterEventHandler<EventRoundStart>(OnEventRoundStart);
			RegisterListener<CheckTransmit>(OnTransmit);
			RegisterListener<OnTick>(OnOnTick);
		}

		public override void Unload(bool hotReload)
		{
			DeregisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
			DeregisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
			DeregisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawnPost);
			DeregisterEventHandler<EventPlayerDeath>(OnEventPlayerDeathPost);
			DeregisterEventHandler<EventRoundStart>(OnEventRoundStart);
			RemoveListener<CheckTransmit>(OnTransmit);
			RemoveListener<OnTick>(OnOnTick);

			foreach (HUD hud in g_HUD)
			{
				hud.RemoveAllHUD();
				hud.RemovePointOrient();
			}
		}

		private HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
		{
			CCSPlayerController? player = @event.Userid;
			if (player == null || !player.IsValid) return HookResult.Continue;
			g_HUD[player.Slot].SetHUDPlayer(player);

			return HookResult.Continue;
		}

		private HookResult OnEventPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
		{
			CCSPlayerController? player = @event.Userid;
			if (player == null || !player.IsValid) return HookResult.Continue;
			g_HUD[player.Slot].RemoveAllHUD();
			g_HUD[player.Slot].RemovePointOrient();
			g_HUD[player.Slot].SetHUDPlayer(null);

			return HookResult.Continue;
		}

		[GameEventHandler(mode: HookMode.Post)]
		private HookResult OnEventPlayerSpawnPost(EventPlayerSpawn @event, GameEventInfo info)
		{
			CCSPlayerController? player = @event.Userid;
			UpdateEvent(player);
			return HookResult.Continue;
		}

		[GameEventHandler(mode: HookMode.Post)]
		private HookResult OnEventPlayerDeathPost(EventPlayerDeath @event, GameEventInfo info)
		{
			CCSPlayerController? player = @event.Userid;
			UpdateEvent(player);
			return HookResult.Continue;
		}

		private HookResult OnEventRoundStart(EventRoundStart @event, GameEventInfo info)
		{
			Utilities.GetPlayers().Where(p => p is { IsValid: true, IsBot: false, IsHLTV: false }).ToList().ForEach(player =>
			{
				_ = new CounterStrikeSharp.API.Modules.Timers.Timer(1.0f, () => UpdateEvent(player));
			});
			return HookResult.Continue;
		}

		private void OnOnTick()
		{
			if (g_bMethod) return;
			var t = new Task(() =>
			{
				Parallel.ForEach(g_HUD, (hud) => hud.ShowAllHUD());
			});
			t.Start();
		}

		void OnTransmit(CCheckTransmitInfoList infoList)
		{
			foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
			{
				if (player == null || !player.IsValid || !player.Pawn.IsValid || player.Pawn.Value == null) continue;

				for (int i = 0; i < g_HUD.Length; i++)
				{
					if (player.Slot != i)
						foreach (var channel in g_HUD[i].Channel)
							if (channel.Value.WTIsValid()) info.TransmitEntities.Remove(channel.Value.WTGetIndex());
				}
			}
		}

		private static void UpdateEvent(CCSPlayerController? player)
		{
			var t = new Task(() =>
			{
				if (player != null && player.IsValid)
					Parallel.ForEach(g_HUD[player.Slot].Channel, (pair) => {
						if (!pair.Value.EmptyMessage())
						{
							Server.NextFrame(() =>
							{
								pair.Value.CreateHUD();
							});
						}
					});
						
			});
			t.Start();
		}

		// --- Getters for HUD API (for direct plugin use, not required for API interface) ---
		public static CCSPlayerPawn? GetHUDOwner(int playerSlot, byte channel)
		{
			if (playerSlot >= 0 && playerSlot < g_HUD.Length && g_HUD[playerSlot].Channel.TryGetValue(channel, out HUDChannel? hudchannel)) return hudchannel.GetOwner();
			return null;
		}

		public static string? GetHUDKeyValue(int playerSlot, byte channel, string key)
		{
			if (playerSlot >= 0 && playerSlot<g_HUD.Length && g_HUD[playerSlot].Channel.TryGetValue(channel, out HUDChannel? hudchannel)) return hudchannel.GetKeyValue(key);
			return null;
		}

		public static string? GetHUDTarget(int playerSlot, byte channel)
		{
			if (playerSlot >= 0 && playerSlot < g_HUD.Length && g_HUD[playerSlot].Channel.TryGetValue(channel, out HUDChannel? hudchannel)) return hudchannel.GetTarget();
			return null;
		}

		public static void PrintToConsole(string sMessage)
		{
			Console.ForegroundColor = (ConsoleColor)8;
			Console.Write("[");
			Console.ForegroundColor = (ConsoleColor)6;
			Console.Write("GameHUD");
			Console.ForegroundColor = (ConsoleColor)8;
			Console.Write("] ");
			Console.ForegroundColor = (ConsoleColor)13;
			Console.WriteLine(sMessage);
			Console.ResetColor();
		}
	}
}
