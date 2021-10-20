using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;


    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    //int[] 에는 해당 퀘스트에 연관된 NPC Id를 입력
    void GenerateData()
    {
        // 퀘스트 아이디, 퀘스트 데이터( 퀘스트, 퀘스트와 연관된 NPC ID // questActionIndex 은 말의 수가 아니라 int 값 뒤의 갯수)
        questList.Add(10, new QuestData("강아지찾기", new int[] { 1000, 2000, 2000 }));

        questList.Add(20, new QuestData("강아지납치", new int[] { 1000 }));

        questList.Add(30, new QuestData("퀘스트 모두 완료", new int[] { 0 }));

    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }


    // 대화가 끝이났을때 대화 진행을 위해 퀘스트 대화순서를 올리는 함수
    public string CheckQuest(int id)
    {


        // Next Talk Target
        if (id == questList[questId].questNpcId[questActionIndex])
            questActionIndex++;

        // Control Quest Object
        ControlObject();


        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].questNpcId.Length)
            NextQuest();

        // Quest Name
        return questList[questId].questName;
    }


    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    //퀘스트 오브젝트를 관리할 함수
    void ControlObject()
    {
        switch(questId)
        {
            case 10:
                if (questActionIndex == 3) 
                    questObject[0].SetActive(false);
                    break;

            case 20:
                if (questActionIndex == 1) 
                    questObject[1].SetActive(true);
                    break;

        }
    }
}
