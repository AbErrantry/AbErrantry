using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Character2D
{
    public class InventoryPrefabReference : MonoBehaviour
    {
        public InventoryItem item;
        public TMP_Text itemText;
        public Image itemImage;
        public TMP_Text itemQuantity;
        public TMP_Text itemPrice;
        public TMP_Text itemStrength;

        private BackpackMenu backpackMenu;

        // Use this for initialization
        private void Start()
        {
            backpackMenu = GameObject.Find("Knight").GetComponent<BackpackMenu>();
        }

        public void SelectItem()
        {
            backpackMenu.SelectItem(item);
        }
    }
}
