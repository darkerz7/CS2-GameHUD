﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2_GameHUD
{
	public class HUD
	{
		
		readonly int iSlot;
		public HUDChannel[] Channel;
		public CPointOrient? PointOrient;
		public HUD(int slot)
		{
			iSlot = slot;
			Channel = new HUDChannel[GameHUD.MAXHUDCHANNELS];
			for (int i = 0; i < Channel.Length; i++) Channel[i] = new HUDChannel(iSlot);
		}

		public bool CreateOrGetPointOrient()
		{
			if (PointOrient != null && PointOrient.IsValid) return true;

			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(iSlot);
			if (hudplayer == null || !hudplayer.IsValid) return false;
			var pawn = hudplayer.Pawn.Value!;

			CPointOrient? entOrient = Utilities.CreateEntityByName<CPointOrient>("point_orient");
			if (entOrient == null || !entOrient.IsValid) return false;

			entOrient.Active = true;
			entOrient.GoalDirection = PointOrientGoalDirectionType_t.eEyesForward;
			entOrient.DispatchSpawn();

			System.Numerics.Vector3 vecPos = (System.Numerics.Vector3)pawn.AbsOrigin! with { Z = pawn.AbsOrigin!.Z + pawn.ViewOffset.Z};
			entOrient.Teleport(vecPos, null, null);
			entOrient.AcceptInput("SetParent", pawn, null, "!activator");
			entOrient.AcceptInput("SetTarget", pawn, null, "!activator");
			//entOrient.AcceptInput("SetParentAttachmentMaintainOffset", pawn, null, "look_straight_ahead_stand");

			PointOrient = entOrient;
			return true;
		}
	}

	public class HUDChannel(int slot)
	{
		System.Numerics.Vector3 Position = new(0, 0, 7);
		System.Numerics.Vector3 CurrentPosition = new();
		System.Numerics.Vector3 CurrentAngle = new();

		Vector vecForward = new();
		Vector vecUp = new();
		Vector vecRight = new();
		
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

		// For getter support
		CCSPlayerPawn? LastOwner = null;
		string? LastTarget = null;
		Dictionary<string, string> LastKeyValues = new();

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

		public void Params(System.Numerics.Vector3 vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			Position.X = vec.X;
			Position.Y = vec.Y;
			Position.Z = vec.Z;
			ParamsWithOutVector(color, fontsize, fontname, units, JH, JV, RM, BGBH, BGBW);
		}

		public void Params(float x, float y, float z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			Position.X = x;
			Position.Y = y;
			Position.Z = z;
			ParamsWithOutVector(color, fontsize, fontname, units, JH, JV, RM, BGBH, BGBW);
		}

		public void ParamsWithOutVector(System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
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

		public void UpdateParams(System.Numerics.Vector3 vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			if (!WTIsValid()) return;
			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(PlayerSlot);
			if (hudplayer == null || !hudplayer.IsValid) return;
			if (Position.X != vec.X || Position.Y != vec.Y || Position.Z != vec.Z)
			{
				Position.X = vec.X;
				Position.Y = vec.Y;
				Position.Z = vec.Z;
				if (GameHUD.g_bMethod) GetPosition();
				else GetPosition(hudplayer);
				WorldText!.Teleport(CurrentPosition, CurrentAngle, null);
			}
			UpdateParamsWithOutVector(color, fontsize, fontname, units, JH, JV, RM, BGBH, BGBW);
		}

		public void UpdateParams(float x, float y, float z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
			if (!WTIsValid()) return;
			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(PlayerSlot);
			if (hudplayer == null || !hudplayer.IsValid) return;
			if (Position.X != x || Position.Y != y || Position.Z != z)
			{
				Position.X = x;
				Position.Y = y;
				Position.Z = z;
				if (GameHUD.g_bMethod) GetPosition();
				else GetPosition(hudplayer);
				WorldText!.Teleport(CurrentPosition, CurrentAngle, null);
			}
			UpdateParamsWithOutVector(color, fontsize, fontname, units, JH, JV, RM, BGBH, BGBW);
		}

		public void UpdateParamsWithOutVector(System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t JH = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t JV = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t RM = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float BGBH = 0.0f, float BGBW = 0.0f)
		{
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
			if (WTIsValid()) WorldText!.Remove();
			WorldText = null;

			CCSPlayerController? hudplayer = Utilities.GetPlayerFromSlot(PlayerSlot);
			if (hudplayer == null || !hudplayer.IsValid) return false;
			var pawn = hudplayer.Pawn.Value!;

			if (GameHUD.g_bMethod && !GameHUD.g_HUD[PlayerSlot].CreateOrGetPointOrient()) return false;

			CPointWorldText? entity = Utilities.CreateEntityByName<CPointWorldText>("point_worldtext");
			if (entity == null || !entity.IsValid) return false;

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

			if (GameHUD.g_bMethod)
			{
				entity.AcceptInput("SetParent", GameHUD.g_HUD[PlayerSlot].PointOrient, null, "!activator");
				GetPosition();
				entity.Teleport(CurrentPosition, CurrentAngle, null);
			} else
			{
				entity.AcceptInput("SetParent", pawn, null, "!activator");
				GetPosition(hudplayer);
				entity.Teleport(CurrentPosition, CurrentAngle, null);
			}

				WorldText = entity;
			WorldText.AcceptInput("SetMessage", null, null, Message);

			// Restore last known owner, target, keyvalues if any
			if (LastOwner != null)
				SetOwner(LastOwner);
			if (LastTarget != null)
				SetTarget(LastTarget);
			foreach (var kv in LastKeyValues)
				SetKeyValue(kv.Key, kv.Value);

			//Console.WriteLine($"[Debug:HUD] WorldText: {WorldText.Index} Orient: {GameHUD.g_HUD[PlayerSlot].PointOrient?.Index}");

			return true;
		}

		public void ShowHUD(CCSPlayerController hudplayer)
		{
			if (!WTIsValid() || EmptyMessage()) return;
			GetPosition(hudplayer);
			WorldText!.Teleport(CurrentPosition, CurrentAngle, null);
		}

		public void RemoveHUD()
		{
			CloseTimer();
			Message = "";
			if (WTIsValid()) WorldText!.Remove();
			WorldText = null;

			Position.X = 0;
			Position.Y = 0;
			Position.Z = 7;
			Color = System.Drawing.Color.White;
			FontSize = 18;
			FontName = "Verdana";
			WorldUnitsPerPx = 0.25f;
			JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
			JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
			ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;
			BackgroundBorderHeight = 0.0f;
			BackgroundBorderWidth = 0.0f;

			LastOwner = null;
			LastTarget = null;
			LastKeyValues.Clear();
		}

		public bool EmptyMessage()
		{
			if (string.Equals(Message, "")) return true;
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
			System.Numerics.Vector3 Angle = (System.Numerics.Vector3)hudplayer.Pawn.Value!.V_angle;
			NativeAPI.AngleVectors(hudplayer.Pawn.Value!.V_angle.Handle, vecForward.Handle, vecRight.Handle, vecUp.Handle);
			CurrentPosition = (System.Numerics.Vector3)hudplayer.Pawn.Value!.AbsOrigin!;
			CurrentPosition += (System.Numerics.Vector3)vecForward * Position.Z;
			CurrentPosition += (System.Numerics.Vector3)vecRight * Position.X;
			CurrentPosition += (System.Numerics.Vector3)vecUp * Position.Y;
			CurrentPosition += new System.Numerics.Vector3(0, 0, hudplayer.Pawn.Value!.ViewOffset.Z);

			CurrentAngle.Y = Angle.Y + 270;
			CurrentAngle.Z = 90 - Angle.X;
		}
		void GetPosition()
		{
			System.Numerics.Vector3 Angle = (System.Numerics.Vector3)GameHUD.g_HUD[PlayerSlot].PointOrient!.AbsRotation!;
			
			NativeAPI.AngleVectors(GameHUD.g_HUD[PlayerSlot].PointOrient!.AbsRotation!.Handle, vecForward.Handle, vecRight.Handle, vecUp.Handle);
			CurrentPosition = (System.Numerics.Vector3)GameHUD.g_HUD[PlayerSlot].PointOrient!.AbsOrigin!;
			CurrentPosition += (System.Numerics.Vector3)vecForward * Position.Z;
			CurrentPosition += (System.Numerics.Vector3)vecRight * Position.X;
			CurrentPosition += (System.Numerics.Vector3)vecUp * Position.Y;

			CurrentAngle.Y = Angle.Y + 270;
			CurrentAngle.Z = 90 - Angle.X;
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

		//Setter methods
		public void SetOwner(CCSPlayerPawn owner)
		{
			if (!WTIsValid()) CreateHUD();
			if (WTIsValid())
			{
				WorldText!.AcceptInput("SetOwner", owner);
				LastOwner = owner;
			}
		}

		public void SetKeyValue(string key, string value)
		{
			if (!WTIsValid()) CreateHUD();
			if (WTIsValid())
			{
				//Input format: "KeyValue" <unused> <unused> <arguments>
				WorldText!.AcceptInput("KeyValue", null, null, $"{key} {value}");
				LastKeyValues[key] = value;
			}
		}

		public void SetTarget(string target)
		{
			if (!WTIsValid()) CreateHUD();
			if (WTIsValid())
			{
				WorldText!.AcceptInput("KeyValue", null, null, $"targetname {target}");
				WorldText!.Target = target;
				LastTarget = target;
			}
		}

		//Getter methods
		public CCSPlayerPawn? GetOwner()
		{
			return LastOwner;
		}

		public string? GetKeyValue(string key)
		{
			if (LastKeyValues.TryGetValue(key, out var value))
				return value;
			return null;
		}

		public string? GetTarget()
		{
			return LastTarget;
		}
	}
}
