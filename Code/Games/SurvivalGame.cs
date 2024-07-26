

namespace GeneralGame;

public class SurvivalGame : BaseGame
{
	[Property] List<float> Vaves;
	[Property] List<ZombieSpawner> zombieSpawners;

	[Property] public float VaveStartColdown;

	public float ZombieLeft { get; set; }

	private TimeUntil TimeUntilNextVave { get; set; }

	protected override void DrawGizmos()
	{
		const float boxSize = 4f;
		var bounds = new BBox( Vector3.One * -boxSize, Vector3.One * boxSize );

		Gizmo.Hitbox.BBox( bounds );

		Gizmo.Draw.Color = Color.Cyan.WithAlpha( (Gizmo.IsHovered || Gizmo.IsSelected) ? 0.5f : 0.2f );
		Gizmo.Draw.LineBBox( bounds );
		Gizmo.Draw.SolidBox( bounds );

		Gizmo.Draw.Color = Color.Cyan.WithAlpha( (Gizmo.IsHovered || Gizmo.IsSelected) ? 0.8f : 0.6f );
	}


	protected override void OnStart()
	{
		TimeUntilNextVave = VaveStartColdown;
		base.OnStart();
	}


	protected override void OnUpdate() 
	{
		base.OnUpdate();

	}

}
