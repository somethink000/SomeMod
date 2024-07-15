using Sandbox;
using System.Diagnostics;

namespace GeneralGame;

public partial class PlayerBase
{

	public void SpawnEntity( string path )
	{
		//Log.Info("ede");
		if (PrefabLibrary.TryGetByPath( path, out var prefabFile ) )
		{
			var obj = SceneUtility.GetPrefabScene( prefabFile.Prefab ).Clone();

			var tr = Scene.Trace.Ray( EyePos, EyePos + Camera.Transform.Rotation.Forward * 500 )
				.IgnoreGameObjectHierarchy( GameObject.Root )
				.WithoutTags( "world" )
				.Run();

			var modelRotation = Rotation.From( new Angles( 0, EyeAngles.yaw, 0 ) ) * Rotation.FromAxis( Vector3.Up, 180 );


			obj.Transform.Position = tr.EndPosition + Vector3.Down * -10; //* model.PhysicsBounds.Mins.z
			obj.Transform.Rotation = modelRotation;
	
			obj.NetworkMode = NetworkMode.Object;
			obj.NetworkSpawn();
			
		}
	}

}
