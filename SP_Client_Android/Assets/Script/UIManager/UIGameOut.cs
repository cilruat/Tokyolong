using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOut : MonoBehaviour {

    public eUI uiType;
    public GameObject objSelect;
    public Text[] txtTableNum;


    public Animator anim;


    public void ShowGameOut()
    {
        
        byte tableNum = 0;

        if (Info.myInfo.listCanCelInfo.Count > 0)
        {
            UserCancelInfo info = Info.myInfo.listCanCelInfo[Info.myInfo.listCanCelInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum[0].text = tableNum.ToString();

        Debug.Log(Info.myInfo.listCanCelInfo[0]);
        anim.Play("UIVersusOut");
        StartCoroutine(_DestroyShadow());
    }

    IEnumerator _DestroyShadow()
    {
        UIManager.Instance.Hide(eUI.eGameOut);
        yield return new WaitForSeconds(2f);
        UIManager.Instance.isGameRoom = false;
        SceneChanger.LoadScene("Main", PageBase.Instance.curBoardObj());
    }

}
