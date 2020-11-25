using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GoogleChatManager : MonoBehaviour {


    //각각 매장마다 단순 URL 주소를 입력 변경
    const string URL = "";
    const string WebURL = "";

    public Text ChatText;
    public InputField NicknameInput, Chatinput;


    private void Start()
    {
        StartCoroutine(Get());
    }


    IEnumerator Get()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        ChatText.text = data;

        StartCoroutine(Get());

    }

    public void ChatPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", NicknameInput.text);
        form.AddField("chat", Chatinput.text);

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using(UnityWebRequest www = UnityWebRequest.Post(WebURL, form)) //반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
        }
    }
}
