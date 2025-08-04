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
		public static readonly int MAXHUDCHANNELS = 32;
		public override string ModuleName => "GameHUD";
		public override string ModuleDescription => "Shows text to the player using static point_worldtext";
		public override string ModuleAuthor => "DarkerZ [RUS], Oz_Lin";
		public override string ModuleVersion => "1.DZ.2";

		public static HUD[] g_HUD = new HUD[65];
		static IGameHUDAPI? _api;
		public static bool g_bMethod = false;

		public FakeConVar<bool> Cvar_Method = new("css_gamehud_method", "true - point_orient, false - teleport", false, flags: ConVarFlags.FCVAR_NOTIFY, new RangeValidator<bool>(false, true));
		public override void Load(bool hotReload)
		{
			for (int i = 0; i < g_HUD.Length; i++) g_HUD[i] = new HUD(i);

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

			RegisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
			RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawnPost);
			RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeathPost);
			RegisterEventHandler<EventRoundStart>(OnEventRoundStart);
			RegisterListener<CheckTransmit>(OnTransmit);
			RegisterListener<OnTick>(OnOnTick);
		}

		public override void Unload(bool hotReload)
		{
			DeregisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
			DeregisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawnPost);
			DeregisterEventHandler<EventPlayerDeath>(OnEventPlayerDeathPost);
			DeregisterEventHandler<EventRoundStart>(OnEventRoundStart);
			RemoveListener<CheckTransmit>(OnTransmit);
			RemoveListener<OnTick>(OnOnTick);

			foreach (HUD hud in g_HUD)
			{
				foreach (HUDChannel channel in hud.Channel)
				{
					channel.RemoveHUD();
				}
				hud.PointOrient?.Remove();
				hud.PointOrient = null;
			}
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

		private HookResult OnEventPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
		{
			CCSPlayerController? player = @event.Userid;
			if(player == null || !player.IsValid) return HookResult.Continue;
			int iSlot = player.Slot;
			for (int j = 0; j < g_HUD[iSlot].Channel.Length; j++) g_HUD[iSlot].Channel[j].RemoveHUD();
			
			if (g_HUD[iSlot].PointOrient != null && g_HUD[iSlot].PointOrient!.IsValid) g_HUD[iSlot].PointOrient!.Remove();
			g_HUD[iSlot].PointOrient = null;
			
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
			for (int i = 0; i < g_HUD.Length; i++)
			{
				CCSPlayerController? player = Utilities.GetPlayerFromSlot(i);
				if (player != null && player.IsValid)
				{
					for (int j = 0; j < g_HUD[i].Channel.Length; j++) g_HUD[i].Channel[j].ShowHUD(player);
				}
			}
		}

		void OnTransmit(CCheckTransmitInfoList infoList)
		{
			foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
			{
				if (player == null || !player.IsValid || !player.Pawn.IsValid || player.Pawn.Value == null) continue;

				for (int i = 0; i < g_HUD.Length; i++)
				{
					if(player.Slot != i)
						for (int j = 0; j < g_HUD[i].Channel.Length; j++)
							if(g_HUD[i].Channel[j].WTIsValid()) info.TransmitEntities.Remove(g_HUD[i].Channel[j].WTGetIndex());
				}
			}
		}

		private static void UpdateEvent(CCSPlayerController? player)
		{
			Server.NextFrame(() =>
			{
				if (player != null && player.IsValid)
					for (int j = 0; j < g_HUD[player.Slot].Channel.Length; j++)
						if (!g_HUD[player.Slot].Channel[j].EmptyMessage())
							g_HUD[player.Slot].Channel[j].CreateHUD();
			});
		}

		// --- Getters for HUD API (for direct plugin use, not required for API interface) ---
		public static CCSPlayerPawn? GetHUDOwner(int playerSlot, int channel)
		{
			if (playerSlot < 0 || playerSlot >= g_HUD.Length) return null;
			if (channel < 0 || channel >= MAXHUDCHANNELS) return null;
			return g_HUD[playerSlot].Channel[channel].GetOwner();
		}

		public static string? GetHUDKeyValue(int playerSlot, int channel, string key)
		{
			if (playerSlot < 0 || playerSlot >= g_HUD.Length) return null;
			if (channel < 0 || channel >= MAXHUDCHANNELS) return null;
			return g_HUD[playerSlot].Channel[channel].GetKeyValue(key);
		}

		public static string? GetHUDTarget(int playerSlot, int channel)
		{
			if (playerSlot < 0 || playerSlot >= g_HUD.Length) return null;
			if (channel < 0 || channel >= MAXHUDCHANNELS) return null;
			return g_HUD[playerSlot].Channel[channel].GetTarget();
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
