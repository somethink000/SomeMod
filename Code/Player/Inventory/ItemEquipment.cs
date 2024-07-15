using System.Collections.Generic;

namespace GeneralGame;

public enum EquipSlot : byte
{
	FirstWeapon,
	SeccondWeapon
}


public class ItemEquipment : ItemComponent
{


	/*[Property, Category( "Equipment" )] public EquipSlot Slot { get; set; } = EquipSlot.FirstWeapon;*/
	[Property, Category( "Equipment" )] public List<EquipSlot> Slots { get; set; } = new List<EquipSlot>();
	public bool Equipped => State == ItemState.Equipped;

	/*public bool IsWeapon => Slot == EquipSlot.FirstWeapon || Slot == EquipSlot.SeccondWeapon;*/



	public void UpdateEquipped()
	{

		
	}

}
