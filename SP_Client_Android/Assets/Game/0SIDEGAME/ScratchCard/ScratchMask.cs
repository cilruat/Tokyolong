using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// 1. 将其附加到读/写的sprite图像上
// 2. 将绘图层设置为在光线投射中使用
// 3. 将绘图层设置为在光线投射中使用
// 4. 按住鼠标左键画出这个纹理！
public class ScratchMask : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("笔刷形状精灵，只识别透明度为1的像素点")]
    [SerializeField]
    Sprite brushSprite;
    [Tooltip("笔刷alpha,只有超过此值的才被渲染")]
    float brushFilterAlpha;
    [Tooltip("笔刷颜色")]
    Color brushColor = new Color(255f, 255f, 255f, 0f);
    [Tooltip("笔刷宽度")]
    [SerializeField]
    int brushWidth = 20;
    [Tooltip("奖励物品")]
    [SerializeField]
    Image rewardImage;

    [Tooltip("遮挡图片，在Unity的文件编辑器中必须有读/写权限")]
    Image maskImage;
    Sprite maskSprite;
    Texture2D maskTeture;
    int maskWidth;
    int maskHeight;

    Vector2Int lastPos;
    Color[] originColors;
    Color32[] curMaskColors;
    bool isPointIn = false;

    //笔刷
    List<Vector2Int> brushPixelPosList = new List<Vector2Int>();

    //奖品区域
    Rect rewardPixelRect;
    //奖品像素数量
    long rewardPixelCount;

    //修改过的像素位置
    HashSet<long> changedPos = new HashSet<long>();

    void Awake()
    {
        maskImage = GetComponent<Image>();
        maskSprite = maskImage.sprite;
        maskTeture = maskSprite.texture;

        // 保存图片所有颜色
        originColors = maskTeture.GetPixels(0, 0, maskTeture.width, maskTeture.height);
        curMaskColors = maskTeture.GetPixels32();

        maskWidth = (int)maskSprite.rect.width;
        maskHeight = (int)maskSprite.rect.height;
    }

    void Start()
    {
        InitBrushPositions();
        InitRewardPositions();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject)
        {
            isPointIn = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointIn = false;
        lastPos = new Vector2Int(0, 0);
    }

    void Update()
    {
        if (isPointIn)
        {
            Vector3 screenPos = Input.mousePosition;
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPos);
            Vector3 localPos = transform.InverseTransformPoint(worldPoint);
            ChangeColorAtPoint(localPos);
        }
    }

    private void OnDestroy()
    {
        ResetColor();
    }

    /// <summary>
    /// 初始化笔刷位置点
    /// </summary>
    private void InitBrushPositions()
    {
        Color[] brushColors = brushSprite.texture.GetPixels(0, 0, brushSprite.texture.width, brushSprite.texture.height);
        int textureWidth = brushSprite.texture.width;
        int textureHeight = brushSprite.texture.height;

        int widthIndex = 0;
        int heightIndex = 0;

        foreach (var item in brushColors)
        {
            if (item.a > brushFilterAlpha)
            {
                brushPixelPosList.Add(new Vector2Int()
                {
                    x = widthIndex - textureWidth / 2,
                    y = heightIndex - textureHeight / 2,
                });
            }
            if ((widthIndex + 1) % textureWidth == 0)
            {
                widthIndex = 0;
                heightIndex++;
            }
            else
            {
                widthIndex++;
            }
        }
    }

    /// <summary>
    /// 初始化奖品位置
    /// </summary>
    void InitRewardPositions()
    {
        RectTransform rewardRtf = rewardImage.rectTransform;
        Vector2 pivot = rewardRtf.pivot;
        Vector2 offsetDis = rewardRtf.localPosition - transform.localPosition;
        Vector2Int pixelPos = CalculatePixelPositionByOffset(offsetDis);
        rewardPixelRect = rewardRtf.rect;
        rewardPixelRect.position = pixelPos + new Vector2(-pivot.x * rewardPixelRect.width, -pivot.y * rewardPixelRect.height);
        rewardPixelCount = (long)(rewardPixelRect.width * rewardPixelRect.height);
    }

    /// <summary>
    /// 修改相对Mask位置的颜色
    /// </summary>
    /// <param name="localPos"></param>
    public void ChangeColorAtPoint(Vector2 localPos)
    {
        Vector2Int pixelPos = CalculatePixelPositionByOffset(localPos);
        curMaskColors = maskTeture.GetPixels32();

        if (lastPos == Vector2.zero)
        {
            // 如果这是我们第一次拖动这个图像，只需在鼠标位置上着色像素
            MarkPixelsToColour(pixelPos);
        }
        else
        {
            // 从我们上次更新呼叫的位置上的颜色
            ColourBetween(lastPos, pixelPos);
        }

        ApplyMarkedPixelChanges();

        lastPos = pixelPos;
    }

    /// <summary>
    /// 通过偏移计算坐标
    /// </summary>
    /// <param name="localPos"></param>
    /// <returns></returns>
    private Vector2Int CalculatePixelPositionByOffset(Vector2 localPos)
    {
        // 需要把我们的坐标居中
        float centered_x = localPos.x + maskWidth / 2;
        float centered_y = localPos.y + maskHeight / 2;

        // 将鼠标移动到最近的像素点
        return new Vector2Int(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));
    }


    // 从起始点一直到终点，将像素的颜色设置为直线，以确保中间的所有内容都是彩色的        
    public void ColourBetween(Vector2 startPos, Vector2 endPos)
    {
        // 从开始到结束的距离
        float distance = Vector2.Distance(startPos, endPos);

        Vector2 curPos = startPos;

        // 计算在开始点和结束点之间插入多少次，基于上次更新以来的时间量
        float lerpSteps = brushWidth / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerpSteps)
        {
            curPos = Vector2.Lerp(startPos, endPos, lerp);
            MarkPixelsToColour(curPos);
        }
    }


    public void MarkPixelsToColour(Vector2 center_pixel)
    {
        // 算出我们需要在每个方向上着色多少个像素（x和y）
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;

        foreach (var item in brushPixelPosList)
        {
            MarkPixelToChange(center_x + item.x, center_y + item.y, brushColor);
        }
    }

    /// <summary>
    /// 在对应位置着色
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void MarkPixelToChange(int x, int y, Color color)
    {
        // 需要将x和y坐标转换为数组的平面坐标
        long array_pos = y * (int)maskSprite.rect.width + x;

        // 检查这是否是一个有效的位置
        if (array_pos > curMaskColors.Length || array_pos < 0)
        {
            //Debug.Log("un pos");
            return;

        }

        //记录已经着色的位置
        if (x > rewardPixelRect.xMin && x < rewardPixelRect.xMax && y > rewardPixelRect.yMin && y < rewardPixelRect.yMax)
        {
            changedPos.Add(array_pos);
        }

        curMaskColors[array_pos] = color;
    }

    /// <summary>
    /// 应用改变
    /// </summary>
    public void ApplyMarkedPixelChanges()
    {
        if (changedPos.Count > rewardPixelCount * 0.5)
        {
            OpenMask();
        }

        maskTeture.SetPixels32(curMaskColors);
        maskTeture.Apply();
    }

    private void OpenMask()
    {
        //maskImage.color = new Color(1, 1, 1, 0);
    }

    //还原被擦除的图片颜色
    private void ResetColor()
    {
        maskTeture.SetPixels(originColors);
        maskTeture.Apply();
    }

}
