
using static Sandbox.Clothing;

namespace GeneralGame;


public class SpawnMenuComponent : Component
{
	/// <summary>
	/// The name of the item.
	/// </summary>
	[Sync]
	[Property]
	public string Name { get; set; }

	/// <summary>
	/// The icon to display.
	/// </summary>
	[Property] public IconSettings Icon { get; set; }

	/// <summary>
	/// The description of the item.
	/// </summary>
	[Property] public string Description { get; set; }
	[Sync] public string Prefab { get; private set; }

	public Texture IconTexture => Texture.Load( FileSystem.Mounted, Icon.Path );

	public static implicit operator SpawnMenuComponent( GameObject obj )
		=> obj.Components.Get<SpawnMenuComponent>();

}
