/*
using Sandbox.Citizen;

namespace GeneralGame;

public sealed class Zombie : Component, IHealthComponent
{
	[Property] public SkinnedModelRenderer body { get; set; }
	[Property] public GameObject eye { get; set; }
	[Property] public ModelPhysics ragdol;
	[Property] public CitizenAnimationHelper animationHelper { get; set; }
	[Property] public SoundEvent hitSounds { get; set; }

	[Sync, Property] public float MaxHealth { get; private set; } = 100f;
	[Sync] public LifeState LifeState { get; private set; } = LifeState.Alive;
	[Sync] public float Health { get; private set; } = 100f;
	private TimeUntil? TimeUntilDestroy { get; set; } = null;


	private NavMeshAgent agent;
	private PlayerBase plyObj;

	public TimeSince timeSinceHit = 0;


	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		
		if ( LifeState == LifeState.Dead && TimeUntilDestroy <= 0 )
		{
			GameObject.Destroy();
		}
	}

	protected override void OnAwake()
	{
		
		base.OnAwake();
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
		Transform.Rotation = Rotation.Slerp(Transform.Rotation, targetRot, Time.Delta * 5.0f);
	}


	void NormalTrace()
	{
		var tr = Scene.Trace.Ray(Transform.Position, Transform.Position + Transform.Rotation.Forward * 100).Run();
		
		if (tr.Hit && timeSinceHit > 1.0f && GameObject is not null)
		{
			IHealthComponent damageable;
			damageable = tr.Component.Components.GetInAncestorsOrSelf<IHealthComponent>();

			//damageable.TakeDamage( DamageType.Bullet, 15, tr.EndPosition, tr.Direction * 5, GameObject.Id );
			
			animationHelper.Target.Set("b_attack", true);
			timeSinceHit = 0;

			//Sound.Play( hitSounds, Transform.Position );
		}

	}

	[Broadcast]
	public void TakeDamage( DamageInfo info )
	{
		
		if ( IsProxy || LifeState == LifeState.Dead )
			return;

		if ( Array.Exists( info.Tags, tag => tag == "head" ) )
			info.Damage *= 2;

		Health -= (int)info.Damage;

		
		if ( Health <= 0 )
		{
			LifeState = LifeState.Dead;
			eye.Destroy();
			animationHelper.Destroy();
			agent.Destroy();
			//body.Destroy();

			var colliders = Components.GetAll<Collider>( FindMode.EverythingInSelfAndParent );

			foreach ( var collider in colliders )
			{
				collider.Enabled = false;
			}
			ragdol.Enabled = true;
			//GameObject.Destroy();
			TimeUntilDestroy = 10;
		}
			

		*//*if ( LifeState == LifeState.Dead )
			return;

		if ( type == DamageType.Bullet )
		{
			var p = new SceneParticles( Scene.SceneWorld, "particles/impact.flesh.bloodpuff.vpcf" );
			p.SetControlPoint( 0, position );
			p.SetControlPoint( 0, Rotation.LookAt( force.Normal * -1f ) );
			p.PlayUntilFinished( Task );
		}

		if ( IsProxy )
			return;


		Health = MathF.Max( Health - damage, 0f );

		if ( Health <= 0f )
		{
			
		}
*//*
	}

}
*/
