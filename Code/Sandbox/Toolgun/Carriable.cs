

using Sandbox.Citizen;

namespace GeneralGame;

public partial class Carriable : Component
{
	/// <summary>Firstperson Model</summary>
	[Property, Group( "Models" )] public Model ViewModel { get; set; }

	/// <summary>Firstperson Hands Model</summary>
	[Property, Group( "Models" )] public Model ViewModelHands { get; set; }

	/// <summary>Thirdperson Model</summary>
	[Property, Group( "Models" )] public Model WorldModel { get; set; }

	[Property, Group( "General" )] public CitizenAnimationHelper.HoldTypes HoldType { get; set; } = CitizenAnimationHelper.HoldTypes.Pistol;
	/// <summary>Default weapon field of view</summary>
	[Property, Group( "FOV" )] public float FOV { get; set; } = 70f;
	/// <summary>Procedural animation speed (lower is slower)</summary>
	[Property, Group( "Vars" )] public float AnimSpeed { get; set; } = 1;

	[Property, Group( "Vars" )] public virtual float PrimaryTime { get; set; } = 1;
	[Property, Group( "Vars" )] public virtual float SeccondaryTime { get; set; } = 1;
	[Property, Group( "Vars" )] public virtual float ReloadTime { get; set; } = 1;

	/// <summary>If the player is running</summary>
	public bool IsRunning => Owner != null && Owner.IsRunning && Owner.IsOnGround && Owner.Velocity.Length >= 200;

	/// <summary>Offset used for setting the weapon to its run position</summary>
	[Property, Group( "Animations" ), Title( "Run Offset (swb_editor_offsets)" )] public AngPos RunAnimData { get; set; }
	public PlayerBase Owner { get; set; }
	public ViewModel ViewModelHandler { get; private set; }
	public SkinnedModelRenderer ViewModelRenderer { get; private set; }
	public SkinnedModelRenderer ViewModelHandsRenderer { get; private set; }
	public SkinnedModelRenderer WorldModelRenderer { get; private set; }

	
	/// <summary>Time since the last primary attack</summary>
	public TimeSince TimeSincePrimaryShoot { get; set; }

	/// <summary>Time since the last secondary attack</summary>
	public TimeSince TimeSinceSecondaryShoot { get; set; }

	/// <summary>Time since the last reload</summary>
	public TimeSince TimeSinceReload { get; set; }


	protected override void OnAwake()
	{
		WorldModelRenderer = Components.GetInDescendantsOrSelf<SkinnedModelRenderer>();

	}


	[Broadcast]
	public virtual void Deploy( PlayerBase player )
	{
		Owner = player;

		GameObject.Enabled = true;

		SetupModels();

		if ( IsProxy ) return;

		
	}

	[Broadcast]
	public virtual void Holster()
	{
		//if ( !CanCarryStop() ) return;
		
		GameObject.Enabled = false;
		
		if ( !IsProxy )
		{
			
			ViewModelHandler.OnHolster();
			WorldModelRenderer.RenderType = ModelRenderer.ShadowRenderType.On;
			ViewModelRenderer.GameObject.Destroy();
			
			ViewModelHandler = null;
			ViewModelRenderer = null;
			ViewModelHandsRenderer = null;
			

		}
		
		Owner = null;

		
	}


	public void OnDeploy()
	{

		// Start drawing
		ViewModelHandler.ShouldDraw = true;

	}


	void SetupModels()
	{

		if ( !IsProxy && ViewModel is not null && ViewModelRenderer is null )
		{

			var viewModelGO = new GameObject( true, "Viewmodel" );
			viewModelGO.SetParent( Owner.GameObject, false );
			viewModelGO.Tags.Add( TagsHelper.ViewModel );
			viewModelGO.Flags |= GameObjectFlags.NotNetworked;

			ViewModelRenderer = viewModelGO.Components.Create<SkinnedModelRenderer>();
			ViewModelRenderer.Model = ViewModel;
			ViewModelRenderer.AnimationGraph = ViewModel.AnimGraph;
			ViewModelRenderer.CreateBoneObjects = true;
			ViewModelRenderer.Enabled = false;
			ViewModelRenderer.OnComponentEnabled += () =>
			{
				// Prevent flickering when enabling the component, this is controlled by the ViewModelHandler
				ViewModelRenderer.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
				OnDeploy();
			};

			ViewModelHandler = viewModelGO.Components.Create<ViewModel>();
			ViewModelHandler.Carriable = this;
			ViewModelHandler.ViewModelRenderer = ViewModelRenderer;
			ViewModelHandler.Camera = Owner.Camera;

			if ( ViewModelHands is not null )
			{
				ViewModelHandsRenderer = viewModelGO.Components.Create<SkinnedModelRenderer>();
				ViewModelHandsRenderer.Model = ViewModelHands;
				ViewModelHandsRenderer.BoneMergeTarget = ViewModelRenderer;
				ViewModelHandsRenderer.OnComponentEnabled += () =>
				{
					// Prevent flickering when enabling the component, this is controlled by the ViewModelHandler
					ViewModelHandsRenderer.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
				};
			}

			ViewModelHandler.ViewModelHandsRenderer = ViewModelHandsRenderer;
		}



		var bodyRenderer = Owner.Body.Components.Get<SkinnedModelRenderer>();
		ModelUtil.ParentToBone( GameObject, bodyRenderer, "hold_R" );
		Network.ClearInterpolation();
	}


	
	
	//First
	public virtual void PrimaryPressed()
	{
		if ( TimeSincePrimaryShoot > PrimaryTime)
		{
			TimeSincePrimaryShoot = 0;
			PrimaryTap(); 
		}
	}
	public virtual void PrimaryTap()
	{

	}
	public virtual void PrimaryUnPressed(){}
	//Seccond
	public virtual void SeccondPressed()
	{
		if ( TimeSinceSecondaryShoot > SeccondaryTime )
		{
			TimeSinceSecondaryShoot = 0;
			SeccondTap();
		}
	}
	public virtual void SeccondTap()
	{

	}
	public virtual void SeccondUnPressed(){}
	//Reload
	public virtual void ReloadPressed()
	{
		if ( TimeSinceReload > PrimaryTime )
		{
			TimeSinceReload = 0;
			ReloadTap();
		}
	}
	public virtual void ReloadTap()
	{

	}
	public virtual void ReloadUnPressed(){}

	protected override void OnUpdate()
	{
		if ( Owner == null ) return;

		Owner.AnimationHelper.HoldType = HoldType;
		UpdateModels();
		



		if ( Input.Down( InputButtonHelper.PrimaryAttack ) ) PrimaryPressed();
		if ( Input.Released( InputButtonHelper.PrimaryAttack ) ) PrimaryUnPressed();

		if (Input.Down( InputButtonHelper.SecondaryAttack )) SeccondPressed();
		if ( Input.Released( InputButtonHelper.SecondaryAttack ) ) SeccondUnPressed();

		if ( Input.Down( InputButtonHelper.Reload ) ) ReloadPressed();
		if ( Input.Released( InputButtonHelper.Reload ) ) ReloadUnPressed();

	}

	void UpdateModels()
	{
		if ( !IsProxy && WorldModelRenderer is not null )
		{
			WorldModelRenderer.RenderType = Owner.IsFirstPerson ? ModelRenderer.ShadowRenderType.ShadowsOnly : ModelRenderer.ShadowRenderType.On;
		}
	}

}
