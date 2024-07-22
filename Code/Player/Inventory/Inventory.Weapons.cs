﻿

namespace GeneralGame;

public partial class Inventory
{
	[Property] public GameObject WeaponBone { get; set; }

	public Weapon Deployed;

	private bool toolgunActive = false;
	public EquipSlot CurrentWeaponSlot { get; set; } = EquipSlot.FirstWeapon;

	public void DeployCurrent()
	{

		var item = _equippedItems[(int)CurrentWeaponSlot];

		if (item == null) return;

		if (item.GameObject.Components.GetInDescendantsOrSelf<Weapon>(true) != null)
		{
			Weapon nextWeapon = item.GameObject.Components.GetInDescendantsOrSelf<Weapon>(true);
			
			
			Deployed = nextWeapon;

			nextWeapon.Deploy( Player );

		}

	}
	
	public void ActivateToolgun()
	{
		if ( toolgunActive ) return;
		Player.toolgun.Deploy( Player );
		toolgunActive = true;
	}
	public void OffToolgun()
	{
		Player.toolgun.Holster();
		toolgunActive = false;
	}


	public void UpdateWeaponSlot()
	{
		
		//if ( activeItem is null || !activeItem.CanCarryStop() ) return;
		if ( Input.Pressed( InputButtonHelper.Slot1 ) ) Next();
		else if ( Input.Pressed( InputButtonHelper.Slot2 ) ) Next();
		else if ( Input.Pressed( InputButtonHelper.Slot3 ) ) ActivateToolgun();
		else if ( Input.MouseWheel.y > 0 ) Next();
		else if ( Input.MouseWheel.y < 0 ) Next();
	}

	public void RemoveEquipUpdate( EquipSlot slot, bool drop = false)
	{
		if ( CurrentWeaponSlot == slot ) { 
			Deployed.Holster();
			Deployed = null;
		}
		
	}
	public void AddEquipUpdate( EquipSlot slot )
	{
		if ( CurrentWeaponSlot == slot ) DeployCurrent();
	}

	
	public void Next()
	{
		if ( toolgunActive ) OffToolgun();
		Deployed?.Holster();
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
