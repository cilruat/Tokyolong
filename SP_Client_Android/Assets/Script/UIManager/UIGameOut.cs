using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOut : MonoBehaviour {

    public eUI uiType;
    public GameObject objSelect;
    public Text txtTableNum;


    public Animator anim;


    //안쓰는건데 나중을 위해 남겨두자

    //단순히 SystemMessage 보내는것보다 이게 더 나을것
    public void ShowGameOut()
    {
        byte tableNum = 0;

        if (Info.myInfo.listCanCelInfo.Count > 0)
        {
            UserCancelInfo info = Info.myInfo.listCanCelInfo[Info.myInfo.listCanCelInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum.text = tableNum.ToString();

        anim.Play("UIVersusOut");
        StartCoroutine(_DestroyShadow());
    }

    IEnumerator _DestroyShadow()
    {
        yield return new WaitForSeconds(3f);
        UIManager.Instance.Hide(eUI.eGameOut);
        UIManager.Instance.isGameRoom = false;

    }

}
