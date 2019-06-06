using UnityEngine;
using UnityEngine.UI;

namespace PnCCasualGameKit
{
    /// <summary>
    /// Component attached to the skin shop item.
    /// Stores the respective ItemData
    /// Handles updating of the UI item, toggling of selection and unlocking in UI.
    /// </summary>
    public class ShopItem : MonoBehaviour
    {
        [HideInInspector]
        public ShopItemData itemData;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image thumbNailImage;

        [SerializeField]
        private Text costText;

        [SerializeField]
        private GameObject selectedOverlayImg, lockImg;

        /// <summary>
        /// Sets the current item according to the data from storage.
        /// This method is called shopManager while populating the shop list in UI
        /// </summary>
        /// <param name="_skin">item data.</param>
        public void setItem(ShopItemData _skin)
        {
            itemData = _skin;
            thumbNailImage.sprite = _skin.thumbnail;
            costText.text = _skin.cost.ToString();
            toggleLock(_skin.isLocked);
        }

        /// <summary>
        /// Toggles the selection of the item in UI to the specified status.
        /// </summary>
        /// <param name="status">selection status.</param>
        public void toggleselection(bool status)
        {
            if (!itemData.isLocked)
                selectedOverlayImg.SetActive(status);
        }

        /// <summary>
        ///  Toggles the lock of the item in UI to the specified status.
        /// </summary>
        /// <param name="status">item lock status.</param>
        public void toggleLock(bool status)
        {
            itemData.isLocked = status;
            lockImg.SetActive(status);
        }
    }
}