using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {

    public Slider progressbar;
    public Text loadText;

    //public GameObject[] objText;

    public string sceneName;

    //스크립트 하나로밖에 줄일수있는실력밖에 안되넹..씬을 로딩으로들어와서 뿌려주는 if를 못쓰겟다 부족해서 바로할수있는실력이안됨,.일단 3개씬을만들어서 등록합니다 ㅠㅠ

    private void Start()
    {
        //objText[0].SetActive(false);
        //objText[1].SetActive(false);
        StartCoroutine(LoadScene(sceneName));
    }


    IEnumerator LoadScene(string sceneName)
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 씬끝나면 로딩멈추는것


        while(!operation.isDone)
        {
            yield return null;
            if(progressbar.value <1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                //objText[0].SetActive(true);
            }
            else
            {
                loadText.text = "완료!! 어여오세요~";
                //objText[0].SetActive(false);
                //objText[1].SetActive(true);
            }

            if (progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }


}
