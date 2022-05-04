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
		GameObject sel;
		Button btn;
		RectTransform rt;
		Vector2 ancPos;
		PagePicturePuzzle page;

		public Clip(int col, int row, RawImage img, GameObject sel,
			Button btn, RectTransform rt, PagePicturePuzzle page)
		{
			this.col = col;
			this.row = row;
			this.img = img;
			this.sel = sel;
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
			sel.SetActive (false);
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

		public void Select(bool isSelect) { sel.SetActive (isSelect); }
		public bool Compare(int col, int row) { return this.col == col && this.row == row; }

		public void SetPos(Vector2 pos)	{ ancPos = pos; }
		public Vector2 GetPos() { return ancPos; }
		public RawImage GetImage() { return img; }
	}
	   
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
	public GameObject objQuit;

	bool start = false;
	bool end = false;
	bool startBtnClick = false;
	int mode = 0;	

	int selRow = -1, selCol = -1;
    int[,] array;
	List<Clip> listClip = new List<Clip>();

	void Start()
	{		
		_Init();
		txtCountDown.text = Info.practiceGame ? "∞" : Info.PICTURE_PUZZLE_LIMIT_TIME.ToString ();

		objQuit.SetActive (Info.practiceGame);
	}		

	void Update()
	{
		if (start == false || end)
			return;

		if (Info.practiceGame)
			return;

		float elapsed = countDown.GetElapsed ();
		float fill = (Info.PICTURE_PUZZLE_LIMIT_TIME - elapsed) / (float)Info.PICTURE_PUZZLE_LIMIT_TIME;
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

		mode = Info.PICTURE_PUZZLE_MODE;
        array = new int[mode, mode];

		float cellSize = img.rectTransform.rect.height / (float)mode;
		grid.cellSize = new Vector2 (cellSize - 2f, cellSize - 2f);
		grid.constraintCount = mode;

        _SetImage();

        for (int i = 0; i < mode; i++)
            for (int j = 0; j < mode; j++)
                array[i, j] = i * mode + j;
    }

    void _SetImage()
    {
        int rand = UnityEngine.Random.Range(1, 37);
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

		if (Info.practiceGame == false)
			countDown.Set (Info.PICTURE_PUZZLE_LIMIT_TIME, () => _FailEndGame ());
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
					            rt.Find ("Select").gameObject,
					            obj.GetComponent<Button> (),
					            rt, this);

				float x = j * size;
				float y = (mode - 1 - i) * size;
				Vector2 vPos = new Vector2 (x, y);
				Vector2 vSize = new Vector2 (size, size);

				Texture t = (total - 1 >= idx) ? tex : null;
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
        int moveRow = -1;
        int moveCol = -1;

        System.Random rand = new System.Random();

        for (int i = 0; i < 100; i++)
        {
			row = rand.Next (mode);
			col = rand.Next (mode);

			moveRow = rand.Next (mode);
			moveCol = rand.Next (mode);
			if (row == moveRow && col == moveCol) {
				while (true) {
					moveRow = rand.Next (mode);
					moveCol = rand.Next (mode);

					if (row != moveRow || col != moveCol)
						break;
				}
			}

            _MoveImg(row, col, moveRow, moveCol);
        }
    }
    
    void _MoveImg(int row, int col, int moveRow, int moveCol)
    {
		int idx = -1;
		int moveIdx = -1;
		for (int i = 0; i < listClip.Count; i++) {
			if (listClip [i].Compare (col, row))				idx = i;
			if (listClip [i].Compare (moveCol, moveRow))		moveIdx = i;
		}

		Vector2 pos = listClip [idx].GetPos ();
		listClip [idx].RefreshPos (listClip [moveIdx].GetPos (), moveCol, moveRow);
		listClip [moveIdx].RefreshPos (pos, col, row);

		int temp = array [col, row];
		array [col, row] = array [moveCol, moveRow];
		array [moveCol, moveRow] = temp;
    }

	void _OnClick(int col, int row)
	{
		if (start == false || end)
			return;

		if (selRow == -1 || selCol == -1) {
			selRow = row;
			selCol = col;

			Clip clip = listClip.Find (x => x.Compare (col, row));
			if (clip != null)	clip.Select (true);
		} else {
			Clip clip = listClip.Find (x => x.Compare (selCol, selRow));
			if (clip != null)	clip.Select (false);

			if (selRow != row || selCol != col) {
				_MoveImg (selRow, selCol, row, col);
				_CheckEndGame ();
			}

			selRow = -1;
			selCol = -1;
		}			
	}

	void _CheckEndGame()
	{
		bool finish = true;
		for (int i = 0; i < mode; i++) {
			for (int j = 0; j < mode; j++) {
				if (array [i, j] != i * mode + j)
					finish = false;
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

		if (Info.practiceGame)
			ReturnPractiveGame ();
		else {
			objSendServer.SetActive (true);
			yield return new WaitForSeconds (3f);
			objSendServer.SetActive (false);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				Info.ShowResult ();
		}			
	}

	void _FailEndGame()
	{
		objGameOver.SetActive (true);
	}

	public void ReturnPractiveGame()
	{
		SceneChanger.LoadScene ("PracticeGame", objBoard);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("SelectGame", objBoard);
	}		
}