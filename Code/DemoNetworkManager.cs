using Sandbox.Network;

namespace GeneralGame;


public class DemoNetworkManager : Component, Component.INetworkListener
{
	[Property] public PrefabScene PlayerPrefab { get; set; }

	protected override void OnStart()
	{
		if ( !GameNetworkSystem.IsActive )
		{
			GameNetworkSystem.CreateLobby();
		}

		base.OnStart();
	}

	// Called on host
	void INetworkListener.OnActive( Connection connection )
	{
		var playerGO = PlayerPrefab.Clone();
		playerGO.Name = "Player";
		playerGO.NetworkSpawn( connection );
	}
}
