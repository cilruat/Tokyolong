using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {

    private string question;
    private List<string> answerList;

    public GameObject objChoicePanel;
    public Text question_Text;
    public Text[] answer_Text;
    public GameObject[] answerPanel;

    public Animator anim;
    public bool choiceIng;

    private int count; // 배열의 크기
    private int result; // 선택한 선택창


    private void Start()
    {
        answerList = new List<string>();
        for (int i = 0; i <= answer_Text.Length; i++)
        {
            answer_Text[i].text = "";
            answerPanel[i].SetActive(false);
        }
        question_Text.text = "";

    }

    public void ShowChoice(ChoiceData _choice)
    {
        objChoicePanel.SetActive(true);
        result = 0;
        question = _choice.qeustionName;

        for(int i = 0; i < _choice.questAnswers.Length; i++)
        {
            answerList.Add(_choice.questAnswers[i]);
            count = i;
        }
        anim.SetBool("Appear", true);
    }

    public int GetResult()
    {
        Debug.Log(result);
        return result;
    }


    public void ExitChoice()
    {
        question_Text.text = "";

        for (int i = 0; i < count; i++)
        {
            answer_Text[i].text = "";
            answerPanel[i].SetActive(false);
        }
        anim.SetBool("Appear", false);
        choiceIng = false;
        objChoicePanel.SetActive(false);

    }


    #region //타이핑이펙트, 미적용
    //이름이 헷갈리는게 많은데 적용안되면 이게 틀린거
    /*
     * 
     *
    IEnumerator ChoiceCorutine()
    {
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(TypingQuestion());
        StartCoroutine(TypingAnswer_0());

        if(count >= 1)
            StartCoroutine(TypingAnswer_1());
        if (count >= 2)
            StartCoroutine(TypingAnswer_2());
        if (count >= 3)
            StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.5f);


    }


    IEnumerator TypingQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < answerList[0].Length; i++)
        {
            answer_Text[0].text += answerList[0][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answerList[1].Length; i++)
        {
            answer_Text[1].text += answerList[1][i];
            yield return waitTime;
        }
    }


    IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < answerList[2].Length; i++)
        {
            answer_Text[2].text += answerList[2][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < answerList[3].Length; i++)
        {
            answer_Text[3].text += answerList[3][i];
            yield return waitTime;
        }
    }
    */
    #endregion


}
