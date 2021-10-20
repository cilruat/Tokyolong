using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCAction : MonoBehaviour {


    //Idle과 walk 상태, 범위를 지정해줄것


    public int npcID;
    public bool isNPC;

    public bool isChoice;

    public GameObject objNPC;
    public GameObject objTalkBox;
    public GameObject objAlarm;

    public Text txtTalk;
    public string m_text;

    public bool IsEnter = false;

    public Button btnNPC;

    public DialogueManager manager;

    //public Animation animNPC;



    private void Start()
    {
        btnNPC.interactable = false;
        StartCoroutine(_typing());
    }

    public void OnEnterPlayer()
    {
        IsEnter = true;
        objAlarm.SetActive(true);
        objTalkBox.SetActive(true);
        btnNPC.interactable = true;

    }

    public void OnExitPlayer()
    {
        IsEnter = false;
        objAlarm.SetActive(false);
        objTalkBox.SetActive(false);
        btnNPC.interactable = false;

        Debug.Log("나감");
    }

    public void OnButtonClick(GameObject objNPC)
    {
        manager.Action(objNPC);
    }



    IEnumerator _typing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < m_text.Length; i++)
            {
                txtTalk.text = m_text.Substring(0, i);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
