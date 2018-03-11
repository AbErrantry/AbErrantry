using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private StoreMenu storeMenu;

        // Use this for initialization
        private void Start()
        {
            backpackMenu = Player.instance.GetComponent<BackpackMenu>();
            storeMenu = Player.instance.GetComponent<StoreMenu>();
        }

        public void SelectItem()
        {
            backpackMenu.SelectItem(item);
        }

        public void SelectStoreItem()
        {
            storeMenu.SelectItem(item);
        }

    }
}
