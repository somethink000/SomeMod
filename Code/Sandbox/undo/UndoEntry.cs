
namespace GeneralGame;
public class UndoEntry
{
	/// <summary>
	/// Owner of undo entities.
	/// </summary>
	private PlayerBase undoOwner { get; set; }

	/// <summary>
	/// The name of the block to be undo.
	/// </summary>
	private string undoName { get; set; }

	/// <summary>
	/// List of objects implemented by the interface "ICanUndo" to undo the action.
	/// </summary>
	public List<ICanUndo> objects { get; set; }

	public Action<PlayerBase, string> OnUndo { get; set; }

	/// <summary>
	/// Undo block constructor.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <param name="name">The name for the block to be undo. By default - "Unknown"</param>
	public UndoEntry( PlayerBase owner, string name = null )
	{
		undoOwner = owner;
		undoName = (name != null) ? name : "Unknown";
		objects = new List<ICanUndo>();
	}

	/// <summary>
	/// Undo block constructor.
	/// </summary>
	/// <param name="owner">The entity from which you want to get the owner</param>
	/// <param name="name">The name for the block to be undo. By default - "Unknown"</param>
	public UndoEntry( GameObject owner, string name = null ) : this( owner.Components.GetInAncestorsOrSelf<PlayerBase>(), name ) { }

	/// <summary>
	/// Undo block constructor.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <param name="item">The item to be undo</param>
	/// <param name="name">The name for the block to be undo. By default - the name of the entity or "Unknown"</param>
	public UndoEntry( PlayerBase owner, ICanUndo item, string name = null ) : this( owner, name ) => Add( item );

	/// <summary>
	/// Undo block constructor.
	/// </summary>
	/// <param name="owner">The entity from which you want to get the owner</param>
	/// <param name="item">The item to be undo</param>
	/// <param name="name">The name for the block to be undo. By default - the name of the entity or "Unknown"</param>
	public UndoEntry( GameObject owner, ICanUndo item, string name = "Unknown" ) : this( owner.Components.GetInAncestorsOrSelf<PlayerBase>(), item, name ) { }


	/// <summary>
	/// Add the item to the undo list.
	/// </summary>
	/// <param name="item">Arbitrary item</param>
	/// <returns>Returns <see langword="true"/> if the item was added to the list. Otherwise a <see langword="false"/>.</returns>
	public bool Add( ICanUndo item )
	{
		if ( item == null )
			return false;

		objects.Add( item );
		return true;
	}

	/// <summary>
	/// Removes an item from the list if it exists there.
	/// </summary>
	/// <param name="item">An arbitrary item that exists in the list</param>
	/// <returns>Will return <see langword="true"/> if the item has been deleted.  Otherwise a <see langword="false"/>.</returns>
	public bool Remove( ICanUndo item ) => objects.Remove( item );

	/// <summary>
	/// Clears the list of items.
	/// </summary>
	public void Clear() => objects.Clear();

	/// <summary>
	/// Undo the action and destroy entities from the list, and also raises events "<see langword="OnUndo"/>" and "<see langword="OnFinishUndo"/>".
	/// </summary>
	public void DoUndo()
	{
		foreach ( ICanUndo item in objects )
			if ( item != null && item.IsValidUndo() )
				item.DoUndo();

		OnUndo?.Invoke( undoOwner, undoName );
	}

	/// <summary>
	/// Checks the validity of the data of the undone block and nested elements.
	/// </summary>
	/// <returns>Will return <see langword="true"/> if there is an owner and objects that can be undo. Otherwise it's a <see langword="false"/>.</returns>
	public bool IsValid()
	{
		if ( undoOwner == null ) return false;
		if ( objects.Count == 0 ) return false;

		bool IsExistEntities = false;

		foreach ( ICanUndo item in objects )
			if ( item != null && item.IsValidUndo() )
			{
				IsExistEntities = true;
				break;
			}

		return IsExistEntities;
	}

	/// <summary>
	/// Adds itself to the global undo list.
	/// </summary>
	public void Save() => Undo.AddEntry( this );

	/// <summary>
	/// Returns the owner of the list of objects.
	/// </summary>
	/// <returns>Owner of undo objects</returns>
	public PlayerBase GetUndoOwner() => undoOwner;

	/// <summary>
	/// Returns the name of the block to be undo.
	/// </summary>
	/// <returns>The name of the block to be undo</returns>
	public string GetUndoName() => undoName;
}
