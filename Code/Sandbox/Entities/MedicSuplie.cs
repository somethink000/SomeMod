

namespace GeneralGame;

public class MedicSuplie : Component
{
	[Property] public ModelRenderer Model { get; set; }
	private int CurPockets { get; set; } = 0;

	public void OnUse( PlayerBase ply )
	{
		if ( CurPockets <= 3 )
		{
			ply.Health = ply.MaxHealth;

			CurPockets += 1;
			Model.SetBodyGroup( "pockets", CurPockets );
		}
	}


	protected override void OnStart()
	{
		var interactions = Components.GetOrCreate<Interactions>();
		interactions.AddInteraction( new Interaction()
		{
			Action = ( PlayerBase interactor, GameObject obj ) => OnUse( interactor ),
			Keybind = "use",
			Description = "Pickup",
			Disabled = () => CurPockets > 3,
			ShowWhenDisabled = () => true,
			Accessibility = AccessibleFrom.World,
		} );
	}




}
