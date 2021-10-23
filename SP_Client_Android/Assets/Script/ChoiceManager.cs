using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {

    public GameObject objChoicePanel;

    public Text sort_Text;
    public Text question_Text;
    public Text answer_Text;
    public Text reject_Text;

    public GameObject[] answerPanel;

    public DialogueManager dialogueManager;

    public Animator anim;

    private void Start()
    {
        sort_Text.text = "";
        question_Text.text = "";
        answer_Text.text = "";
        reject_Text.text = "";
    }

    public void ShowChoice(NPCAction _choice)
    {
        objChoicePanel.SetActive(true);

        sort_Text.text = _choice.questionSort;
        question_Text.text = _choice.questionName;
        answer_Text.text = _choice.questAnswers;
        reject_Text.text = _choice.questReject;
        anim.SetBool("Appear", true);
    }


    public void OnGoScene()
    {
        anim.SetBool("Appear", false);
        StartCoroutine(DelayChoicePanel());
        NPCAction nPC = dialogueManager.scanObject.GetComponent<NPCAction>();
        nPC.SceneMove();
    }

    public void ExitChoice()
    {
        sort_Text.text = "";
        question_Text.text = "";
        answer_Text.text = "";
        reject_Text.text = "";

        anim.SetBool("Appear", false);
        StartCoroutine(DelayChoicePanel());
    }


    IEnumerator DelayChoicePanel()
    {
        dialogueManager.isAction = false;
        yield return new WaitForSeconds(0.1f);
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
