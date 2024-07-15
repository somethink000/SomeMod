
namespace GeneralGame;
public class Undo
{
	/// <summary>
	/// The list of objects to be canceled, implemented through the interface - <see langword="ICanUndo"/>.
	/// </summary>
	public static Dictionary<PlayerBase, List<UndoEntry>> UndoItems = new Dictionary<PlayerBase, List<UndoEntry>>();

	/// <summary>
	/// Delegate to add methods to the event "CanUndo".
	/// </summary>
	/// <param name="entry">Item undo object</param>
	/// <returns>Return <see langword="false"/> to stop undo, otherwise return <see langword="true"/>.</returns>

	public delegate bool UndoHandler( UndoEntry entry );

	/// <summary>
	/// Event for the ability to interrupt the "undo" under certain conditions.
	/// </summary>

	public static event UndoHandler CanUndo;

	/// <summary>
	/// Returns a list of canceled objects for a single client.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <returns>Returns a <see langword="List"/> or <see langword="null"/></returns>
	public static List<UndoEntry> GetClientUndoList( PlayerBase owner )
	{
		if ( UndoItems.ContainsKey( owner ) ) return UndoItems[owner];
		return null;
	}

	/// <summary>
	/// Adds an item to the undo list.
	/// </summary>
	/// <param name="item">Item undo object</param>
	/// <returns>Returns <see langword="true"/> if added successfully. Otherwise a <see langword="false"/>.</returns>
	public static bool AddEntry( UndoEntry item )
	{

		if ( !item.IsValid() )
			return false;

		PlayerBase owner = item.GetUndoOwner();

		if ( !UndoItems.ContainsKey( owner ) )
			UndoItems[owner] = new List<UndoEntry>();

		UndoItems[owner].Add( item );

		return true;
	}

	/// <summary>
	/// Adds an item to the undo list.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <param name="undoItem">Object with implementation of the undo interface</param>
	/// <param name="name">Name of the undo action</param>
	/// <returns>Returns <see langword="true"/> if added successfully. Otherwise a <see langword="false"/>.</returns>
	public static bool Add( PlayerBase owner, ICanUndo undoItem, string name = null ) => AddEntry( new UndoEntry( owner, undoItem, name ) );

	/// <summary>
	/// Adds an item to the undo list.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <param name="undoItem">Object with implementation of the undo interface</param>
	/// <param name="name">Name of the undo action</param>
	/// <returns>Returns <see langword="true"/> if added successfully. Otherwise a <see langword="false"/>.</returns>
	public static bool Add( GameObject owner, ICanUndo undoItem, string name = null ) => Add( owner.Components.GetInAncestorsOrSelf<PlayerBase>(), undoItem, name );

	/// <summary>
	/// Adds an entity to the undo list.
	/// </summary>
	/// <param name="owner">Client owner</param>
	/// <param name="ent">Any entity</param>
	/// <param name="name">Name of the undo action</param>
	/// <returns>Returns <see langword="true"/> if added successfully. Otherwise a <see langword="false"/>.</returns>
	public static bool AddEntity( PlayerBase owner, GameObject ent, string name = null )
	{
		var item = new UndoEntry( owner, new GameObjectUndo( ent ), name );
		return AddEntry( item );
	}

	/// <summary>
	/// Adds an entity to the undo list.
	/// </summary>
	/// <param name="owner">The entity from which you want to get the owner</param>
	/// <param name="ent">Any entity</param>
	/// <param name="name">Name of the undo action</param>
	/// <returns>Returns <see langword="true"/> if added successfully. Otherwise a <see langword="false"/>.</returns>
	public static bool AddEntity( GameObject owner, GameObject ent, string name = null ) => AddEntity( owner.Components.GetInAncestorsOrSelf<PlayerBase>(), ent, name );

	/// <summary>
	/// Undoes an action on the index of the list.
	/// </summary>
	/// <param name="owner">The client that undo the action</param>
	/// <param name="index">The index of the item in the undo list</param>
	public static void DoUndo( PlayerBase owner, int index )
	{
		

		if ( index == -1 || owner == null || !UndoItems.ContainsKey( owner ) ) return;

		List<UndoEntry> items = UndoItems[owner];
		if ( items == null ) return;

		UndoEntry entry = items[index];
		if ( entry == null || CanUndo?.Invoke( entry ) == false ) return;

		bool isValid = entry.IsValid();
		int undoCount = 0;

		if ( isValid )
		{
			undoCount = entry.objects.Count;
			
		}

		entry.DoUndo();

		items.RemoveAt( index );

		if ( isValid )
		{
			PlayerBase undoOwner = entry.GetUndoOwner();
			string ownerNick = undoOwner.Network.OwnerConnection.DisplayName;
			string undoName = entry.GetUndoName();

			Log.Info( $"Player { ownerNick } undo the { undoName }" );
			
		}
	}

	/// <summary>
	/// Undoes the last action for a client.
	/// </summary>
	/// <param name="owner">The client that undo the action</param>
	/// <param name="all">Set <see langword="true"/> if you want to cancel all player actions. The default is <see langword="false"/></param>
	public static void DoUndo( PlayerBase owner, bool all = false )
	{

		if ( owner == null || !UndoItems.ContainsKey( owner ) ) return;

		List<UndoEntry> items = UndoItems[owner];

		if ( items != null )
			for ( int i = items.Count - 1; i >= 0; i-- )
			{
				UndoEntry entry = items[i];
				if ( entry == null ) continue;
				
				bool isValid = entry.IsValid();

				DoUndo( owner, i );
				
				if ( !all && isValid ) break;
			}
	}

	/// <summary>
	/// Clears the undo list.
	/// </summary>
	public static void ClearItems() => UndoItems.Clear();
}
