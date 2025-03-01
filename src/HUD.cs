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
		Vector CurrentPosion = new();
		Vector vecForward = new();
		Vector vecUp = new();
		Vector vecRight = new();
		Vector vecOffset = new();
		Vector vecLast = new();
		QAngle CurrentAngle = new();
		System.Drawing.Color Color = System.Drawing.Color.White;
		int FontSize = 18;
		string FontName = "Verdana";
		float WorldUnitsPerPx = 0.25f;
		PointWorldTextJustifyHorizontal_t JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
		PointWorldTextJustifyVertical_t JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
		PointWorldTextReorientMode_t ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;
		float BackgroundBorderHeight = 0.0f;
		float BackgroundBorderWidth = 0.0f;

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

		public bool ShowPermanent(string MessageText)
		{
			if (MessageText == null) return false;

			CloseTimer();

			if (!WTIsValid()) CreateHUD();
			if (WTIsValid())
			{
				Message = MessageText;
				WorldText!.AcceptInput("SetMessage", null, null, MessageText);
			}

			return true;
		}

		public void Params(Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			Position.X = vec.X;
			Position.Y = vec.Y;
			Position.Z = vec.Z;
			Color = color;
			FontSize = fontsize;
			FontName = fontname;
			WorldUnitsPerPx = units;
			JustifyHorizontal = JH;
			JustifyVertical = JV;
			ReorientMode = RM;
			BackgroundBorderHeight = BGBH;
			BackgroundBorderWidth = BGBW;
			CreateHUD();
		}

		public void UpdateParams(Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			if (!WTIsValid()) return;
			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(PlayerSlot);
			if (hudplayer == null || !hudplayer.IsValid) return;
			if (Position.X != vec.X || Position.Y != vec.Y || Position.Z != vec.Z)
			{
				Position.X = vec.X;
				Position.Y = vec.Y;
				Position.Z = vec.Z;
				var pawn = hudplayer.Pawn.Value!;
				GetPosition(hudplayer);
				WorldText!.Teleport(CurrentPosion, CurrentAngle, null);
			}
			if (Color != color)
			{
				Color = color;
				WorldText!.Color = Color;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_Color");
			}
			if (FontSize != fontsize)
			{
				FontSize = fontsize;
				WorldText!.FontSize = FontSize;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_flFontSize");
			}
			if (FontName != fontname)
			{
				FontName = fontname;
				WorldText!.FontName = FontName;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_FontName");
			}
			if (WorldUnitsPerPx != units)
			{
				WorldUnitsPerPx = units;
				WorldText!.WorldUnitsPerPx = WorldUnitsPerPx;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_flWorldUnitsPerPx");
			}
			if (JustifyHorizontal != JH)
			{
				JustifyHorizontal = JH;
				WorldText!.JustifyHorizontal = JustifyHorizontal;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_nJustifyHorizontal");
			}
			if (JustifyVertical != JV)
			{
				JustifyVertical = JV;
				WorldText!.JustifyVertical = JustifyVertical;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_nJustifyVertical");
			}
			if (ReorientMode != RM)
			{
				ReorientMode = RM;
				WorldText!.ReorientMode = ReorientMode;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_nReorientMode");
			}
			if (BackgroundBorderHeight != BGBH)
			{
				BackgroundBorderHeight = BGBH;
				WorldText!.BackgroundBorderHeight = BackgroundBorderHeight;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_flBackgroundBorderHeight");
			}
			if (BackgroundBorderWidth != BGBW)
			{
				BackgroundBorderWidth = BGBW;
				WorldText!.BackgroundBorderWidth = BackgroundBorderWidth;
				Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_flBackgroundBorderWidth");
			}
			WorldText!.DrawBackground = BackgroundBorderHeight != 0.0f || BackgroundBorderWidth != 0.0f;
			Utilities.SetStateChanged(WorldText, "CPointWorldText", "m_bDrawBackground");
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

			if (BackgroundBorderHeight != 0.0f || BackgroundBorderWidth != 0.0f)
			{
				entity.DrawBackground = true;
				entity.BackgroundBorderHeight = BackgroundBorderHeight;
				entity.BackgroundBorderWidth = BackgroundBorderWidth;
			}

			entity.DispatchSpawn();

			GetPosition(hudplayer);
			entity.Teleport(CurrentPosion, CurrentAngle, null);
			if (GameHUD.g_HUD[PlayerSlot].ViewModel != null && GameHUD.g_HUD[PlayerSlot].ViewModel!.IsValid && pawn.LifeState == (byte)LifeState_t.LIFE_ALIVE)
			{
				entity.AcceptInput("SetParent", GameHUD.g_HUD[PlayerSlot].ViewModel!.Value, null, "!activator");
			}
			else
			{
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
			GetPosition(hudplayer);
			WorldText!.Teleport(CurrentPosion, CurrentAngle, null);
		}

		public void RemoveHUD()
		{
			CloseTimer();
			Message = "";
			if (WTIsValid()) WorldText!.Remove();
			WorldText = null;

			Position.X = 0;
			Position.Y = 0;
			Position.Z = 50;
			Color = System.Drawing.Color.White;
			FontSize = 18;
			FontName = "Verdana";
			WorldUnitsPerPx = 0.25f;
			JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
			JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
			ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;
			BackgroundBorderHeight = 0.0f;
			BackgroundBorderWidth = 0.0f;
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

		void GetPosition(CCSPlayerController hudplayer)
		{
			NativeAPI.AngleVectors(hudplayer.Pawn.Value!.V_angle.Handle, vecForward.Handle, vecRight.Handle, vecUp.Handle);

			vecOffset = vecForward * Position.Z;
			vecOffset += vecRight * Position.X;
			vecOffset += vecUp * Position.Y;
			CurrentAngle.Y = hudplayer.Pawn.Value!.V_angle.Y + 270;
			CurrentAngle.Z = 90 - hudplayer.Pawn.Value!.V_angle.X;
			vecLast.Z = hudplayer.Pawn.Value!.ViewOffset.Z;
			CurrentPosion = hudplayer.Pawn.Value!.AbsOrigin! + vecOffset + vecLast;
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
