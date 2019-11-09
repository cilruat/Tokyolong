using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PnCCasualGameKit
{

    /// <summary>
    /// The base Data class for any shop item.
    /// Consists of the basic properties an item has. Derive from this for adding new properties to the item.
    /// </summary>
    [System.Serializable]
    public class ShopItemData
    {
        ///<summary> unique ID for each item. Don't change ID when you add new items to list, since unlocked and selected item is stored with ID </summary>
        public int ID;

        public bool isLocked;
        public float cost;
        public Sprite thumbnail;
    }


    /// <summary>
    /// The base Implementation of a Shop. This class provides all the basic functionalities of a shop : UI list setup, purchase,  selection of items etc.
    /// How to use:
    /// 1. Extend ItemData for new item properties if required
    /// 2. Extend ShopManger with the extended data class
    /// 3. Override all the necessary methods to implement your shop.
    /// </summary>
    public abstract class ShopManager<T> : MonoBehaviour where T : ShopItemData
    {
        [Tooltip("UI Gameobject parent for shop items")]
        [SerializeField]
        private GameObject container;

        [Tooltip("The UI shop item prefab")]
        [SerializeField]
        private GameObject shopItemPrefab;

        private ShopItem selectedShopItem, selectedShopItemforBuying;

        private float availableCash;
        private List<int> unlockedItemIDs;
        private int selectedItemID;

        /// <summary>
        /// Override this class to pass the default list of items
        /// </summary>
        /// <returns>default item list.</returns>
        protected abstract List<T> GetDefaultList();

        /// <summary>
        /// Override this class to pass the current cash player has
        /// </summary>
        /// <returns>The getcash.</returns>
        protected abstract float Getcash();

        /// <summary>
        /// Override this class to pass the list of unlocked Items
        /// </summary>
        /// <returns> Unlocked Item IDs.</returns>
        protected abstract List<int> GetUnlockedItemIDs();

        /// <summary>
        /// Override this class for getting the last selected item
        /// </summary>
        /// <returns>last selected Item.</returns>
        protected abstract int GetSelectedItemID();


        /// <summary>
        /// Populates the shop list in UI. Sets the unlocked items and the last selected.
        /// </summary>
        public void InitShopList()
        {
            availableCash = Getcash();
            unlockedItemIDs = GetUnlockedItemIDs();
            selectedItemID = GetSelectedItemID();

            //Get the default list typcasted in base data class(ItemData). We don't require the extra properties in the derived data class.
            List<ShopItemData> items = GetDefaultList().ConvertAll(x => (ShopItemData)x);

            //Populating the Shop list UI
            for (int i = 0; i < items.Count; i++)
            {
                GameObject shopItem = (GameObject)Instantiate(shopItemPrefab, container.transform, false);

                ShopItemData itemData = items[i];

                //Set itemData and click listener for every item
                shopItem.GetComponent<ShopItem>().setItem(itemData);
                shopItem.GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    selectItem(shopItem.GetComponent<ShopItem>());
                });

                //If this Item ID exists in unlockedItemIDs list, remove lock
                if (unlockedItemIDs.Exists(x => x == itemData.ID))
                {
                    shopItem.GetComponent<ShopItem>().toggleLock(false);
                }

                //If it is equal to selectedItemID, set it as selected
                if (items[i].ID == selectedItemID)
                {
                    shopItem.GetComponent<ShopItem>().toggleselection(true);
                    selectedShopItem = shopItem.GetComponent<ShopItem>();
                }
            }

            shopItemsInitialised();
        }

        /// <summary>
        /// Override this if anything is required to be done after the shop has initialized
        /// </summary>
        protected virtual void shopItemsInitialised()
        {
        }

        /// <summary>
        /// Gets the available cash since it might have changed outside the shop screen. No need to repopulate the list.
        /// </summary>
        public void RefreshScreen()
        {
            availableCash = Getcash();
        }

        /// <summary>
        /// select an item
        /// </summary>
        /// <param name="shopItem">Shop Item.</param>
        public void selectItem(ShopItem shopItem)
        {
            //if item is not locked
            if (!shopItem.itemData.isLocked)
            {
                //Turn of current items selection
                if (selectedShopItem != null)
                {
                    selectedShopItem.toggleselection(false);
                }
                //set this item to selectedShopItem
                selectedShopItem = shopItem;

                shopItem.toggleselection(true);

                ItemSelected(true, true, (T)selectedShopItem.itemData);

            }
            //else call ItemSelected with necessary data for the derived class to do the required action.
            else
            {
                //set this item to selectedShopItemforBuying
                selectedShopItemforBuying = shopItem;

                if (selectedShopItemforBuying.itemData.cost <= availableCash)
                {
                    ItemSelected(false, true, (T)selectedShopItem.itemData);
                }
                else
                {
                    ItemSelected(false, false, (T)selectedShopItem.itemData);

                }
            }

        }

        /// <summary>
        /// Override this method for implementing actions when an item is selected
        /// </summary>
        /// <param name="isAlreadyPurchased">If <c>true</c> item is already purchased.</param>
        /// <param name="hasSufficientCash">If <c>true</c> player has sufficient cash for purchase.(When if alreadyPurchased is not true)</param>
        /// <param name="item">respective ItemData.</param>
        protected abstract void ItemSelected(bool isAlreadyPurchased, bool hasSufficientCash, T item);

        /// <summary>
        /// Buy an item.
        /// </summary>
        public void BuyItem()
        {
            //if cash is enough for purchase unlock this item, select it adn reduce available cash
            if (selectedShopItemforBuying.itemData.cost <= availableCash)
            {
                selectedShopItemforBuying.toggleLock(false);
                selectItem(selectedShopItemforBuying);
                availableCash -= selectedShopItemforBuying.itemData.cost;
                ItemPurcahsed(true, availableCash, selectedShopItemforBuying.itemData.ID);
                selectedShopItemforBuying = null;
            }
            else
            {
                ItemPurcahsed(false);
            }
        }

        /// <summary>
        /// Override this method to implement any methods after the item is purchased
        /// </summary>
        /// <param name="success">If <c>true</c> the purchase was successful.</param>
        /// <param name="remaingingCash">Remainging cash after purchase.</param>
        /// <param name="newUnlockedId">new unlocked item ID.</param>
        protected abstract void ItemPurcahsed(bool success = true, float remaingingCash = 0, int newUnlockedId = 0);

    }
}