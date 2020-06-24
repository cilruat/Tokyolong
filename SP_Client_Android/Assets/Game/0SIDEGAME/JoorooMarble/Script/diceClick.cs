using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class diceClick : MonoBehaviour
{
    public class TEAM
    {
        public int num = 0;
        public string name = "";
        public int score = 0;
        public int locate = 1;
        public TEAM(int num, string name)
        {
            this.num = num;
            this.name = name;
        }
    }
    public List<string> bad;
    public List<string> good;
    public bool chk = true;
    public int tnstj = 1; // 1~7 = abcdef 순서
    public GameObject _dice, goodcr, badcr;
    public Animator anim;
    public int timer = 0;
    public int random, stacksoju = 0;
    public Text score_a, score_b, score_c, score_d, score_e, score_f, score_g, ckfP, stackso;
    public int a, b, c, d, e, f, g;
    public List<TEAM> teamList;

	public Slider slider;


    // Use this for initialization
    void Awake()
    {
        tnstj = 1;
        print(tnstj);
        teamList = new List<TEAM>();
        teamList.Add(new TEAM(0, "X"));
        teamList.Add(new TEAM(1, "A"));
        teamList.Add(new TEAM(2, "B"));
        teamList.Add(new TEAM(3, "C"));
        teamList.Add(new TEAM(4, "D"));
        teamList.Add(new TEAM(5, "E"));
        teamList.Add(new TEAM(6, "F"));
        teamList.Add(new TEAM(7, "G"));
        bad.Add("(뽑은사람 조원 전체)소주 1잔 마시기");
        bad.Add("(뽑은사람 조원 전체)소주 1잔 마시기");
        bad.Add("임원들 소주 1잔씩 마시기");
        bad.Add("임원들 소주 1잔씩 마시기");
        bad.Add("LiFliNe 소속 1잔씩 마시기");
        bad.Add("17막내 소주 1잔 마시기");
        bad.Add("18막내 소주 1잔 마시기");
        bad.Add("99단체짠");
        bad.Add("98단체짠");
        bad.Add("LiFliNe 소속 1잔씩 마시기");
        bad.Add("(뽑은사람 본인만)소주 1잔 마시기");
        bad.Add("(뽑은사람 본인만)소주 1잔 마시기");
        bad.Add("(뽑은사람 지정)지목한 조 1잔씩 마시기");
        bad.Add("(뽑은사람 지정 1명)뽑은 사람과 소주 1잔 마시기");
        bad.Add("(뽑은사람 본인만)10초동안 춤 추기");
        bad.Add("(뽑은사람 본인만)10초동안 춤 추기");
        bad.Add("동아리 중복가입인 사람 소주 1잔 마시기");
        bad.Add("동아리 중복가입인 사람 소주 1잔 마시기");
        bad.Add("(뽑은사람 본인만)노래 한소절 하기(무반주)");
        bad.Add("(뽑은사람 본인만)노래 한소절 하기(무반주)");
        bad.Add("(뽑은사람 본인)회장님 한잔 따라드리기");
        bad.Add("(뽑은사람 본인)부회장님 한잔 따라드리기");
        bad.Add("각 조장들 소주 한잔 씩 마시기");
        bad.Add("각 조장들 소주 한잔 씩 마시기");
        bad.Add("18학번 소주 1잔씩 마시기");
        bad.Add("17학번 소주 1잔씩 마시기");
        bad.Add("(뽑은사람이 시작)더게임오브데스");
        bad.Add("(뽑은사람이 시작)공산당");
        bad.Add("(뽑은사람이 시작)눈치게임");
        bad.Add("(뽑은사람 본인)옆사람과 가위바위보 : 소주2잔걸고");
        bad.Add("(뽑은사람 본인만)소주 2잔 마시기");
        bad.Add("(뽑은사람 본인만)아무나 성대모사 하기(아무도 못맞출 경우 실패)");
        bad.Add("1~3월생 소주 1잔 마시기");
        bad.Add("4~6월생 소주 1잔 마시기");
        bad.Add("7~9월생 소주 1잔 마시기");
        bad.Add("10~12월생 소주 1잔 마시기");
        bad.Add("(뽑은사람 지목)조 별 가위바위보 토너먼트 : 1병걸고");
        bad.Add("(뽑은사람 본인)자신의 이름을 모를 것 같은 사람 찾기(기회 1번, 찾을 경우 상대방이 소주1잔)");
        bad.Add("조장들끼리 가위바위보(최후의 패배조은 조원들 소주한잔씩 마시기)");
        bad.Add("(뽑은사람이 시작하고 왼쪽으로)귀엽고 깜찍한 베스킨라빈스31");
        good.Add("(뽑은사람 팀이아닌)청/홍팀 소주1병 알아서 분할하여 마시기");
        good.Add("(뽑은사람 본인)5분동안 입 꾹 닫기");
        good.Add("임원들 소주1병 알아서 분할하여 마시기");
        good.Add("다음 사람 벌칙 대신 수행해주기");
        good.Add("다음 사람 벌칙 대신 수행해주기");
        good.Add("(흑기사 가능, 뽑은사람 본인)소주 4잔");
        good.Add("트레비 한병 원샷");
        print(good.Count);

    }

    // Update is called once per frame
    void Update()
    {
        score_a.text = "A조 : " + teamList[1].score + "점";
        score_b.text = "B조 : " + teamList[2].score + "점";
        score_c.text = "C조 : " + teamList[3].score + "점";
        score_d.text = "D조 : " + teamList[4].score + "점";
        score_e.text = "E조 : " + teamList[5].score + "점";
        score_f.text = "F조 : " + teamList[6].score + "점";
        score_g.text = "G조 : " + teamList[7].score + "점";
        ckfP.text = teamList[tnstj].name + "팀 " + "차례!";
        stackso.text = "누적 소주 : " + stacksoju + "잔";
    }
    public void click()
    {
        if (chk)
        {
            chk = false;
            _dice.gameObject.SetActive(false);
            _dice.gameObject.SetActive(true);
            StartCoroutine("dice");
        }
    }

    IEnumerator dice()
    {
        anim.Play("dice");
        while (true)
        {
            if (timer == 1)
            {
                break;
            }
            timer++;
            yield return new WaitForSeconds(1f);
        }
        random = Random.Range(1, 7);
        print(random);
        switch (random)
        {
            case 1:
                anim.Play("1");
                break;
            case 2:
                //_dice.transform.rotation = Quaternion.Euler(-90, 0, -180);
                anim.Play("2");
                break;
            case 3:
                //_dice.transform.rotation = Quaternion.Euler(-0, 0, -360);
                anim.Play("3");
                break;
            case 4:
                //_dice.transform.rotation = Quaternion.Euler(-180, 0, -180);
                anim.Play("4");
                break;
            case 5:
                //_dice.transform.rotation = Quaternion.Euler(-90, 0, -360);
                anim.Play("5");
                break;
            case 6:
                //_dice.transform.rotation = Quaternion.Euler(-90, 0, -270);
                anim.Play("6");
                break;

        }
        if (teamList[tnstj].locate == 7)
        {
            //무인도
            if (random % 2 == 1)
            {
                asd();
            }
            else
            {
                timer = 0;
                tnstj++;
                if (tnstj == 8)
                    tnstj = 1;
                chk = true;
            }
        }
        else
        {
            asd();
        }
    }
    public void asd()
    {
        teamList[tnstj].locate += random;
        if (teamList[tnstj].locate >= 13)
        {
            teamList[tnstj].locate -= 12;
            teamList[tnstj].score++;
        }
        print(teamList[tnstj].name + "team");
        GameObject.Find("Canvas").transform.Find(teamList[tnstj].name + "team").transform.position = GameObject.Find("Canvas").transform.Find("background").
            transform.Find(teamList[tnstj].locate + "").transform.Find(teamList[tnstj].name + "pos").transform.position;
        switch (teamList[tnstj].locate)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("bad").transform.Find("Text").GetComponent<Text>().text = bad[Random.Range(0, bad.Count)];
                GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(true);
                break;
            case 4:
                break;
            case 5:
                stacksoju++;
                break;
            case 6:
                GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("bad").transform.Find("Text").GetComponent<Text>().text = bad[Random.Range(0, bad.Count)];
                GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(true);
                break;
            case 7:
                break;
            case 8:
                GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("bad").transform.Find("Text").GetComponent<Text>().text = bad[Random.Range(0, bad.Count)];
                GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(true);
                break;
            case 9:
                break;
            case 10:
                GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("bad").transform.Find("Text").GetComponent<Text>().text = "소주" + stacksoju + "잔 마시기";
                GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(true);
                stacksoju = 0;
                break;
            case 11:
                GameObject.Find("Canvas").transform.Find("good").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("good").transform.Find("Text").GetComponent<Text>().text = good[Random.Range(0, good.Count)];
                GameObject.Find("Canvas").transform.Find("gooddefend").gameObject.SetActive(true);
                break;
            case 12:
                GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("bad").transform.Find("Text").GetComponent<Text>().text = bad[Random.Range(0, bad.Count)];
                GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(true);
                break;

        }
        timer = 0;
        tnstj++;
        if (tnstj == 8)
            tnstj = 1;
        chk = true;
    }
    public void badcardClick()
    {
        GameObject.Find("Canvas").transform.Find("bad").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("baddefend").gameObject.SetActive(false);
    }
    public void goodcardClick()
    {
        GameObject.Find("Canvas").transform.Find("good").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("gooddefend").gameObject.SetActive(false);
    }
}
