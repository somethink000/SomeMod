
using Sandbox;

namespace GeneralGame;

public enum SpawnMenuCategories
{
	Props,
	Entities
}

public partial class PlayerBase
{
	[Property] public GameObject PhysicsPrefab { get; set; }

	
	
	public SpawnMenuCategories ActiveSpawnCategory { get; set; } = SpawnMenuCategories.Entities;

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


			obj.Transform.Position = tr.EndPosition + Vector3.Down * -5;//obj.Mins.z
			obj.Transform.Rotation = modelRotation;
	
			obj.NetworkMode = NetworkMode.Object;
			obj.NetworkSpawn();


			Undo.AddEntity( this, obj, obj.Name );
		}
	}

	public void SpawnMenuPackage( Package package )
	{

		var tr = Scene.Trace.Ray( Scene.Camera.Transform.Position, Scene.Camera.Transform.Position + Scene.Camera.Transform.Rotation.Forward * 1000f )
		   .WithoutTags( "player", "trigger" )
		   .Run();

		SpawnCloudModelAsync( package.FullIdent, tr.Hit ? tr.HitPosition : tr.EndPosition );

	}

	public async void SpawnCloudModelAsync( string ident, Vector3 pos )
	{

		var package = await Package.FetchAsync( ident, false );
		await package.MountAsync();
		Log.Info( package.Thumb );


		var mins = package.GetMeta( "RenderMins", Vector3.Zero );
		var maxs = package.GetMeta( "RenderMaxs", Vector3.Zero );

		var spawnPos = Transform.World;
		spawnPos.Position = pos + (mins + maxs) * 0.5f;
		var physicsObj = PhysicsPrefab.Clone( spawnPos, name: ident );
		physicsObj.NetworkMode = NetworkMode.Object;
		physicsObj.NetworkSpawn();


		var model = Model.Load( package.GetMeta( "PrimaryAsset", "" ) );
		var modelRenderer = physicsObj.Components.Get<ModelRenderer>();
		if ( modelRenderer is not null )
			modelRenderer.Model = model;
		var modelCollider = physicsObj.Components.Get<ModelCollider>();
		if ( modelCollider is not null )
			modelCollider.Model = model;
		var rb = physicsObj.Components.Get<Rigidbody>();
		if ( rb is not null )
			rb.Velocity = Vector3.Zero;

		Undo.AddEntity( this, physicsObj, physicsObj.Name );
	}


	public void SpawnMenuUpdate()
	{
		if ( Input.Pressed( InputButtonHelper.Undo ) )
		{
			Undo.DoUndo( this, false );
		}
		
	}

}
