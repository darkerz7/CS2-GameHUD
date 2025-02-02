using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_GameHUDAPI;
using static CounterStrikeSharp.API.Core.Listeners;

namespace CS2_GameHUD
{
	[MinimumApiVersion(285)]
	public class GameHUD : BasePlugin
	{
		public static readonly int MAXHUDCHANNELS = 32;
		public override string ModuleName => "GameHUD";
		public override string ModuleDescription => "Shows text to the player using static point_worldtext";
		public override string ModuleAuthor => "DarkerZ [RUS]";
		public override string ModuleVersion => "0.DZ.1";

		public static HUD[] g_HUD = new HUD[65];
		public static IGameHUDAPI? _api;

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

			RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
			RegisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
			RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawnPost);
			RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeathPost);
			RegisterEventHandler<EventRoundStart>(OnEventRoundStart);
			RegisterListener<CheckTransmit>(OnTransmit);
			RegisterListener<OnTick>(OnOnTick);

			if (hotReload)
			{
				Utilities.GetPlayers().Where(p => p is { IsValid: true, IsBot: false, IsHLTV: false }).ToList().ForEach(player =>
				{
					CreateViewModel(player);
				});
			}
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
			g_HUD[iSlot].ViewModel = null;
			return HookResult.Continue;
		}

		private HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
		{
			CreateViewModel(@event.Userid);

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
			for (int i = 0; i < g_HUD.Length; i++)
			{
				CCSPlayerController? player = Utilities.GetPlayerFromSlot(i);
				if (player != null && player.IsValid && player.Pawn?.Value?.LifeState != (byte)LifeState_t.LIFE_ALIVE)
				{
					for (int j = 0; j < g_HUD[i].Channel.Length; j++) g_HUD[i].Channel[j].ObserverHUD(player);
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

		private static void CreateViewModel(CCSPlayerController? player)
		{
			if (player == null || !player.IsValid) return;
			CCSPlayerPawn pawn = player.PlayerPawn.Value!;
			var handle = new CHandle<CCSGOViewModel>((IntPtr)(pawn.ViewModelServices!.Handle + Schema.GetSchemaOffset("CCSPlayer_ViewModelServices", "m_hViewModel") + 4));
			if (!handle.IsValid)
			{
				CCSGOViewModel viewmodel = Utilities.CreateEntityByName<CCSGOViewModel>("predicted_viewmodel")!;
				handle.Raw = viewmodel.EntityHandle.Raw;
				Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_pViewModelServices");
			}

			g_HUD[player.Slot].ViewModel = handle;
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
