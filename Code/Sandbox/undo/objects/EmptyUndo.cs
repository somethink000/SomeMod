

namespace GeneralGame;
public class EmptyUndo : ICanUndo
{
	public void DoUndo() { }

	public bool IsValidUndo() => true;

	/// <summary>
	/// Returns the internal content of the block being undo.
	/// </summary>
	/// <returns>Will return "<see langword="null"/>"</returns>
	public object GetUndoContent() => null;
}
