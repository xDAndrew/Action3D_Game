using System;
using Items;
using UnityEngine;

namespace Player
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipParent;
        public Equip currentEquip;
        
        public Guid CurrentEquipGuid { get; set; }
        
        public void Equip(PickUpModel equipPickUp)
        {
            Unequip();
            currentEquip = Instantiate(equipPickUp.equipmentPrefab, equipParent).GetComponent<Equip>();
        }

        public void Unequip()
        {
            if (currentEquip is null)
            {
                return;
            }
        
            Destroy(currentEquip.gameObject);
            currentEquip = null;
        }
    }
}
