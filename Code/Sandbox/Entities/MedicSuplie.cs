

namespace GeneralGame;

public class MedicSuplie : Component
{
	[Property] public ModelRenderer Model { get; set; }
	private int CurPockets { get; set; } = 0;

	public void OnUse()
	{
		if ( CurPockets <= 3 )
		{
			
			CurPockets += 1;
			Model.SetBodyGroup( "pockets", CurPockets );
		}
	}


	protected override void OnStart()
	{
		var interactions = Components.GetOrCreate<Interactions>();
		interactions.AddInteraction( new Interaction()
		{
			Action = ( PlayerBase interactor, GameObject obj ) => OnUse(),
			Keybind = "use",
			Description = "Pickup",
			Disabled = () => CurPockets > 3,
			ShowWhenDisabled = () => true,
			Accessibility = AccessibleFrom.World,
		} );
	}

	
	

}
