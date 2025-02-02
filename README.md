# [Core]CS2-GameHUD
API for displaying messages to the player. GameText analogue

## Required packages:
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/) (Min version: 285)

## Installation:
1. Compile or copy CS2-GameHUD to `counterstrikesharp/plugins/CS2-GameHUD` folger
2. Compile or copy CS2-GameHUDAPI to `counterstrikesharp/shared/CS2-GameHUDAPI` folger
3. Restart server

## Example:
### Add the dependency CS2-GameHUDAPI to your project:
```
using CS2_GameHUDAPI;

<My plugin class>: BasePlugin
{
	<...>
	public static IGameHUDAPI? _api;

	public override void OnAllPluginsLoaded(bool hotReload)
	{
		<...>
		try
		{
			PluginCapability<IGameHUDAPI> CapabilityCP = new("gamehud:api");
			_api = IGameHUDAPI.Capability.Get();
		}
		catch (Exception)
		{
			_api = null;
			Console.WriteLine($"[TestSharedPlugin] GameHUD API Loading Failed!");
		}
		<...>
	}
	<...>
}
```

### Using API function. Description of parameters see in source code CS2-GameHUDAPI
```
public void MyExampleFunc(CCSPlayerController? player)
{
	<...>
	if (_api != null && player != null && player.IsValid)
	{
		<...>
		_api.Native_GameHUD_SetParams(player, 0, new CounterStrikeSharp.API.Modules.Utils.Vector(20, 20, 80), System.Drawing.Color.Red);
		_api.Native_GameHUD_Show(player, 0, "MyMessage", 10.0f);
		<...>
		_api.Native_GameHUD_Remove(player, 0);
		<...>
	}
	<...>
}
```