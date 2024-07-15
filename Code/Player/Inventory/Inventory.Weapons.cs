

namespace GeneralGame;

public partial class Inventory
{
	[Property] public GameObject WeaponBone { get; set; }

	public Weapon Deployed; 
	public EquipSlot CurrentWeaponSlot { get; set; } = EquipSlot.FirstWeapon;

	public void DeployCurrent()
	{
		var item = _equippedItems[(int)CurrentWeaponSlot];

		if (item == null) return;



		if (item.GameObject.Components.GetInDescendantsOrSelf<Weapon>(true) != null)
		{
			Weapon nextWeapon = item.GameObject.Components.GetInDescendantsOrSelf<Weapon>(true);
			//Log.Info( nextWeapon );

			//nextWeapon.GameObject.SetParent(WeaponBone);
			//nextWeapon.GameObject.Transform.Position = WeaponBone.Transform.Position;
			//nextWeapon.GameObject.Transform.Rotation = WeaponBone.Transform.Rotation;
			nextWeapon.Owner = Player;
			Deployed = nextWeapon;


			

			nextWeapon.OnCarryStart();

			nextWeapon.GameObject.Enabled = true;
		}

	}

	

	public void UpdateWeaponSlot()
	{
		
		//if ( activeItem is null || !activeItem.CanCarryStop() ) return;
		if ( Input.Pressed( InputButtonHelper.Slot1 ) ) Next();
		else if ( Input.Pressed( InputButtonHelper.Slot2 ) ) Next();
		//else if ( Input.Pressed( InputButtonHelper.Slot3 ) ) SwitchItem( 3 );
		else if ( Input.MouseWheel.y > 0 ) Next();
		else if ( Input.MouseWheel.y < 0 ) Next();
	}

	public void RemoveEquipUpdate( EquipSlot slot, bool drop = false)
	{
		if ( CurrentWeaponSlot == slot ) { 
			Deployed.OnCarryStop();
			Deployed = null;
		}
		
	}
	public void AddEquipUpdate( EquipSlot slot )
	{
		if ( CurrentWeaponSlot == slot ) DeployCurrent();
	}

	
	public void Next()
	{
		Deployed?.OnCarryStop();
		Deployed = null;
		if ( CurrentWeaponSlot == EquipSlot.FirstWeapon )
		{
			CurrentWeaponSlot = EquipSlot.SeccondWeapon;
		}
		else
		{
			CurrentWeaponSlot = EquipSlot.FirstWeapon;
		}

		DeployCurrent();

	}

}
