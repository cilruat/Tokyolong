using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HitAndMissScrachCard : MonoBehaviour {

    public GameObject objBoard;
    public GameObject objReSetBtn;
    public GameObject objImagePanel;

    public GameObject AnimationPanel;
    public GameObject BlindPanel;

    public List<Image> ImageList = new List<Image>();


    private void Start()
    {
        objReSetBtn.SetActive(false);
        AnimationPanel.SetActive(false);
        BlindPanel.SetActive(false);

    }


    //startbutton OnClick
    public void OnRandom()
    {
        AnimationPanel.SetActive(true);

        for (int i = 0; i < 1; i++)
        {
            if (ImageList.Count != 0)
            {
                int rand = Random.Range(0, ImageList.Count);
                print(ImageList[rand]);
                Image WithOneMouthImg = Instantiate(ImageList[i], gameObject.transform);
                WithOneMouthImg = ImageList[rand];
                //ImageList.RemoveAt(rand);
                //StartCoroutine(OnDestroyImg(WithOneMouthImg.gameObject, 2f));
                //WithOneMouthImg.gameObject.SetActive(true);
                //Invoke("OnDestroy", 3f);
            }
            else
            {
                objReSetBtn.SetActive(true);
                BlindPanel.SetActive(true);
            }
        }
    }

    IEnumerator OnDestroyImg(GameObject WithOneMouthImg, float second)
    {
        WithOneMouthImg.SetActive(true);

        yield return new WaitForSeconds(3.15f);
        WithOneMouthImg.SetActive(false);
    }

    public void Reset()
    {
        SceneChanger.LoadScene("InssaWithOneMouth", objBoard);
    }

    public void GoHome()
    {
        SceneChanger.LoadScene("InSsa", objBoard);
    }
}
