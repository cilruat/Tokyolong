using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JjangGameBbo : MonoBehaviour {

    public SpriteRenderer RSPSR;
    public Sprite[] RSPSprites;
    public GameObject[] Roullettes;
    public GameObject InsertBtn, Blind;
    public Text LogText, CoinText;

    int rsp, input = -1; // 0바위, 1가위, 2보
    int rouletteNum, myCoin = 1;
    int[] rouletteCoins = new int[12] { 1, 2, 2, 1, 2, 5, 1, 2, 2, 1, 2, 2};


    //Cnt를 내가 서버에 얼마가지고 있다 정보와 값보다 적으면 시도할수 없는 시스템메세지를 넣으면 되겠다

    public void Insert()
    {
        Blind.SetActive(false);
        rsp = 0;
        input = -1;
        rouletteNum = 0;
        for (int i = 0; i < Roullettes.Length; i++)
            Roullettes[i].SetActive(false);

        StartCoroutine(RSPLoop());
    }

    void End()
    {
        InsertBtn.SetActive(true);
        CoinText.gameObject.SetActive(true);
    }

    IEnumerator ShowLog(string log)
    {
        LogText.text = log;
        yield return new WaitForSeconds(1);
        LogText.text = "";
    }

    void ShowCoin()
    {
        CoinText.text = myCoin.ToString();
    }

    public void MinusCoin()
    {
        if (myCoin > 1) --myCoin;
        ShowCoin();
    }


    void RSPWinner()
    {
        if (rsp == input) // 비김
        {
            //비겨도 돈날리는거입니다
            //Invoke("Insert", 1);
            End();
            StartCoroutine(ShowLog("비겼다"));
        }

        else if ((rsp == 0 && input == 2) || (rsp == 1 && input == 0) || (rsp == 2 && input == 1)) // 이김
        {
            StartCoroutine(RouletteSpin());
            StartCoroutine(ShowLog("이겼다!"));
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
        }

        int curCoin = rouletteCoins[rouletteNum];
        myCoin += curCoin;
        ShowCoin();
        StartCoroutine(ShowLog(curCoin + " 얻었다"));
        End();
    }

}
