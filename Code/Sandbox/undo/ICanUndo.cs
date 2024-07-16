
namespace GeneralGame;

public interface ICanUndo
{
	/// <summary>
	/// Calls the undo method.
	/// </summary>
	void DoUndo();

	/// <summary>
	/// Checks the validity of the data in the undo block.
	/// </summary>
	/// <returns>Returns <see langword="false"/> if the content of the parameters is violated in the block, otherwise <see langword="true"/>.</returns>
	bool IsValidUndo();

	/// <summary>
	/// Gets an undo content object. Use typecasting to get the class you want, example:
	/// <para>
	///	<br><see langword="object"/> content = item.GetUndoContent();</br>
	///	<br>if ( content is <see langword="Entity"/> ent ) {</br>
	///	<br>/* Any action */</br>
	/// <br>}</br>
	///	</para>
	/// </summary>
	/// <returns>Object with content</returns>
	object GetUndoContent();
}
//Package
