using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public TalkManager talkManager;
    public int talkIndex;

    public Animator talkPanel;
    public TypeEffect talk;
    public GameObject scanObject;

    public bool isAction = false;

    public Image portraitImg;
    public Animator portraitAnim;
    public Sprite prevPortrait;

    public QuestManager questManager;

    public ChoiceManager choiceManager;



    public void Action(GameObject scanObj)
    {
        talkPanel.SetBool("isShow", true);
        scanObject = scanObj;
        NPCAction objData = scanObject.GetComponent<NPCAction>();
        Talk(objData.npcID, objData.isNPC, objData.isChoice);
    }


    public void disableTalkPanel()
    {
        NPCAction objData = scanObject.GetComponent<NPCAction>();
        Talk(objData.npcID, objData.isNPC, objData.isChoice);
    }


    public void Talk(int id, bool isNPC, bool isChoice)
    {

        // Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";


        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // End Talk // 여기서 조절해주면 되네
        if (talkData == null)
        {
            talkPanel.SetBool("isShow", false);
            talkIndex = 0;

            if (isChoice == true)
            {
                //choiceManager.ShowChoice();
            }
            else
            {
                isAction = false; //초이스 없어서 움직일수있다.
                Debug.Log(questManager.CheckQuest(id));
                return;
            }
        }

        // Continue Talk
        if(isNPC)
        {
            talk.SetMsg(talkData.Split(':')[0]);
            
            //Parse 문자열을 해당 타입으로 변환해주는 함수, 형변환

            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);

            // Animation Portrait
            if(prevPortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else
        {
            talk.SetMsg(talkData);
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }




}
