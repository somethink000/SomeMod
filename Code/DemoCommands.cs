﻿
using System.Linq;

namespace GeneralGame;

internal class DemoCommands
{
    static OffsetEditor offsetEditor;

    [ConCmd("swb_editor_offsets", Help = "Opens the offsets editor")]
    public static void OpenOffsetsEditor()
    {
        var player = PlayerBase.GetLocal();
        var weapon = player.Inventory.Deployed;

        if (offsetEditor is not null)
        {
            offsetEditor.Delete();
            offsetEditor = null;

            if (weapon.ViewModelHandler is not null)
                weapon.ViewModelHandler.EditorMode = false;

            return;
        }

        if (weapon is not null)
        {
            var screenPanel = player.RootDisplay;
            offsetEditor = new OffsetEditor(weapon);
            screenPanel.Panel.AddChild(offsetEditor);

            if (weapon.ViewModelHandler is not null)
                weapon.ViewModelHandler.EditorMode = true;
        }
    }


  /*  [ConCmd( "kill", Help = "Kills the player" )]
	public static void Kill()
	{
		var player = PlayerBase.GetLocal();
		player?.TakeDamage( DamageInfo.FromBullet( player.GameObject.Id, null, 99999, Vector3.Zero, Vector3.Zero, System.Array.Empty<string>() ) );
	}*/

	[ConCmd( "respawn", Help = "Respawns the player (host only)" )]
	public static void Respawn()
	{
		var player = PlayerBase.GetLocal();
		if ( !player.Network.OwnerConnection.IsHost ) return;
		player?.Respawn();
	}

	[ConCmd( "god", Help = "Toggles godmode (host only)" )]
	public static void GodMode()
	{
		var player = PlayerBase.GetLocal();
		if ( !player.Network.OwnerConnection.IsHost ) return;
		player.GodMode = !player.GodMode;
		Log.Info( (player.GodMode ? "Enabled" : "Disabled") + " Godmode" );
	}

	[ConCmd( "bot", Help = "Adds a bot" )]
	public static void Bot()
	{
		var player = PlayerBase.GetLocal();
		if ( !player.Network.OwnerConnection.IsHost ) return;

		var networkManager = Game.ActiveScene.Components.Get<DemoNetworkManager>( FindMode.EnabledInSelfAndChildren );
		var botGO = networkManager.PlayerPrefab.Clone();
		var botPlayer = botGO.Components.Get<PlayerBase>();

		var players = Game.ActiveScene.GetAllComponents<PlayerBase>();
		var bots = players.Count( ( player ) => player.IsBot );
		var botName = "Bot " + (bots + 1);

		botPlayer.IsBot = true;
		botGO.Name = botName;
		botGO.NetworkSpawn();

		Log.Info( botName + " joined the game!" );
	}
}
