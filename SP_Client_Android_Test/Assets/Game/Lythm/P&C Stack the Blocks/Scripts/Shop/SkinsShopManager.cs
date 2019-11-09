using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using PnCCasualGameKit;

namespace PnCCasualGameKit
{
/// <summary>
/// Derives from the ShopMangager which has all the shop functionalities.
/// Overrides the required methods.
/// Responsibe for:
/// - Populating skin shop
/// - handling selection, purcahase and unlocking of skin items.
/// </summary>
public class SkinsShopManager : ShopManager<SkinItemData>
{

    [Tooltip("Default List of skins. This is a Scriptable object asset")]
    [SerializeField]
    private SkinList skinList;


    /// <summary>
    /// Opens the skins screen. Call RefreshScreen which gets the current cash. Not calling InitShopList instead, since the UI is not requried to be populated again 
    /// </summary>
    public void OpenSkinsScreen()
    {
        SoundManager.Instance.playSound(AudioClips.UI);
        RefreshScreen();
        UIManager.Instance.cashText.text = Getcash().ToString();
        UIManager.Instance.OpenScreen(UIScreensList.SkinsShop);
    }

    /// <summary>
    /// Get Current cash the player has
    /// </summary>
    /// <returns>player cash.</returns>
    protected override float Getcash()
    {
        return PlayerData.Instance.cash;
    }

    /// <summary>
    /// Gets the unlocked skins Items from player data
    /// </summary>
    /// <returns>unlocked skin items.</returns>
    protected override List<int> GetUnlockedItemIDs()
    {
       return PlayerData.Instance.unlockedSkinItems;
    }

    /// <summary>
    /// Gets the selected item.
    /// </summary>
    /// <returns>selected item ID.</returns>
    protected override int GetSelectedItemID()
    {
        return PlayerData.Instance.selectedSkinID;
    }

    /// <summary>
    /// Initialise the shop list at start and pass the selected skin ID to GameplayController
    /// </summary>
    private void Start()
    {
        InitShopList();
        GameplayController.Instance.UpdateSkin(skinList.skins.Find(x => x.ID == GetSelectedItemID()).texture);
    }

    /// <summary>
    /// Gets the default list. Instantiate the scriptable object skinList. Or else the default values in the SO asset gets changed with any update in shop.
    /// </summary>
    /// <returns>The default list.</returns>
    protected override List<SkinItemData> GetDefaultList()
    {
        return Instantiate(skinList).skins;
    }

    /// <summary>
    /// Overriding this method for implementing actions after a skin is selected.
    /// </summary>
    /// <param name="isAlreadyPurchased">If <c>true</c> Skin is already purchased.</param>
    /// <param name="hasSufficientCash">If <c>true</c> player has sufficient cash for purchase.</param>
    /// <param name="item">respective SkinItemData.</param>
    protected override void ItemSelected(bool isAlreadyPurchased, bool hasSufficientCash, SkinItemData item)
    {
        SoundManager.Instance.playSound(AudioClips.UI);
        //Select the skin if already purchased
        if (isAlreadyPurchased)
        {
            PlayerData.Instance.selectedSkinID = item.ID;
            PlayerData.Instance.SaveData();
            GameplayController.Instance.UpdateSkin(item.texture);
        }
        //If not purchased : if cash is sufficient show "buy" pop up. else show "Insufficient cash" pop up
        else
        { 
            if (hasSufficientCash)
            {
                UIManager.Instance.ToggleOpenCostPopUp(true);
            }
            else
            {
                UIManager.Instance.ToggleOpenCostPopUp(true, "Insufficient cash!");
            }
        }
    }

    /// <summary>
    /// Overriding this method for implementing actions after item is purchased result
    /// </summary>
    /// <param name="success">If set to <c>true</c> success.</param>
    /// <param name="remaingingCash">Remainging cash.</param>
    /// <param name="newUnlockedId">New unlocked identifier.</param>
    protected override void ItemPurcahsed(bool success = true, float remaingingCash = 0, int newUnlockedId = 0)
    {
        SoundManager.Instance.playSound(AudioClips.UI);
        //if successful in purchase, update player data in persistat storage with the new unlocked item and remaining cash.
        if (success)
        {
            PlayerData.Instance.unlockedSkinItems.Add(newUnlockedId);
            PlayerData.Instance.selectedSkinID = newUnlockedId;
            PlayerData.Instance.cash = remaingingCash;
            PlayerData.Instance.SaveData();
            UIManager.Instance.cashText.text = remaingingCash.ToString();
            UIManager.Instance.ToggleOpenCostPopUp(false);
        }
        //else dismiss the UI pop up.
        else
        {
            UIManager.Instance.ToggleOpenCostPopUp(false);
        }
    }
}
}