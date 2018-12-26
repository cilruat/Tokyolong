using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyProfile : MonoBehaviour 
{
    public Text textTableNo;
    public RawImage imgCustomer;
    public Text textPeopleCnt;
    public Text todayWin;
    public Text todayDiscount;
    public GameObject newChat; 

    public Texture[] imgCustomers;

    void Awake()
    {
        SetInfo();
    }   

    public void SetInfo()
    {
        textTableNo.text =  "NO. " + Info.TableNum.ToString();

        float rtImgWidth = Info.ECustomer != ECustomerType.COUPLE ? 50f : 100f;
        imgCustomer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rtImgWidth);
        imgCustomer.texture = imgCustomers[(int)Info.ECustomer];

        textPeopleCnt.text = Info.PersonCnt.ToString() + "명";
        todayWin.text = "-";
        todayDiscount.text = "-";
    }

    public void ShowChat()
    {
        GameObject obj = UIManager.Instance.Show(eUI.eChat);
        UIChat uiChat = obj.GetComponent<UIChat>();
        uiChat.ShowChatTable();
    }
}
