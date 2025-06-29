using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

//VersionAPI: 0.DZ.5

namespace CS2_GameHUDAPI
{
	public interface IGameHUDAPI
	{
		public static PluginCapability<IGameHUDAPI> Capability { get; } = new("gamehud:api");

		/**
		 * Initializes hud with the specified parameters
		 *
		 * @param Player				CCSPlayerController for whom the hud will be initialized
		 * @param channel				Channel number to initialize
		 * @param vec					Vector where the hud will be located relative to the player
		 * @param color					Color of hud
		 * @param fontsize				Hud font size
		 * @param fontname				Hud font name
		 * @param units					Hud world units per px
		 * @param justifyhorizontal		Horizontal alignment of hud
		 * @param justifyvertical		Vertical alignment of hud
		 * @param reorientmode			Reorient mode for hud
		 * @param bgborderheight		Background border height if needed (to disable both must be equal to 0.0f)
		 * @param bgborderwidth			Background border width if needed
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, CounterStrikeSharp.API.Modules.Utils.Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0.0f, float bgborderwidth = 0.0f);

		/**
		 * Initializes hud with the specified parameters
		 *
		 * @param Player				CCSPlayerController for whom the hud will be initialized
		 * @param channel				Channel number to initialize
		 * @param X						Horizontal position
		 * @param Y						Vertical position
		 * @param Z						Distancing from the player (Recomended 7.0f)
		 * @param color					Color of hud
		 * @param fontsize				Hud font size
		 * @param fontname				Hud font name
		 * @param units					Hud world units per px
		 * @param justifyhorizontal		Horizontal alignment of hud
		 * @param justifyvertical		Vertical alignment of hud
		 * @param reorientmode			Reorient mode for hud
		 * @param bgborderheight		Background border height if needed (to disable both must be equal to 0.0f)
		 * @param bgborderwidth			Background border width if needed
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_SetParams(CCSPlayerController Player, byte channel, float X, float Y, float Z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0.0f, float bgborderwidth = 0.0f);

		/**
		 * Initializes hud with the specified parameters
		 *
		 * @param Player				CCSPlayerController for whom the hud will be initialized
		 * @param channel				Channel number to initialize
		 * @param vec					Vector where the hud will be located relative to the player
		 * @param color					Color of hud
		 * @param fontsize				Hud font size
		 * @param fontname				Hud font name
		 * @param units					Hud world units per px
		 * @param justifyhorizontal		Horizontal alignment of hud
		 * @param justifyvertical		Vertical alignment of hud
		 * @param reorientmode			Reorient mode for hud
		 * @param bgborderheight		Background border height if needed (to disable both must be equal to 0.0f)
		 * @param bgborderwidth			Background border width if needed
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, CounterStrikeSharp.API.Modules.Utils.Vector vec, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0.0f, float bgborderwidth = 0.0f);

		/**
		 * Initializes hud with the specified parameters
		 *
		 * @param Player				CCSPlayerController for whom the hud will be initialized
		 * @param channel				Channel number to initialize
		 * @param X						Horizontal position
		 * @param Y						Vertical position
		 * @param Z						Distancing from the player (Recomended 7.0f)
		 * @param color					Color of hud
		 * @param fontsize				Hud font size
		 * @param fontname				Hud font name
		 * @param units					Hud world units per px
		 * @param justifyhorizontal		Horizontal alignment of hud
		 * @param justifyvertical		Vertical alignment of hud
		 * @param reorientmode			Reorient mode for hud
		 * @param bgborderheight		Background border height if needed (to disable both must be equal to 0.0f)
		 * @param bgborderwidth			Background border width if needed
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_UpdateParams(CCSPlayerController Player, byte channel, float X, float Y, float Z, System.Drawing.Color color, int fontsize = 18, string fontname = "Verdana", float units = 0.25f, PointWorldTextJustifyHorizontal_t justifyhorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT, PointWorldTextJustifyVertical_t justifyvertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP, PointWorldTextReorientMode_t reorientmode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE, float bgborderheight = 0.0f, float bgborderwidth = 0.0f);

		/**
		 * Shows a message to the player
		 *
		 * @param Player				CCSPlayerController for whom the message is displayed
		 * @param channel				Channel number to display
		 * @param message				Message to display
		 * @param time					Show time
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_Show(CCSPlayerController Player, byte channel, string message, float time = 1.0f);

		/**
		 * Shows a message to the player, until another command is used
		 *
		 * @param Player				CCSPlayerController for whom the message is displayed
		 * @param channel				Channel number to display
		 * @param message				Message to display
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_ShowPermanent(CCSPlayerController Player, byte channel, string message);

		/**
		 * Deletes the displayed channel
		 *
		 * @param Player				CCSPlayerController for whom the channel is being deleted
		 * @param channel				Channel number to remove
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_Remove(CCSPlayerController Player, byte channel);

		/**
		 * Sets the owner of the HUD entity for a desired channel.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel			Channel number where the owner needs to be set
		 * @param owner				The pawn to be assigned as the owner of the HUD entity
		 *
		 *
		 * On error/errors:			Invalid player, Invalid channel
		 */
		void Native_GameHUD_SetOwner(CCSPlayerController Player, byte channel, CCSPlayerPawn owner);

		/**
		 * Sets a key-value pair on the HUD entity, typically used for further customization.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel				Channel number on which the key-value pair is set
		 * @param key					The key to be set
		 * @param value					The value to assign to the key
		 *
		 *
		 * On error/errors:			Invalid player, Invalid channel
		 */
		void Native_GameHUD_SetKeyValue(CCSPlayerController Player, byte channel, string key, string value);

		/**
		 * Assigns a target name to the HUD entity, which can be retrieved by other scripts or services.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel				Channel number whose target should be updated
		 * @param target				The new target name for the HUD entity
		 *
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_SetTarget(CCSPlayerController Player, byte channel, string target);

		/**
		 * Gets the owner of the HUD entity for a desired channel.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel				Channel number to query
		 *
		 * @return						The pawn assigned as the owner, or null if not set
		 */
	   // CCSPlayerPawn? Native_GameHUD_GetOwner(CCSPlayerController Player, byte channel);

		/**
		 * Gets a key-value pair from the HUD entity.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel				Channel number to query
		 * @param key					The key to retrieve
		 *
		 * @return						The value assigned to the key, or null if not set
		 */
		string? Native_GameHUD_GetKeyValue(CCSPlayerController Player, byte channel, string key);

		/**
		 * Gets the target name of the HUD entity.
		 *
		 * @param Player				CCSPlayerController for whom this request is made
		 * @param channel			   Channel number to query
		 *
		 * @return					  The target name, or null if not set
		 */
		string? Native_GameHUD_GetTarget(CCSPlayerController Player, byte channel);
	}
}
