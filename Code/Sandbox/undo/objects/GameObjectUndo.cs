
namespace GeneralGame;

public class GameObjectUndo : ICanUndo
{
	internal GameObject entity { get; set; }

	public GameObjectUndo( GameObject entity ) => this.entity = entity;

	public void DoUndo() => entity.Destroy();

	public bool IsValidUndo() => entity != null && entity.IsValid();

	/// <summary>
	/// Returns the internal content of the block being undo.
	/// </summary>
	/// <returns>Will return "<see langword="Entity"/>"</returns>
	public object GetUndoContent() => entity;
}
