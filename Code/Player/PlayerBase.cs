
using System;
using System.Linq;

namespace GeneralGame;


public partial class PlayerBase : Component, Component.INetworkSpawn, IPlayerBase, IHealthComponent
{
	[Property] public GameObject Head { get; set; }
	[Property] public GameObject Body { get; set; }
	[Property] public SkinnedModelRenderer BodyRenderer { get; set; }
	[Property] public CameraComponent Camera { get; set; }
	[Property] public PanelComponent RootDisplay { get; set; }
    [Property] public Inventory Inventory { get; set; }

	public int MaxCarryWeight { get; set; }
	public bool IsEncumbered => Inventory.Weight > MaxCarryWeight;

	public bool IsFirstPerson => cameraMovement.IsFirstPerson;
	public float InputSensitivity
	{
		get { return cameraMovement.InputSensitivity; }
		set { cameraMovement.InputSensitivity = value; }
	}
	public Angles EyeAnglesOffset
	{
		get { return cameraMovement.EyeAnglesOffset; }
		set { cameraMovement.EyeAnglesOffset = value; }
	}

	Guid IPlayerBase.Id { get => GameObject.Id; }
	CameraMovement cameraMovement;

	protected override void OnAwake()
	{
		
		//Inventory = new Inventory( this );
		cameraMovement = Components.GetInChildren<CameraMovement>();

		OnMovementAwake();
	}

	public void OnNetworkSpawn( Connection connection )
	{
		ApplyClothes( connection );
	}

	protected override void OnStart()
	{
		

		if ( IsProxy )
		{
			if ( Camera is not null )
				Camera.Enabled = false;
		}

		if ( !IsProxy )
		{

			Respawn();
		}


		base.OnStart();
	}

	[Broadcast]
	public virtual void OnDeath( Vector3 force, Vector3 origin )
	{
		

		if ( IsProxy ) return;

		Deaths += 1;
		CharacterController.Velocity = 0;
		Ragdoll( force, origin );
		RespawnWithDelay( 2 );
	}

	public async void RespawnWithDelay( float delay )
	{
		await GameTask.DelaySeconds( delay );
		Respawn();
	}

	public virtual void Respawn()
	{
		//Log.Info( IsProxy );
		MaxCarryWeight = Inventory.MAX_WEIGHT_IN_GRAMS;

		Unragdoll();
		Health = MaxHealth;
		
		var spawnLocation = GetSpawnLocation();
		Transform.Position = spawnLocation.Position;
		EyeAngles = spawnLocation.Rotation.Angles();
		Network.ClearInterpolation();

	}

	public virtual Transform GetSpawnLocation()
	{
		var spawnPoints = Scene.Components.GetAll<SpawnPoint>();

		if ( !spawnPoints.Any() )
			return new Transform();

		var rand = new Random();
		var randomSpawnPoint = spawnPoints.ElementAt( rand.Next( 0, spawnPoints.Count() - 1 ) );

		return randomSpawnPoint.Transform.World;
	}

	
	protected override void OnUpdate()
	{

		//if ( !IsProxy ) ViewModelCamera.Enabled = IsFirstPerson && IsAlive;

		if ( IsAlive ) { 
			OnMovementUpdate();
		}

		HandleFlinch();
		UpdateClothes();
		SpawnMenuUpdate();
	}

	protected override void OnFixedUpdate()
	{
		if ( !IsAlive ) return;
		OnMovementFixedUpdate();

		if (IsProxy)
			return;

		UpdateInteractions();
	}
}
