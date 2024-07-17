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
		/*var playerGO = PlayerPrefab.Clone();
		playerGO.Name = "Player";
		playerGO.NetworkSpawn( connection );*/
		var obj = PlayerPrefab.Clone();
		var player = obj.Components.Get<PlayerBase>( FindMode.EverythingInSelfAndDescendants );
		obj.NetworkMode = NetworkMode.Object;
		obj.BreakFromPrefab();
		obj.NetworkSpawn( connection );
		player.SetupConnection( connection );
	}
}
	/*System.InvalidOperationException: Sequence contains no matching element
   at System.Linq.ThrowHelper.ThrowNoMatchException()
   at GeneralGame.PlayerBase.GetLocal() in C:\Users\susan\Documents\GitHub\SomeMod\Code\Player\PlayerBase.cs:line 118
   at GeneralGame.UI.InGameHud.BuildRenderTree( RenderTreeBuilder __builder ) in UI/InGameHud/InGameHud.razor:line 11
   at Sandbox.UI.Panel.InternalRenderTree()
*/
