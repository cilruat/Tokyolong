using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour {


    //Dictionary는 한쌍, int와 value를 항상 써줘야한다
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

	void Awake ()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();

        GenerateData();
	}
	
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕?:0",
                                          "이곳에 처음왔구나?:1" });

        talkData.Add(2000, new string[] { "멍멍!! 멍멍!!"});


        talkData.Add(3000, new string[] { "하하하 여기서 명령은 못내리나?" });


        talkData.Add(100, new string[] { "평범한 나무상자다" });

        // Quest Talk
        talkData.Add(10 + 1000, new string[] { "사랑아 어딧니 사랑아 ㅠㅠ:0",
                                               "강아지를 잃어버렸어요..:3",
                                               "사랑이를 누가 찾아주면 참 좋을텐데....:1" });

        talkData.Add(11 + 1000, new string[] { "사랑이를 찾아주면 사례를 해야지...:0"});


        talkData.Add(11 + 2000, new string[] { "멍멍멍!!!",
                                               "끼이이이이잉...",
                                               "(이 강아지..아까 그 여자가 찾던 강아지 아닌가?)" });

        talkData.Add(12 + 2000, new string[] { "(강아지를 데리고 간다)" });

        //NPC면 초상화 꼭 넣어주기
        talkData.Add(12 + 1000, new string[] { "사랑아 어딨니 사랑아 ㅠㅠ:0" });



        talkData.Add(20 + 1000, new string[]  { "어? 사랑아!!!:1",
                                               "사랑이를 찾아주셧군요 감사해요:2",
                                               "소소하지만 여기..제가 코인을 드릴께요!:2" });







        // Portrait Data // 앞 + int 값이 대화상자에 들어가는 :int 값과 동일, 뒷값은 스프라이트 배정값
        portraitData.Add(1000 + 0, portraitArr[2]); // Cry
        portraitData.Add(1000 + 1, portraitArr[3]); // Quest
        portraitData.Add(1000 + 2, portraitArr[5]); // Good
        portraitData.Add(1000 + 3, portraitArr[6]); // Sad

    }

    public string GetTalk(int id, int talkIndex)
    {
        // ConstainKey == Ditionary 에 Key가 존재하는지 검사
        if(!talkData.ContainsKey(id))
        {
            if(!talkData.ContainsKey(id - id % 10))
            {
                //퀘스트 맨 처음 대사마저 없을때
                //기본 대사를 가지고 온다
                if (talkIndex == talkData[id - id % 100].Length)
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
            }
            else
            {
                //해당 퀘스트 진행순서중 대사가 없을때
                //퀘스트 맨  처음 대사를 가지고 온다
                if (talkIndex == talkData[id - id % 10].Length)
                    return null;
                else
                    return talkData[id - id % 10][talkIndex];
            }


        }

        if (talkIndex == talkData[id].Length)
            return null;

        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
