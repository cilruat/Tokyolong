using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    public Slider tabSlider;
    public RectTransform[] BtnRect;
    public RectTransform[] BtnImageRect;

    const int SIZE = 3;
    float[] pos = new float[SIZE];
    float distance;
    float targetPos;
    float curPos;
    bool isDrag;
    int targetIndex;

    void Start () {

        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;

	}

    float SetPos()
    {
        //절반 거리를 기준으로 가까운 위치를 반환한다
        for(int i = 0; i < SIZE; i++)
            //기획한것을 코드로 옮길수 있는가 기본적인 예시
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }


    //메서드 씩 =>  괄호안에것을 없앰
    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = SetPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        targetPos = SetPos();


        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if(curPos == targetPos)
        {
            //print(eventData.delta.x);
            
            // 스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
            if(eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // 스크롤이 오른쪽으로 빠르게 이동시 목표가 하나 증가
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        //목표가 수직스크롤이고, 옆에서 옮겨왔다면 수직스크롤을 맨 위로 올림
        for (int i = 0; i < SIZE; i++)
            if (contentTr.GetChild(i).GetComponent<OrderScroll>() && curPos != pos[i] && targetPos == pos[i])
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;

    }


    void Update ()
    {
        tabSlider.value = scrollbar.value;
        if (!isDrag)

        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
            for (int i = 0; i < SIZE; i++) BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 200 : 100, BtnRect[i].sizeDelta.y);
        }

        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            if(i == targetIndex)
            {
                BtnTargetPos.y = -5f;
                BtnTargetScale = new Vector3(1.3f, 1.3f, 1);
                textActive = true;
            }


            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }


	}

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];

    }
}
