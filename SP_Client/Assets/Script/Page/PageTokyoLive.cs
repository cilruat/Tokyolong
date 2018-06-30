using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTokyoLive : MonoBehaviour {
   
    public Text txtQuesiton;
    public Text[] txtChoice;
    public GameObject[] objChoice;

    public CountDown countDown;
    public Image imgTime;
    public GameObject objTime;

    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

    const int LIMIT_TIME = 10;

    string[] question1 = { "", "", "", "" };
    string[] question2 = { "", "", "", "" };

    int answer1 = 0;
    int answer2 = 0;

    void Awake()
    {
        string path = Application.dataPath;
        int lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\TokyoLive_QuestionBook.csv";

        data = CSVReader.Read(path);

        _RandQuestion(ref question1, ref answer1);
        _RandQuestion(ref question2, ref answer2);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        UITweenAlpha.Start(gameObject, 0f, 1f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));

        yield return new WaitForSeconds(.5f);

        string str = "";
        char[] ch = question1[0].ToCharArray();
        for (int i = 0; i < ch.Length; i++)
        {
            str += ch[i].ToString();
            txtQuesiton.text = str;
            yield return new WaitForSeconds(.01f);
        }

        txtQuesiton.text = question1[0];

        for (int i = 0; i < objChoice.Length; i++)
        {
            txtChoice[i].text = question1[i + 1];
            float delay = i * .2f;
            UITweenAlpha.Start(objChoice[i], 0f, 1f, TWParam.New(.8f, delay).Curve(TWCurve.CurveLevel2));
        }

        objTime.SetActive(true);
        UITweenScale.Start(objTime, 1f, 1.1f, TWParam.New(.5f, .7f).Curve(TWCurve.CurveLevel2).Loop(TWLoop.PingPong));
        countDown.Set(LIMIT_TIME, () => _Finish());

        while (true)
        {
            float elapsed = countDown.GetElapsed();
            float fill = (LIMIT_TIME - elapsed) / LIMIT_TIME;
            imgTime.fillAmount = fill; 

            if (fill <= 0)  break;
            yield return null;
        }

        objTime.SetActive(false);
    }

    void _Finish()
    {
        objTime.SetActive(false);
    }

    void _RandQuestion(ref string[] question, ref int answer)
    {
        int rand = Random.Range(0, data.Count);
        if (rand >= data.Count)
            rand -= (rand - data.Count + 1);

        Dictionary<string, object> randQuestion = data[rand];

        int idx = 0;
        foreach (object obj in randQuestion.Values)
        {
            if (idx == randQuestion.Values.Count - 1)
            {
                string s = string.Concat(obj);
                answer = int.Parse(s);
            }
            else
                question[idx] = string.Concat(obj);
            ++idx;
        }

        data.RemoveAt(rand);
    }
}
