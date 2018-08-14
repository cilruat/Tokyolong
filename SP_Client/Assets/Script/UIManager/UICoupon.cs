using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class UICoupon : MonoBehaviour 
{
    public GameObject objReconfirmClose;

    public void OnClose()
    {
        if (objReconfirmClose.activeSelf)
            return;

        objReconfirmClose.SetActive(true);
    }

    public void OnReconfirmClose()
    {
        if (objReconfirmClose.activeSelf == false)
            return;
        
        objReconfirmClose.SetActive(false);

        UIManager.Instance.Hide(eUI.eCoupon);
    }
}
