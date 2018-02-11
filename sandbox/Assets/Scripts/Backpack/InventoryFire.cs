using TMPro;
using UnityEngine;

namespace Character2D
{
    public class InventoryFire : MonoBehaviour
    {
        private BackpackMenu backpackMenu;

        // Use this for initialization
        void Start()
        {
            backpackMenu = GameObject.Find("Knight").GetComponent<BackpackMenu>();
        }

        public void SelectItem(InventoryItem item)
        {
            backpackMenu.SelectItem(item);
        }
    }
}
