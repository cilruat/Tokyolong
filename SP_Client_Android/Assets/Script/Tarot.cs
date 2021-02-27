using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tarot : MonoBehaviour {

    public GameObject objFrontCard;
    public GameObject objBackCard;
    public GameObject objText;

    // 연출 추가, 카드의 이름, 주제등을 더 넣어야겟다...하 ㅠㅠ 연출힘드렁

    private void Start()
    {
        objFrontCard.SetActive(true);
        objBackCard.SetActive(false);
        objText.SetActive(false);
    }

    public void OnClickTaroCard()
    {
        objBackCard.SetActive(true);
        StartCoroutine(_ShowDesc());
    }


    IEnumerator _ShowDesc()
    {
        //여기에 카드의 이미지나 위치나 그런것들을 추가하면 된다
        yield return new WaitForSeconds(.8f);
        objText.SetActive(true);
        UITweenAlpha.Start(objText, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        //UITweenPosY.Start(objText, 0f, 30f, TWParam.New(1f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
    }


}
