using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace CrashRacing
{
    public class StoreUIController : MonoBehaviour
    {
        public GameObject coinPackPrefab;
        public Transform productList;

        // Use this for initialization
        void Start()
        {
            var purchaser = InAppPurchaser.Instance;
            for (int i = 0; i < purchaser.coinPacks.Length; i++)
            {
                InAppPurchaser.CoinPack pack = purchaser.coinPacks[i];
                GameObject newPack = Instantiate(coinPackPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                Transform newPackTf = newPack.transform;
                newPackTf.Find("CoinValue").GetComponent<Text>().text = pack.coinValue.ToString();
                newPackTf.Find("Button/PriceString").GetComponent<Text>().text = pack.priceString;
                newPackTf.SetParent(productList, true);
                newPackTf.localScale = Vector3.one;

                // Add button listener
                newPackTf.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Utilities.ButtonClickSound();

                        #if EASY_MOBILE
                        purchaser.Purchase(pack.productName);
                        #endif
                    });
            }
        }
    }
}
