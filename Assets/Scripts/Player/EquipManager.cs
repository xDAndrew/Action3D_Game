using Items;
using UnityEngine;

namespace Player
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipParent;
        public Equip currentEquip;
        public void Equip(ItemModel equipItem)
        {
            Unequip();
            currentEquip = Instantiate(equipItem.equipmentPrefab, equipParent).GetComponent<Equip>();
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
