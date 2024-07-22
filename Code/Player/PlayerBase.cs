
using System;
using System.Linq;

namespace GeneralGame;


public partial class PlayerBase : Component, Component.INetworkSpawn, IPlayerBase, IHealthComponent
{
	[Property] public GameObject Head { get; set; }
	[Property] public GameObject Body { get; set; }
	[Property] public SkinnedModelRenderer BodyRenderer { get; set; }
	[Property] public ToolGun toolgun { get; set; }
	[Property] public CameraComponent Camera { get; set; }
	[Property] public PanelComponent RootDisplay { get; set; }
    [Property] public Inventory Inventory { get; set; }
	[Property] public Vehicle Vehicle { get; set; }

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
	public CameraMovement cameraMovement;

	protected override void OnAwake()
	{

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

	public void Respawn()
	{
		if ( IsProxy )
			return;


		MaxCarryWeight = Inventory.MAX_WEIGHT_IN_GRAMS;

		Unragdoll();
		Health = MaxHealth;

		MoveToSpawnPoint();
		
	}

	private void MoveToSpawnPoint()
	{
		if ( IsProxy )
			return;
		
		var spawnpoints = Scene.GetAllComponents<SpawnPoint>();
		var randomSpawnpoint = Game.Random.FromList( spawnpoints.ToList() );
		Network.ClearInterpolation();
		Transform.Position = randomSpawnpoint.Transform.Position;
		Transform.Rotation = Rotation.FromYaw( randomSpawnpoint.Transform.Rotation.Yaw() );
		
		EyeAngles = Transform.Rotation;
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

	
	protected override void OnUpdate()
	{


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
