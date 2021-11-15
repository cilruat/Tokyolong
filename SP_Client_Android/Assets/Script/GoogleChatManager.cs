using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GoogleChatManager : MonoBehaviour {


    // 영남대
    //https://docs.google.com/spreadsheets/d/1osVtywsdEM9J4-w8Dhs_Gm0bBhYB1fCAR40LK23WlGg/export?format=tsv&range=B:B
    //https://script.google.com/macros/s/AKfycbxIf7mzSmD-B0QXe-XlA-Tt9VkiuflhEElj1p3EKgeFK32aoUN-/exec


    //각각 매장마다 단순 URL 주소를 입력 변경
    const string URL = "https://docs.google.com/spreadsheets/d/1osVtywsdEM9J4-w8Dhs_Gm0bBhYB1fCAR40LK23WlGg/export?format=tsv&range=B:B";

    //구글 스프레드시트 코드 - 게시 -웹배포 URL 
    const string WebURL = "https://script.google.com/macros/s/AKfycbxIf7mzSmD-B0QXe-XlA-Tt9VkiuflhEElj1p3EKgeFK32aoUN-/exec";


    public Text ChatText;
    public InputField NicknameInput, ChatInput;

    public GameObject objChatBtn;

    public Animator anim;


    void Start()
    {
        StartCoroutine(Get());
        NicknameInput.text = Info.TableNum.ToString();
        objChatBtn.SetActive(true);
    }


    IEnumerator Get()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        ChatText.text = data;

        StartCoroutine(Get());
    }


    public void ShowChatPanel()
    {
        anim.SetBool("Appear", true);
        objChatBtn.SetActive(false);

    }

    public void CloseChatPanel()
    {
        anim.SetBool("Appear", false);
        objChatBtn.SetActive(true);
    }


    public void ChatPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", NicknameInput.text);
        form.AddField("chat", ChatInput.text);
        StartCoroutine(Post(form));
    }


    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebURL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
        }
    }
}