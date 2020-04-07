using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    [Header("LineCanvas")]
    public GameObject[] Line;
    public GameObject[] Verse;
    
    [Header("UpPanel")]
    public Text PersonnelText;
    public Slider slider;

    [Header("MiddlePanel")]
    public GameObject[] Finish;
    public GridLayoutGroup PlayersGl;
    public GameObject[] Player;
    public Player[] PlayerCs;

    [Header("DownPanel")]
    public Dropdown dropdown;
    public InputField[] inputField;

    [Header("OtherPanel")]
    public GameObject BeforeStartPanel;
    public GameObject ResultPanel;
    public Text[] ResultText;
    public GameObject ErrorPanel;
    public GameObject BlindPanel;
    public GameObject DownBlindPanel;

    int peopleNum;
    bool isBlocker;
    bool isEndAll;

	private AudioSource LadderOn; //for use Audio source
	[SerializeField] private AudioClip LadderClip;

/*#if (UNITY_ANDROID)
    void Awake() { Screen.SetResolution(1080, 1920, false); }
#else
    void Awake() { Screen.SetResolution(540, 960, false); }
#endif*/

    void Start() 
	{ 
		LadderOn = GetComponent<AudioSource> ();

		LadderSetting(false); 
	}


    void Update()
    {
        // 추천내기 선택이 끝나면 추천내기 실행
        if (GameObject.Find("Blocker")) isBlocker = true;
        if (isBlocker && !GameObject.Find("Blocker")) { isBlocker = false; Recommand(); }

        // 플레이어가 모두 내려가면 결과창 띄움
        if (isEndAll) return;
        isEndAll = true;
        for (int i = 0; i < peopleNum; i++) if (!PlayerCs[i].isEnd) isEndAll = false;
        if (isEndAll)
        {
            ResultPanel.SetActive(true);
            for (int i = 0; i < peopleNum; i++)
                ResultText[i].text = inputField[PlayerCs[i].finishNum - 1].text;
        }
    }


    // 슬라이더가 바뀔때
    public void LadderSetting(bool isRestart)
    {
        // 다시하기 버튼으로 불러온게 아니라면
        if (!isRestart)
        {
            BeforeStartPanel.SetActive(true);
            DownBlindPanel.SetActive(false);
        }
        ResultPanel.SetActive(false);

        // 인원 수 넣기
        peopleNum = (int)slider.value;
        PersonnelText.text = peopleNum + "명";

        // 끝난것들 초기화
        for (int i = 0; i < 6; i++) PlayerCs[i].Clear();
        isEndAll = false;

        // 플레이어 정렬
        PlayersGl.enabled = true;

        // 라인, 끝번호, 플레이어, 인풋필드 부모셀, 결과텍스트 부모셀 인원수만큼 활성화
        for (int i = 0; i < 6; i++)
        {
            if(i < peopleNum)
            {
                Line[i].SetActive(true);
                Finish[i].SetActive(true);
                Player[i].SetActive(true);
                inputField[i].transform.parent.gameObject.SetActive(true);
                ResultText[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                Line[i].SetActive(false);
                Finish[i].SetActive(false);
                Player[i].SetActive(false);
                inputField[i].transform.parent.gameObject.SetActive(false);
                ResultText[i].transform.parent.gameObject.SetActive(false);

                // 인원 수 줄어들때 사라진 부분은 인풋필드가 비워짐
                inputField[i].text = "";
            }
        }

        // 모든 행 비활성화
        for (int i = 0; i < Verse.Length; i++)
            Verse[i].SetActive(false);

        // 활성화 된 라인들의 7행 중 2~4행을 랜덤하게 활성화
        for (int i = 0; i < peopleNum - 1; i++)
        {
            List<int> list = new List<int>() { 7 * i, 7 * i + 1, 7 * i + 2, 7 * i + 3, 7 * i + 4, 7 * i + 5, 7 * i + 6 };
            for (int j = 0; j < Random.Range(2, 5); j++)
            {
                int rand = Random.Range(0, list.Count);
                Verse[list[rand]].SetActive(true);
                list.RemoveAt(rand);
            }
        }
    }

    // 모든 버튼 입력
    public void ButtonClick(string whatButton)
    {
        switch (whatButton)
        {
                // BeforeStartPanel - 사다리타기 시작하기 버튼, 인풋필드 하나라도 비어있으면 에러패널 띄움
            case "BeforeStart":
                for (int i = 0; i < peopleNum; i++)
                    if(inputField[i].text == "") { ErrorPanel.SetActive(true); return; }

                BeforeStartPanel.SetActive(false);
                DownBlindPanel.SetActive(true);
                break;

                // DownPanel - 초기화버튼
            case "InputReset":
                for (int i = 0; i < peopleNum; i++)
                    inputField[i].text = "";
                break;

                // MiddlePanel - 다시하기 버튼, ResultPanel - 다시하기 버튼
            case "Restart":
                LadderSetting(true);
                break;

                // MiddlePanel - 전체결과 버튼
            case "Result":
                for (int i = 0; i < peopleNum; i++) PlayerCs[i].StartCoroutine("Move");
				LadderOn.clip = LadderClip;
				LadderOn.Play ();
                break;


			case "InGameReset":
				for (int i = 0; i < peopleNum; i++)
					inputField[i].text = "";
					DownBlindPanel.SetActive(false);
					BeforeStartPanel.SetActive(true);

				break;

                // MiddlePanel - 플레이어 버튼들
            default:
                for (int i = 0; i < 6; i++) if (whatButton == "Player" + (i + 1).ToString()) PlayerCs[i].StartCoroutine("Move");
                break;
        }
    }

    void Recommand()
    {
        List<string> allList = new List<string>();

        switch (dropdown.value)
        {
                // 추천내기를 누르면 초기화
            case 0: ButtonClick("InputReset"); return;

                // 간식내기
            case 1:
                switch (peopleNum)
                {
                    case 2: allList = new List<string>() { "간식사오기", "5천원내기" }; break;
                    case 3: allList = new List<string>() { "간식사오기", "5천원내기", "2천원내기" }; break;
                    case 4: allList = new List<string>() { "간식사오기", "5천원내기", "2천원내기", "천원내고사오기" }; break;
                    case 5: allList = new List<string>() { "간식사오기", "5천원내기", "2천원내기", "3천원내기", "그냥쉬세요" }; break;
                    case 6: allList = new List<string>() { "간식사오기", "5천원내기", "2천원내기", "3천원내기", "그냥쉬세요", "2천원내고사오기" }; break;
                }
                break;

                // 벌칙자뽑기
            case 2: allList = new List<string>() { "걸렸다ㅠㅠ", "살았다^^", "살았다^^", "살았다^^", "살았다^^", "살았다^^" }; break;

                // 당첨자뽑기
            case 3: allList = new List<string>() { "아싸 당첨!", "꽝", "꽝", "꽝", "꽝", "꽝" }; break;

                // 순서정하기
            case 4: allList = new List<string>() { "1번", "2번", "3번", "4번", "5번", "6번" }; break;

                // 편나누기
            case 5: allList = new List<string>() { "A팀", "B팀", "A팀", "B팀", "A팀", "B팀" }; break;

                // 조모임역할
            case 6: allList = new List<string>() { "발표하기", "PPT만들기", "자료조사", "자료조사", "PPT만들기", "자료조사" }; break;
        }

        // 리스트에 인원수만큼 문장 넣음
        List<string> list = new List<string>();
        for (int i = 0; i < peopleNum; i++) list.Add(allList[i]);

        // 인풋필드에 섞어서 넣음
        for (int i = 0; i < peopleNum; i++)
        {
            int rand = Random.Range(0, list.Count);
            inputField[i].text = list[rand];
            list.RemoveAt(rand);
        }
    }
}
