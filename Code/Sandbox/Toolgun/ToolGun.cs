

namespace GeneralGame;

public class ToolGun : Carriable
{
	[Property] public BaseTool CurrentTool { get; set; }

	protected override void OnAwake()
	{
		base.OnAwake();

		//GlobalGameNamespace.TypeLibrary.GetType<BaseTool>( PhysGun );
		//CurrentTool = Components.Create( new , true );
		CurrentTool.parentToolgun = this;
	}

	public void SetTool(string name )
	{
		
		CurrentTool.OnClear();
		CurrentTool.Destroy();
		CurrentTool = GlobalGameNamespace.TypeLibrary.Create<BaseTool>( name );
		CurrentTool.parentToolgun = this;
	}

	public override void Holster()
	{
		
		base.Holster();

		CurrentTool.Enabled = false;


	}

	public override void Deploy( PlayerBase player )
	{
		base.Deploy( player );
		CurrentTool.Enabled = true;

	}

	//First
	public override void PrimaryPressed()
	{
		base.PrimaryPressed();
		CurrentTool.OnPrimaryPressed();
	}
	public override void PrimaryTap()
	{
		CurrentTool.OnPrimaryTap();
	}
	public override void PrimaryUnPressed() 
	{
		CurrentTool.OnPrimaryUnPressed();
	}
	//Seccond
	public override void SeccondPressed()
	{
		base.SeccondPressed();
		CurrentTool.OnSeccondPressed();
	}
	public override void SeccondTap()
	{
		CurrentTool.OnSeccondTap();
	}
	public override void SeccondUnPressed() 
	{
		CurrentTool.OnSeccondUnPressed();
	}
	//Reload
	public override void ReloadPressed()
	{
		base.ReloadPressed();
		CurrentTool.OnReloadPressed();
	}
	public override void ReloadTap()
	{
		CurrentTool.OnReloadTap();
	}
	public override void ReloadUnPressed() 
	{
		CurrentTool.OnReloadUnPressed();
	}
}



public partial class BaseTool : Component
{
	public ToolGun parentToolgun;

	public PlayerBase Owner => parentToolgun.Owner;

	public virtual void OnClear() {}

	//First
	public virtual void OnPrimaryPressed() { }
	public virtual void OnPrimaryTap() { }
	public virtual void OnPrimaryUnPressed() { }

	//Seccond
	public virtual void OnSeccondPressed() { }
	public virtual void OnSeccondTap() { }
	public virtual void OnSeccondUnPressed() { }

	//Reload
	public virtual void OnReloadPressed() { }
	public virtual void OnReloadTap() { }
	public virtual void OnReloadUnPressed() { }

}
