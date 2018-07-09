using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicRequestor : MonoBehaviour 
{
    public InputField inputTitle;
    public InputField inputSinger;

    public void OnSendMusicRequest()
    {
        if (string.IsNullOrEmpty(inputTitle.text))
        {
            SystemMessage.Instance.Add("노래의 제목을 알려 주세요");
            return;
        }

        if (string.IsNullOrEmpty(inputSinger.text))
        {
            SystemMessage.Instance.Add("가수의 성함을 알려 주세요");
            return;
        }

        NetworkManager.Instance.Request_Music_REQ(inputTitle.text, inputSinger.text);

        inputTitle.text = string.Empty;
        inputSinger.text = string.Empty;
        this.gameObject.SetActive(false);
    }

    public void OnClose()
    {
        this.gameObject.SetActive(false);
    }
}
