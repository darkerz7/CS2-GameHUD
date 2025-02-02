using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;

namespace CS2_GameHUD
{
	public class HUD
	{
		public CHandle<CCSGOViewModel>? ViewModel;
		readonly int iSlot;
		public HUDChannel[] Channel;
		public HUD(int slot)
		{
			iSlot = slot;
			Channel = new HUDChannel[GameHUD.MAXHUDCHANNELS];
			for (int i = 0; i < Channel.Length; i++) Channel[i] = new HUDChannel(iSlot);
		}
	}

	public class HUDChannel(int slot)
	{
		Vector Position = new(0, 0, 50);
		System.Drawing.Color Color = System.Drawing.Color.White;
		int FontSize = 18;
		string FontName = "Verdana";
		float WorldUnitsPerPx = 0.25f;
		PointWorldTextJustifyHorizontal_t JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
		PointWorldTextJustifyVertical_t JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
		PointWorldTextReorientMode_t ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;

		CounterStrikeSharp.API.Modules.Timers.Timer? timer;
		CPointWorldText? WorldText;
		string Message = "";
		readonly int PlayerSlot = slot;

		~HUDChannel()
		{
			RemoveHUD();
		}

		public bool Show(string MessageText, float fTime = 1.0f)
		{
			if (MessageText == null || fTime <= 0.0f) return false;

			CloseTimer();

			if (!WTIsValid()) CreateHUD();
			if (WTIsValid())
			{
				Message = MessageText;
				WorldText!.AcceptInput("SetMessage", null, null, MessageText);
				timer = new CounterStrikeSharp.API.Modules.Timers.Timer(fTime, OnTimer);
			}

			return true;
		}

		public void Params(Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE)
		{
			Position = vec;
			Color = color;
			FontSize = fontsize;
			FontName = fontname;
			WorldUnitsPerPx = units;
			JustifyHorizontal = JH;
			JustifyVertical = JV;
			ReorientMode = RM;
			CreateHUD();
		}

		public bool CreateHUD()
		{
			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(PlayerSlot);
			if (hudplayer == null || !hudplayer.IsValid) return false;
			var pawn = hudplayer.Pawn.Value!;
			CPointWorldText entity = Utilities.CreateEntityByName<CPointWorldText>("point_worldtext")!;
			entity.FontSize = FontSize;
			entity.FontName = FontName;
			entity.Enabled = true;
			entity.Fullbright = true;
			entity.WorldUnitsPerPx = WorldUnitsPerPx;
			entity.Color = Color;
			entity.MessageText = "";
			entity.JustifyHorizontal = JustifyHorizontal;
			entity.JustifyVertical = JustifyVertical;
			entity.ReorientMode = ReorientMode;

			entity.DispatchSpawn();

			if (GameHUD.g_HUD[PlayerSlot].ViewModel != null && GameHUD.g_HUD[PlayerSlot].ViewModel!.IsValid && pawn.LifeState == (byte)LifeState_t.LIFE_ALIVE)
			{
				(Vector, QAngle) pos = GetPosition(hudplayer);
				entity.Teleport(pos.Item1, pos.Item2, null);
				entity.AcceptInput("SetParent", GameHUD.g_HUD[PlayerSlot].ViewModel!.Value, null, "!activator");
			}
			else
			{
				(Vector, QAngle) pos = GetPosition(hudplayer);
				entity.Teleport(pos.Item1, pos.Item2, null);
				entity.AcceptInput("SetParent", pawn, null, "!activator");
			}

			if (WTIsValid()) WorldText!.Remove();

			WorldText = entity;
			WorldText.AcceptInput("SetMessage", null, null, Message);

			return true;
		}

		public void ObserverHUD(CCSPlayerController hudplayer)
		{
			if (!WTIsValid() || EmptyMessage()) return;
			(Vector, QAngle) pos = GetPosition(hudplayer);
			WorldText!.Teleport(pos.Item1, pos.Item2, null);
		}

		public void RemoveHUD()
		{
			CloseTimer();
			Message = "";
			if (WTIsValid()) WorldText!.Remove();
			WorldText = null;

			Position = new(0, 0, 50);
			Color = System.Drawing.Color.White;
			FontSize = 18;
			FontName = "Verdana";
			WorldUnitsPerPx = 0.25f;
			JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
			JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
			ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;
		}

		public bool EmptyMessage()
		{
			if (Message.CompareTo("") == 0) return true;
			return false;
		}

		public bool WTIsValid()
		{
			if (WorldText == null || !WorldText.IsValid) return false;
			return true;
		}

		public uint WTGetIndex()
		{
			return WorldText!.Index;
		}

		(Vector, QAngle) GetPosition(CCSPlayerController hudplayer)
		{
			QAngle eyeAngles = hudplayer.Pawn.Value!.V_angle;
			Vector forward = new(), right = new(), up = new();
			NativeAPI.AngleVectors(eyeAngles.Handle, forward.Handle, right.Handle, up.Handle);

			Vector offset = new();
			offset += forward * Position.Z;
			offset += right * Position.X;
			offset += up * Position.Y;
			QAngle angles = new()
			{
				Y = eyeAngles.Y + 270,
				Z = 90 - eyeAngles.X,
				X = 0
			};
			Vector vec = hudplayer.Pawn.Value!.AbsOrigin! + offset + new Vector(0, 0, hudplayer.Pawn.Value!.ViewOffset.Z);
			return (vec, angles);
		}

		void OnTimer()
		{
			CloseTimer();
			Message = "";
			if (WTIsValid()) WorldText!.AcceptInput("SetMessage", null, null, "");
		}

		void CloseTimer()
		{
			if (timer != null)
			{
				timer.Kill();
				timer = null;
			}
		}
	}
}
