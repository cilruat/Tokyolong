using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
	#region Singlton:Shop

	public static Shop Instance;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}

	#endregion

	[System.Serializable] public class ShopItem
	{
		public Sprite Image;
		public int Price;
		public bool IsPurchased = false;
	}

	public List<ShopItem> ShopItemsList; 

    public GameObject prefabMenu;
	public GameObject Menu;
	public Transform ShopScrollView;
    public GameObject ShopPanel;

    public GameObject MenuImg;
    public GameObject MenuPrice;

	void Start ()
	{
		int len = ShopItemsList.Count;
		for (int i = 0; i < len; i++) {
            Menu = Instantiate (prefabMenu, ShopScrollView);

            Menu.transform.GetChild (0).GetComponent <Image> ().sprite = ShopItemsList [i].Image;
            Menu.transform.GetChild (1).GetChild (0).GetComponent <Text> ().text = ShopItemsList [i].Price.ToString ();
            Menu.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].ToString(); // 여기 네임 추가해야됨
		}
	}

	void OnShopItemBtnClicked (int itemIndex)
	{
		if (Game.Instance.HasEnoughCoins (ShopItemsList [itemIndex].Price)) {
			Game.Instance.UseCoins (ShopItemsList [itemIndex].Price);
			//purchase Item
			ShopItemsList [itemIndex].IsPurchased = true;

			//change UI text: coins
			Game.Instance.UpdateAllCoinsUIText ();

            Profile.Instance.AddAvatar (ShopItemsList [itemIndex].Image);
		} else {
			Debug.Log ("You don't have enough coins!!");
		}
	}

	/*---------------------Open & Close shop--------------------------*/
	public void OpenShop ()
	{
		ShopPanel.SetActive (true);
	}

	public void CloseShop ()
	{
		ShopPanel.SetActive (false);
	}

}
