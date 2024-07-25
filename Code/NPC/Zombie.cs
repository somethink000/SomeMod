

using Sandbox.Citizen;

namespace GeneralGame;

public class Zombie : NPC
{
	[Property] public CitizenAnimationHelper animationHelper { get; set; }
	[Property] public SoundEvent hitSounds { get; set; }
	[Property] public SoundEvent rageSounds { get; set; }
	[Property] public SoundEvent deathSounds { get; set; }
	[Property] public SoundEvent shotedSounds { get; set; }
	[Property] public SoundEvent detectSounds { get; set; }

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		animationHelper.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		animationHelper.MoveStyle = CitizenAnimationHelper.MoveStyles.Run;
		animationHelper.WithWishVelocity( MoveHelper.WishVelocity );
		animationHelper.WithVelocity( MoveHelper.Velocity );
	}


	protected override void BroadcastOnDetect()
	{
		base.BroadcastOnDetect();
		GameObject.PlaySound( detectSounds );
	}

	protected override void BroadcastOnAttack()
	{
		base.BroadcastOnAttack();
		animationHelper.Target.Set( "b_attack", true );
		Sound.Play( hitSounds, Transform.Position );
		GameObject.PlaySound( rageSounds );

		IHealthComponent damageable;
		damageable = TargetObject.Components.GetInAncestorsOrSelf<IHealthComponent>();

		damageable.TakeDamage( DamageType.Bullet, 10, Transform.Position, Transform.Rotation.Forward * 5, GameObject.Id );
	}

	public override void Damaged( GameObject target )
	{
		base.Damaged( target );
		GameObject.PlaySound( shotedSounds );
	}

	protected override void OnDead( GameObject killer )
	{
		GameObject.PlaySound( deathSounds );
	}
}
