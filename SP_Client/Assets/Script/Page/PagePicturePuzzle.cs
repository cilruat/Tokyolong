using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageClip
{
    RawImage imgMain;
    int row, col;
    float height, width;

    public ImageClip(RawImage imgMain, int row, int col)
    {
        this.imgMain = imgMain;
        this.row = row;
        this.col = col;

        RectTransform rt = imgMain.rectTransform;
        this.height = (float)rt.rect.height / (float)row;
        this.width = (float)rt.rect.width / (float)col;
    }


}

public class PagePicturePuzzle : SingletonMonobehaviour<PagePicturePuzzle> {

    const int EASY_MODE = 3;
    const int HARD_MODE = 5;
    const int LIMIT_TIME = 15;

    public RawImage img;
    public RawImage imgPreview;
    public GameObject objClip;
    public RectTransform rtParent;

    int mode = 0;
    int[,] array;
    ImageClip imgClip;

    void Awake()
    {
        _Init();
    }

    void _Init()
    {
        ////////////////
        Info.GameDiscountWon = (short)(UnityEngine.Random.Range(0, 1) == 0 ? EASY_MODE : HARD_MODE);
        ////////////////

        int mode = Info.GameDiscountWon;
        array = new int[mode, mode];

        _SetImage();

        for (int i = 0; i < mode; i++)
            for (int j = 0; j < mode; j++)
                array[i, j] = i * mode + j;

        array[mode - 1, mode - 1] = -1;
    }

    void _SetImage()
    {
        int rand = UnityEngine.Random.Range(1, 2);
        Texture tex = Resources.Load("Puzzle" + rand.ToString()) as Texture;

        //img.texture = tex;
        imgPreview.texture = tex;

        imgClip = new ImageClip(img, mode, mode);
        //imgPreview.gameObject.SetActive(false);

        _ClopClip();
        _MixClip();
    }

    void _ClopClip()
    {
        int rand = UnityEngine.Random.Range(1, 2);
        Texture tex = Resources.Load("Puzzle" + rand.ToString()) as Texture;

        for (int i = 0; i < 4; i++)
        {
            GameObject obj = Instantiate(objClip) as GameObject;

            RectTransform rt = (RectTransform)obj.transform;
            rt.SetParent(rtParent);
            rt.InitTransform();

            rt.anchoredPosition = new Vector2(i * 100f, 0f);
        }
        
        img.SetClipRect(new Rect(-200, 0, 200, 200), true);
    }

    void _MixClip()
    {
        int row, col;
        int emptyRow = -1;
        int emptyCol = -1;

        System.Random rand = new System.Random();

        for (int i = 0; i < 100; i++)
        {
            row = rand.Next(mode);
            col = rand.Next(mode);

            if (_FindEmptyIdx(row, col, ref emptyRow, ref emptyCol))
                _MoveImg(row, col, emptyRow, emptyCol);
        }
    }          

    bool _FindEmptyIdx(int row, int col, ref int emptyRow, ref int emptyCol)
    {
        for (int i = 0; i < mode; i++)
        {
            if (array[i, col] == -1)
            {
                emptyRow = i;
                emptyCol = col;
                return true;
            }
        }

        for (int i = 0; i < mode; i++)
        {
            if (array[row, i] == -1)
            {
                emptyRow = row;
                emptyCol = i;
                return true;
            }
        }

        return false;
    }
    
    void _MoveImg(int row, int col, int emptyRow, int emptyCol)
    {
        
    }
}