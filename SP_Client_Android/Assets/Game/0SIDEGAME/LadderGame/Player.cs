using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameManager GM;
    public int finishNum;
    public bool isEnd;

    bool isHorizontal;
    Vector2 dir;


    // PlayerCs를 초기화한다
    public void Clear()
    {
        GetComponent<Button>().interactable = true;
        isEnd = false;
    }


    public IEnumerator Move()
    {
        // 이미 내려갔는데 전체결과로 또 불러오면 안되기 때문에
        if (isEnd) yield break;
        finishNum = 0;
        isHorizontal = false;
        dir = Vector2.down;
        GM.PlayersGl.enabled = false;
        GetComponent<Button>().interactable = false;

        //transform.GetChild(2).gameObject.SetActive(true);
        GM.BlindPanel.SetActive(true);
        RectTransform Rt = GetComponent<RectTransform>();
        while(Rt.anchoredPosition.y > -415)
        {
            transform.Translate(dir * 50 * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        //transform.GetChild(2).gameObject.SetActive(false);
        GM.BlindPanel.SetActive(false);
    }


    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        switch (col.name)
        {
            case "Left": case "Right":
                yield return new WaitForSeconds(0.001f);
                if (isHorizontal)
                {
                    isHorizontal = false;
                    dir = Vector2.down;
                }
                else
                {
                    isHorizontal = true;
                    dir = (col.name == "Left") ? Vector2.right : Vector2.left;
                }
                break;

            case "1": case "2": case "3": case "4": case "5": case "6":
                finishNum = int.Parse(col.name);
                isEnd = true;
                break;
        }
    }
}
