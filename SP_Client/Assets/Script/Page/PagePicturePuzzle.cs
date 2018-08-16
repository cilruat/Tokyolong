using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagePicturePuzzle : SingletonMonobehaviour<PagePicturePuzzle> {

	class Clip
	{
		int col;
		int row;
		RawImage img;
		Button btn;
		RectTransform rt;
		Vector2 ancPos;
		PagePicturePuzzle page;

		public Clip(int col, int row, RawImage img,
			Button btn, RectTransform rt, PagePicturePuzzle page)
		{
			this.col = col;
			this.row = row;
			this.img = img;
			this.btn = btn;
			this.rt = rt;
			this.page = page;

			btn.onClick.RemoveAllListeners ();
			btn.onClick.AddListener (() => page._OnClick (col, row));
		}			

		public void SetTex(Texture tex, Vector2 vPos, Vector2 vSize)
		{
			img.texture = tex;
			img.uvRect = new Rect (vPos, vSize);
		}

		public void RefreshPos(Vector2 pos, int col, int row)
		{
			this.col = col;
			this.row = row;
			ancPos = pos;

			rt.anchoredPosition = pos;

			btn.onClick.RemoveAllListeners ();
			btn.onClick.AddListener (() => page._OnClick (col, row));
		}

		public bool Compare(int col, int row)
		{
			return this.col == col && this.row == row;
		}

		public void SetPos(Vector2 pos)	{ ancPos = pos; }
		public Vector2 GetPos() { return ancPos; }
		public RawImage GetImage() { return img; }
	}

    const int EASY_MODE = 3;
    const int HARD_MODE = 4;
    const int LIMIT_TIME = 20;

	public Text txtCountDown;
	public CountDown countDown;
	public Image imgTime;
    public RawImage img;
	public CanvasGroup cgImg;
    public RawImage imgPreview;
	public GridLayoutGroup grid;
	public GameObject objClip;
	public GameObject objBoard;
	public GameObject objSendServer;
	public GameObject objHide;
	public GameObject objBtnStart;
	public GameObject objTxtReady;
	public GameObject objTxtGo;
	public GameObject objGameOver;

	bool start = false;
	bool end = false;
	bool startBtnClick = false;
	int mode = 0;
    int[,] array;
	List<Clip> listClip = new List<Clip>();

	void Start()
	{		
		_Init();
		txtCountDown.text = LIMIT_TIME.ToString ();
	}		

	void Update()
	{
		if (start == false || end)
			return;

		float elapsed = countDown.GetElapsed ();
		float fill = (LIMIT_TIME - elapsed) / (float)LIMIT_TIME;
		imgTime.fillAmount = fill;
	}

    void _Init()
    {
		start = false;
		end = false;
		startBtnClick = false;

		listClip.Clear();
		for (int i = 0; i < grid.transform.childCount; i++) {
			Transform child = grid.transform.GetChild (i);
			if (child)
				Destroy (child.gameObject);
		}

		mode = EASY_MODE; //Info.GameDiscountWon == 0 ? EASY_MODE : HARD_MODE;
        array = new int[mode, mode];

		float cellSize = img.rectTransform.rect.height / (float)mode;
		grid.cellSize = new Vector2 (cellSize - 2f, cellSize - 2f);
		grid.constraintCount = mode;

        _SetImage();

        for (int i = 0; i < mode; i++)
            for (int j = 0; j < mode; j++)
                array[i, j] = i * mode + j;

        array[mode - 1, mode - 1] = -1;
    }

    void _SetImage()
    {
        int rand = UnityEngine.Random.Range(1, 2);
		Texture tex = Resources.Load("PicturePuzzle/Puzzle" + rand.ToString()) as Texture;

        img.texture = tex;
        imgPreview.texture = tex;
    }

	public void OnStart()
	{
		if (startBtnClick)
			return;

		startBtnClick = true;
		StartCoroutine (_ReadyGo ());
	}

	IEnumerator _ReadyGo()
	{
		UITweenAlpha.Start(objBtnStart, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		UITweenAlpha.Start(grid.gameObject, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		UITweenAlpha.Start(objTxtReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		UITweenAlpha.Start(img.gameObject, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		_ClopClip (img.texture);
		yield return StartCoroutine (_SetUpImage ());
		_MixClip ();

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(grid.gameObject, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.5f);
		UITweenAlpha.Start(objTxtReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.25f);
		UITweenAlpha.Start(objTxtGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objTxtGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.3f);

		objHide.SetActive (false);
		grid.enabled = false;

		start = true;
		countDown.Set (LIMIT_TIME, () => _FailEndGame ());
	}

	void _ClopClip(Texture tex)
	{
		grid.enabled = true;
		float size = 1f / (float)mode;

		int idx = 0;
		int total = mode * mode;
		for (int i = 0; i < mode; i++) {
			for (int j = 0; j < mode; j++) {
				GameObject obj = Instantiate (objClip) as GameObject;
				obj.name = "Clip[" + i.ToString () + ", " + j.ToString () + "]";

				RectTransform rt = (RectTransform)obj.transform;
				rt.SetParent (grid.transform);
				rt.InitTransform ();

				Clip clip = new Clip (i, j,
					obj.GetComponent<RawImage> (),
					obj.GetComponent<Button>(),
					rt, this);

				float x = j * size;
				float y = (mode - 1 - i) * size;
				Vector2 vPos = new Vector2 (x, y);
				Vector2 vSize = new Vector2 (size, size);

				Texture t = (total - 1 > idx) ? tex : null;
				clip.SetTex (t, vPos, vSize);

				obj.SetActive (true);
				idx++;

				listClip.Add (clip);
			}
		}			
    }

	IEnumerator _SetUpImage()
	{
		yield return null;

		for (int i = 0; i < grid.transform.childCount; i++) {
			RectTransform rt = (RectTransform)grid.transform.GetChild (i);
			if (rt == null)
				continue;

			listClip [i].SetPos (rt.anchoredPosition);
		}			
	}

    void _MixClip()
    {
        int row, col;
        int emptyRow = -1;
        int emptyCol = -1;

        System.Random rand = new System.Random();

        for (int i = 0; i < 1000; i++)
        {
			row = rand.Next (mode);
			col = rand.Next (mode);

            if (_FindEmptyIdx(row, col, ref emptyRow, ref emptyCol))
                _MoveImg(row, col, emptyRow, emptyCol);
        }
    }          

    bool _FindEmptyIdx(int row, int col, ref int emptyRow, ref int emptyCol)
    {		
		for (int i = 0; i < mode; i++) {
			for (int j = 0; j < mode; j++) {
				if (array [i, j] == -1) {
					emptyCol = i;
					emptyRow = j;
					break;
				}
			}            
		}

		int calcCol = Mathf.Abs (col - emptyCol);
		int calcRow = Mathf.Abs (row - emptyRow);
		if (calcCol + calcRow == 1)
			return true;

        return false;
    }
    
    void _MoveImg(int row, int col, int emptyRow, int emptyCol)
    {
		int idx = -1;
		int emptyIdx = -1;
		for (int i = 0; i < listClip.Count; i++) {
			if (listClip [i].Compare (col, row))				idx = i;
			if (listClip [i].Compare (emptyCol, emptyRow))		emptyIdx = i;
		}

		Vector2 pos = listClip [idx].GetPos ();
		listClip [idx].RefreshPos (listClip [emptyIdx].GetPos (), emptyCol, emptyRow);
		listClip [emptyIdx].RefreshPos (pos, col, row);

		int temp = array [col, row];
		array [col, row] = array [emptyCol, emptyRow];
		array [emptyCol, emptyRow] = temp;
    }

	void _OnClick(int col, int row)
	{
		if (start == false || end)
			return;

		int emptyRow = -1;
		int emptyCol = -1;

		if (_FindEmptyIdx (row, col, ref emptyRow, ref emptyCol)) {
			_MoveImg (row, col, emptyRow, emptyCol);

			_CheckEndGame ();
		}
	}

	void _CheckEndGame()
	{
		bool finish = true;
		int calc = mode * mode;
		for (int i = 0; i < mode; i++) {
			for (int j = 0; j < mode; j++) {
				if (i == mode - 1 && j == mode - 1) {
					if (array [i, j] != -1)
						finish = false;
				} else {
					if (array [i, j] != i * mode + j)
						finish = false;
				}
			}
		}

		if (finish) {
			end = true;
			countDown.Stop ();
			StartCoroutine (_SuccessEndGame ());
		}
	}

	IEnumerator _SuccessEndGame()
	{
		float sec = 1f / (float)(mode * mode);
		for (int i = 0; i < listClip.Count; i++) {
			ShiningGraphic.Start (listClip [i].GetImage ());
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (.25f);

		UITweenAlpha.Start (grid.gameObject, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.25f);

		UITweenAlpha.Start (img.gameObject, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		ShiningGraphic.Start (img);
		yield return new WaitForSeconds (1f);

		objSendServer.SetActive (true);
		yield return new WaitForSeconds (1f);

		NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}

	void _FailEndGame()
	{
		objGameOver.SetActive (true);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Game", objBoard);
	}
}