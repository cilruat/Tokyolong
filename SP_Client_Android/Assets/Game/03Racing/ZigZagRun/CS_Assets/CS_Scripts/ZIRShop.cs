using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Zigzag.Types;

namespace Zigzag
{
	/// <summary>
	/// This script handles a shop, in which there are items that can be bought and unlocked with money.
	/// </summary>
	public class ZIRShop : MonoBehaviour
	{
		//How much money we have left in the shop
		public int moneyLeft = 0;

		//The text that displays the money we have
		public Transform moneyText;
		public string moneyTextSuffix = "$";

		//The player prefs record of the money we have
		public string moneyPlayerPrefs = "Money";
		
		// An array of shop items that can be bought and unlocked
		public ShopItem[] shopItems;
		
		//The number of the currently selected item
		public int currentItem = 0;
		
		//This is the player prefs name that will be updated with the number of the currently selected item
		public string playerPrefsName = "CurrentPlayer";
		
		//The color of the item when we have at least one of it
		public Color unselectedColor = new Color(0.6f,0.6f,0.6f,1);
		
		//The color of the item when it is selected
		public Color selectedColor = new Color(1,1,1,1);
		
		//The color of the item when we can't afford it
		public Color errorColor = new Color(1,0,0,1);
		
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			//Get the number of money we have
			moneyLeft = PlayerPrefs.GetInt(moneyPlayerPrefs, moneyLeft);
			
			//Update the text of the money we have
			moneyText.GetComponent<Text>().text = moneyLeft.ToString() + moneyTextSuffix;
			
			//Get the number of the current player
			currentItem = PlayerPrefs.GetInt(playerPrefsName, currentItem);
			
			//Update all the items
			UpdateItems();
		}

		/// <summary>
		/// Updates the items we have in the shop, locking/unlocking them accordingly
		/// </summary>
		void UpdateItems()
		{
			for ( int index = 0 ; index < shopItems.Length ; index++ )
			{
				//Get the lock state of this item from player prefs
				shopItems[index].lockState = PlayerPrefs.GetInt(shopItems[index].playerPrefsName, shopItems[index].lockState);
				
				//Deselect the item
				shopItems[index].itemButton.GetComponent<Image>().color = unselectedColor;
				
				//If we already unlocked this item, don't display its price
				if ( shopItems[index].lockState > 0 )
				{
					//Deactivate the price and money icon
					shopItems[index].itemButton.Find("TextPrice").gameObject.SetActive(false);
					
					//Highlight the currently selected item
					if ( index == currentItem )    shopItems[index].itemButton.GetComponent<Image>().color = selectedColor;
				}
				else
				{
					//Update the text of the cost
					shopItems[index].itemButton.Find("TextPrice").GetComponent<Text>().text = shopItems[index].costToUnlock.ToString() + moneyTextSuffix;
				}
			}
		}
		
		/// <summary>
		/// Buys an item from the shop and unlocks it, but only if we have enough money
		/// </summary>
		/// <param name="itemNumber">Index of the item to buy</param>
		void BuyItem( int itemNumber )
		{
			//If we already unlocked this item, just select it
			if ( shopItems[itemNumber].lockState > 0 )
			{
				//Select the item
				SelectItem(itemNumber);
			}
			else if ( shopItems[itemNumber].costToUnlock <= moneyLeft ) //If we have enough money, buy this item
			{
				//Increase the item count
				shopItems[itemNumber].lockState = 1;
				
				//Register the item count in the player prefs
				PlayerPrefs.SetInt(shopItems[itemNumber].playerPrefsName, shopItems[itemNumber].lockState);
				
				//Deduct the price from the money we have
				moneyLeft -= shopItems[itemNumber].costToUnlock;
				
				//Update the text of the money we have
				moneyText.GetComponent<Text>().text = moneyLeft.ToString() + moneyTextSuffix;
				
				//Register the item lock state in the player prefs
				PlayerPrefs.SetInt(moneyPlayerPrefs, moneyLeft);
				
				//Select the item
				SelectItem(itemNumber);
			}
			
			//Update all the items
			UpdateItems();
		}
		
		//This function selects an item
		/// <summary>
		/// Selects an item from the shop, and set it to be the current item
		/// </summary>
		/// <param name="itemNumber">Index of the item</param>
		void SelectItem( int itemNumber )
		{
			currentItem = itemNumber;
			
			PlayerPrefs.SetInt( playerPrefsName, itemNumber);
		}
	}
}




