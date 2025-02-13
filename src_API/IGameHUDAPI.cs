using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

//VersionAPI: 0.DZ.2

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
		 * Deletes the displayed channel
		 *
		 * @param Player				CCSPlayerController for whom the channel is being deleted
		 * @param channel				Channel number to remove
		 * 
		 *
		 * On error/errors:				Invalid player, Invalid channel
		 */
		void Native_GameHUD_Remove(CCSPlayerController Player, byte channel);
	}
}
