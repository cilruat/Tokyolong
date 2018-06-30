using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTableStatus : PageBase {

    public enum EFloor
    {
        Floor1 = 0,
        Floor2,
    }

    public enum ETableDetail
    {
        Like = 0,
        Chat,
        Gift,
        Battle,
    }

    const int FLOOR_MAX_TABLE_1 = 7;
    const int FLOOR_MAX_TABLE_2 = 8;

    public CanvasGroup[] cgBoards;

    public List<GameObject> floors = new List<GameObject>();
    public List<TableSpotElt> tableSpots = new List<TableSpotElt>();

    public RectTransform rtFloor;
    public RectTransform rtDetail;
    public RectTransform rtArrow;

    public Texture[] imgCustomerSpot;

    EFloor curViewFloor = EFloor.Floor1;
    int curViewFloorIdx { get { return (int)curViewFloor; } }

    int startTableIdx { get { return curViewFloor == EFloor.Floor1 ? 0 : FLOOR_MAX_TABLE_1; } }
    int endTableIdx { get { return curViewFloor == EFloor.Floor1 ? FLOOR_MAX_TABLE_1 : FLOOR_MAX_TABLE_1 + FLOOR_MAX_TABLE_2; } }

    int selectTableSpot = -1;

    protected override void Awake ()
	{
        this.boards = cgBoards;
		base.Awake ();
	}

    void Start()
    {
        StartCoroutine(ViewTableSpot());
    }

    public void OnSelectFloor(int floor)
    {
        if (curViewFloorIdx == floor)
            return;
        
        floors[curViewFloorIdx].gameObject.SetActive(false);
        for (int i = startTableIdx; i < endTableIdx; i++)
            tableSpots[i].iconSpot.gameObject.SetActive(false);

        floors[floor].gameObject.SetActive(true);
        curViewFloor = (EFloor)floor;
        OnDetailClose();

        StopAllCoroutines();
        StartCoroutine(ViewTableSpot());
    }

    public void OnSelectSpot(int i)
    {
        if (tableSpots[i].tableNo == Info.TableNum)
        {
            SystemMessage.Instance.Add("현재 테이블입니다");
            return;
        }

        if (tableSpots[i].IsNone)
        {
            SystemMessage.Instance.Add("아직 입석하지 않은 테이블 입니다");
            return;
        }

        if (detailDisableTween != null)
        {
            detailDisableTween.StopTween();
            detailDisableTween = null;
        }

        selectTableSpot = i;

        RectTransform rtTableSpot = tableSpots[i].GetComponent<RectTransform>();
        float anchorX = (rtTableSpot.anchoredPosition.x + (rtFloor.rect.width * .5f)) / rtFloor.rect.width;
        float anchorY = (rtTableSpot.anchoredPosition.y + (rtFloor.rect.height * .5f)) / rtFloor.rect.height;
        rtDetail.anchorMin = new Vector2(anchorX, anchorY);
        rtDetail.anchorMax = new Vector2(anchorX, anchorY);

        float detailWidth = rtDetail.rect.width + (rtArrow.rect.width * .6f);
        float marginLeft    = (rtFloor.rect.width * rtDetail.anchorMin.x) - ((rtTableSpot.rect.width * .5f) + detailWidth);
        float marginRight   = (rtFloor.rect.width * (1f - rtDetail.anchorMin.x)) - ((rtTableSpot.rect.width * .5f) + detailWidth);

        float detailPosX = 0f;
        float arrowPosX = 0f;
        float arrowAngle = 0f;
        float dir = 0f;

        if (marginRight > 0)
        {
            dir = Vector2.right.x;
            detailPosX = dir * ((rtDetail.rect.width * .5f) + (rtArrow.rect.width * .6f) + (rtTableSpot.rect.width * .5f));
            arrowPosX = (dir*-1f) * ((rtDetail.rect.width * .5f) + 11.5f);
            arrowAngle = -90f;
        }
        else if(marginLeft > 0)
        {
            dir = Vector2.left.x;
            detailPosX = dir * ((rtDetail.rect.width * .5f) + (rtArrow.rect.width * .6f) + (rtTableSpot.rect.width * .5f));
            arrowPosX = (dir*-1f) * ((rtDetail.rect.width * .5f) + 11.5f);
            arrowAngle = 90f;
        }

        rtDetail.anchoredPosition = new Vector2(detailPosX, 0f);
        rtArrow.anchoredPosition = new Vector2(arrowPosX, 0f);
        rtArrow.eulerAngles = new Vector3(0f, 0f, arrowAngle);

        float startX = detailPosX + (-dir * 20f);
        UITweenPosX.Start(rtDetail.gameObject, startX, detailPosX, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));
        UITweenAlpha.Start(rtDetail.gameObject, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.Linear).Speed(TWSpeed.Slower));
    }

    IEnumerator ViewTableSpot()
    {
        for (int i = startTableIdx; i < endTableIdx; i++)
        {
			TableSpotElt elt = tableSpots [i];
			UserInfo info = Info.GetUser (elt.tableNo);
            byte customer = info != null ? info.customerType : (byte)TableSpotElt.ESpotType.eNone;
            elt.SetTableSpot (customer);
            yield return new WaitForSeconds(.15f);
        }

        yield return null;
    }

    UITween detailDisableTween = null;
    public void OnDetailClose()
    {
        if (detailDisableTween != null && detailDisableTween.IsTweening())
            return;

        if (rtDetail.gameObject.activeSelf == false)
            return;

        UITweenPosY.Start(rtDetail.gameObject, rtDetail.anchoredPosition.y, rtDetail.anchoredPosition.y + 20f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));
        detailDisableTween = UITweenAlpha.Start(rtDetail.gameObject, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.Linear).Speed(TWSpeed.Slower).DisableOnFinish());
    }

    public void OnSelectDetail(int i)
    {
        if (selectTableSpot == -1)
        {
            SystemMessage.Instance.Add ("대상 테이블을 선택해 주세요");
            return;
        }

        ETableDetail detailType = (ETableDetail)i;
        switch (detailType)
        {
            case ETableDetail.Like:
            case ETableDetail.Gift:
            case ETableDetail.Battle:
                SystemMessage.Instance.Add ("현재 기능은 미구현 상태입니다");
                break;
            case ETableDetail.Chat:
                GameObject obj = UIManager.Instance.Show(eUI.eChat);
                UIChat uiChat = obj.GetComponent<UIChat>();
                uiChat.AddTableChat(tableSpots[selectTableSpot].tableNo);
                break;
        }
    }
}
