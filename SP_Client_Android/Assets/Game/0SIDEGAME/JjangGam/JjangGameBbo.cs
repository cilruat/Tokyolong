using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JjangGameBbo : SingletonMonobehaviour<JjangGameBbo>
{

    public SpriteRenderer RSPSR;
    public Sprite[] RSPSprites;
    public GameObject[] Roullettes;
    public GameObject InsertBtn, Blind;
    public Text LogText, CoinText;

    public GameObject PanelNoCoin;

    int rsp, input = -1; // 0바위, 1가위, 2보
    int rouletteNum, myCoin = 1;
    int[] rouletteCoins = new int[12] { 1, 2, 2, 1, 2, 5, 1, 2, 2, 1, 2, 2};

    public AudioClip clipOpening;
    public AudioClip clipWin;
    public AudioClip clipCoin;

    public AudioSource audioOpening;
    public AudioSource audioWin;
    public AudioSource audioYappi;



    private void Start()
    {
        PanelNoCoin.SetActive(false);
    }

    public void Insert()
    {
        if (Info.GamePlayCnt >= 1)
        {
            audioOpening.Play();
            Blind.SetActive(false);
            rsp = 0;
            input = -1;
            rouletteNum = 0;
            for (int i = 0; i < Roullettes.Length; i++)
                Roullettes[i].SetActive(false);

            StartCoroutine(RSPLoop());
        }
        else
        {
            InsertBtn.SetActive(true);
        }
    }

    void End()
    {
        InsertBtn.SetActive(true);
        //CoinText.gameObject.SetActive(true);
        //Refresh해주면 좋을텐데
        ShowCoin();
    }

    IEnumerator ShowLog(string log)
    {
        LogText.text = log;
        yield return new WaitForSeconds(1);
        LogText.text = "";
    }

    public void ShowCoin()
    {
        //CoinText.text = myCoin.ToString();
        //myCoin = Info.GamePlayCnt;
        //myCoin = Info.GamePlayCnt;

        CoinText.text = Info.GamePlayCnt.ToString();
    }

    public void MinusCoin()
    {
        if (Info.GamePlayCnt >= 1)
        {
            NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, -1);
            ShowCoin();
        }
        else
        {
            PanelNoCoin.SetActive(true);
        }
    }

    public void ClosePanelBtn()
    {
        PanelNoCoin.SetActive(false);
    }


    void RSPWinner()
    {
        if (rsp == input) // 비김
        {
            //비겨도 돈날리는거입니다
            Invoke("Insert", 1);
            //End();
            StartCoroutine(ShowLog("비겼다"));
        }

        else if ((rsp == 0 && input == 2) || (rsp == 1 && input == 0) || (rsp == 2 && input == 1)) // 이김
        {
            StartCoroutine(RouletteSpin());
            StartCoroutine(ShowLog("이겼다!"));
            audioWin.Play();
        }

        else // 짐
        {
            End();
            StartCoroutine(ShowLog("졌다ㅠ"));
        }
    }

    IEnumerator RSPLoop()
    {
        while (true)
        {
            if (input != -1) break;
            if (++rsp > 2) rsp = 0;
            RSPSR.sprite = RSPSprites[rsp];
            yield return new WaitForSeconds(0.05f);
        }
        RSPWinner();
    }

    public void InputRSP(int value)
    {
        input = value;
        Blind.SetActive(true);
    }


    IEnumerator RouletteSpin()
    {
        int count = 0;
        int curNum = Random.Range(0, Roullettes.Length);

        while (true)
        {
            if (count >= 2 && curNum == rouletteNum) break;
            if (++rouletteNum > 11) { rouletteNum = 0; ++count; }

            for (int i = 0; i < Roullettes.Length; i++)
                Roullettes[i].SetActive(rouletteNum == i);
            yield return new WaitForSeconds(0.07f);
            audioYappi.Play();

        }

        int curCoin = rouletteCoins[rouletteNum];
        //myCoin += curCoin;
        NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, +curCoin);
        ShowCoin();
        StartCoroutine(ShowLog(curCoin + " 얻었다"));
        End();
    }


}
