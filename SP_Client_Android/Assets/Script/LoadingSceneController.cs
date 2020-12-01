using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour {

    static string nextScene;


    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;


        SceneManager.LoadScene("Loading");
    }




	// Use this for initialization
	void Start () {

        StartCoroutine(LoadSceneProcess());

	}
	

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //Async 비동기방식으로 다른씬불러오는중 다른작업가능한 함수, AsyncOpertation 으로 반환
        op.allowSceneActivation = false; //씬을 비동기로 불러들일때 씬의 로딩이 끝나면 자동으로 불러들일것인지 설정

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else //90% 채워지면 나머지 10%는 1초간 채운뒤 씬을 불러온다
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }


}
