using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SampleWebView : MonoBehaviour {

	[Header("UI")]
	public InputField inputUrl;
	public InputField inputWidth;
	public InputField inputHeight;

	public Text inputcurrent;
	public Text inputlast;

	public Text txthtml;

	[Header("Variable")]
	public string url;
	public int width;
	public int height;

	[Header("CallBack Url")]
	public string currenturl;
	public string lasturl;
	public string htmlcode;

    public GameObject objCanvas;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void BtnFullWeb()
	{
		url = inputUrl.text;

		Debug.Log ("Full Screen WebView");
		PopupWebView.FullWebView(url);


	}

    public void BtnYouTube()
    {
        url = "http://www.youtube.com";
        PopupWebView.FullWebView(url);
    }

    public void BtnNaver()
    {
        url = "http://www.naver.com";
        PopupWebView.FullWebView(url);
    }



    public void BtnLastCallFullWeb()
	{
		lasturl = inputlast.text;
		
		Debug.Log ("Full Screen WebView");
		PopupWebView.FullWebView(lasturl);			
	}

	public void BtnCustomWeb()
	{
		url = inputUrl.text;
		width = int.Parse(inputWidth.text);
		height =  int.Parse(inputHeight.text);

		Debug.Log("Custom Screen WebView");
		PopupWebView.CustomWebView(url,false,width,height);

	}

    public void BtYoutubeCustom()
    {
        url = "http://www.youtube.com";
        width = int.Parse(inputWidth.text);
        height = int.Parse(inputHeight.text);
        PopupWebView.CustomWebView(url, false, width, height);
    }



    public void BtNaverCustom()
    {
        url = "http://www.naver.com";
        width = int.Parse(inputWidth.text);
        height = int.Parse(inputHeight.text);
        PopupWebView.CustomWebView(url, false, width, height);
    }





    public void BtnLastCustomWeb()
	{
		lasturl = inputlast.text;
		width = int.Parse(inputWidth.text);
		height =  int.Parse(inputHeight.text);
		
		Debug.Log("Custom Screen WebView");
		PopupWebView.CustomWebView(lasturl,false,width,height);
		
	}

	/// <summary>
	/// Currents the view URL.
	/// call back from mobile
	/// </summary>
	/// <param name="url">URL.</param>
	public void CurrentViewUrl(string viewurl)
	{
		currenturl = viewurl;
		inputcurrent.text = currenturl;
	}

	/// <summary>
	/// Lasts the view URL.
	/// callback from mobile
	/// </summary>
	/// <param name="url">URL.</param>
	public void LastViewUrl(string viewurl)
	{
		lasturl = viewurl;
		inputlast.text = lasturl;

	}

	/// <summary>
	/// Current View Html Code 
	/// callback from mobile
	/// </summary>
	/// <param name="url">URL.</param>
	public void CurrentHtmlCode(string html)
	{
		htmlcode = html;
		txthtml.text = html;
		
	}

    public void OnGoHome()
    {
        SceneChanger.LoadScene("Main", objCanvas);
    }


}
