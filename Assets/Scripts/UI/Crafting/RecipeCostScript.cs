using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class RecipeCostScript : MonoBehaviour
    {
        private PickUpModel _item;
        private int _amount;
        
        public void SetItem(PickUpModel item, int amount)
        {
            _item = item;
            _amount = amount;
            
            var costIcon = transform.Find("CostIcon").GetComponent<Image>();
            costIcon.sprite = _item.icon;
            
            var costAmount = transform.Find("CostAmount").GetComponent<TextMeshProUGUI>();
            costAmount.text = _amount.ToString();
        }
    }
}
