using System;
using System.Linq;
using Sandbox;
using Sandbox.Citizen;
using Sandbox.UI;

namespace GeneralGame;

public sealed class Zombie : Component, IHealthComponent
{
	[Property] public GameObject body { get; set; }
	[Property] public GameObject eye { get; set; }
	[Property] public CitizenAnimationHelper animationHelper { get; set; }
	[Property] public SoundEvent hitSounds { get; set; }
	[Property] public GameObject ZombieRagedol { get; set; }

	[Sync, Property] public float MaxHealth { get; private set; } = 100f;
	[Sync] public LifeState LifeState { get; private set; } = LifeState.Alive;
	[Sync] public float Health { get; private set; } = 100f;


	private NavMeshAgent agent;
	private PlayerBase plyObj;
	public TimeSince timeSinceHit = 0;

	protected override void OnAwake()
	{
		agent = Components.Get<NavMeshAgent>();

		
			plyObj = Scene.GetAllComponents<PlayerBase>().FirstOrDefault();
		
	}
	protected override void OnUpdate()
	{
		plyObj = Scene.GetAllComponents<PlayerBase>().FirstOrDefault();
		if ( plyObj == null || LifeState == LifeState.Dead ) return;

		animationHelper.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		animationHelper.MoveStyle = CitizenAnimationHelper.MoveStyles.Run;
		var target = plyObj.Transform.Position;
		
		
		UpdateAnimtions();

		if (Vector3.DistanceBetween(target, GameObject.Transform.Position ) < 100f)
		{
			agent.Stop();
			NormalTrace();
		}
		else
		{
			agent.MoveTo(plyObj.Transform.Position);
		}
	}
	

	void UpdateAnimtions()
	{
		animationHelper.WithWishVelocity(agent.WishVelocity);
		animationHelper.WithVelocity(agent.Velocity);
		var targetRot = Rotation.LookAt(plyObj.GameObject.Transform.Position.WithZ(Transform.Position.z) - body.Transform.Position);
		body.Transform.Rotation = Rotation.Slerp(body.Transform.Rotation, targetRot, Time.Delta * 5.0f);
	}
	void NormalTrace()
	{
		var tr = Scene.Trace.Ray(body.Transform.Position, body.Transform.Position + body.Transform.Rotation.Forward * 100).Run();

		if (tr.Hit && timeSinceHit > 1.0f && GameObject is not null)
		{
			
			IHealthComponent damageable;
			damageable = tr.Component.Components.GetInAncestorsOrSelf<IHealthComponent>();

			damageable.TakeDamage( DamageType.Bullet, 15, tr.EndPosition, tr.Direction * 5, GameObject.Id );

			animationHelper.Target.Set( "b_attack", true );
			timeSinceHit = 0;

			//Sound.Play( hitSounds, Transform.Position );
		}

	}

	[Broadcast]
	public void TakeDamage( DamageType type, float damage, Vector3 position, Vector3 force, Guid attackerId )
	{

		if ( IsProxy || LifeState == LifeState.Dead )
			return;

		

		Health -= damage;


		if ( Health <= 0 )
		{
			LifeState = LifeState.Dead;
			var zombie = ZombieRagedol.Clone( this.GameObject.Transform.Position, this.GameObject.Transform.Rotation );
			zombie.NetworkSpawn();
			GameObject.Destroy();
		}
	}

}
