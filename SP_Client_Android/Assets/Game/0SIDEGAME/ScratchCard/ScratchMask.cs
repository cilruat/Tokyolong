using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// 1. ���丽�ӵ���/д��spriteͼ����
// 2. ����ͼ������Ϊ�ڹ���Ͷ����ʹ��
// 3. ����ͼ������Ϊ�ڹ���Ͷ����ʹ��
// 4. ��ס�����������������
public class ScratchMask : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("��ˢ��״���飬ֻʶ��͸����Ϊ1�����ص�")]
    [SerializeField]
    Sprite brushSprite;
    [Tooltip("��ˢalpha,ֻ�г�����ֵ�Ĳű���Ⱦ")]
    float brushFilterAlpha;
    [Tooltip("��ˢ��ɫ")]
    Color brushColor = new Color(255f, 255f, 255f, 0f);
    [Tooltip("��ˢ���")]
    [SerializeField]
    int brushWidth = 20;
    [Tooltip("������Ʒ")]
    [SerializeField]
    Image rewardImage;

    [Tooltip("�ڵ�ͼƬ����Unity���ļ��༭���б����ж�/дȨ��")]
    Image maskImage;
    Sprite maskSprite;
    Texture2D maskTeture;
    int maskWidth;
    int maskHeight;

    Vector2Int lastPos;
    Color[] originColors;
    Color32[] curMaskColors;
    bool isPointIn = false;

    //��ˢ
    List<Vector2Int> brushPixelPosList = new List<Vector2Int>();

    //��Ʒ����
    Rect rewardPixelRect;
    //��Ʒ��������
    long rewardPixelCount;

    //�޸Ĺ�������λ��
    HashSet<long> changedPos = new HashSet<long>();

    void Awake()
    {
        maskImage = GetComponent<Image>();
        maskSprite = maskImage.sprite;
        maskTeture = maskSprite.texture;

        // ����ͼƬ������ɫ
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
    /// ��ʼ����ˢλ�õ�
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
    /// ��ʼ����Ʒλ��
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
    /// �޸����Maskλ�õ���ɫ
    /// </summary>
    /// <param name="localPos"></param>
    public void ChangeColorAtPoint(Vector2 localPos)
    {
        Vector2Int pixelPos = CalculatePixelPositionByOffset(localPos);
        curMaskColors = maskTeture.GetPixels32();

        if (lastPos == Vector2.zero)
        {
            // ����������ǵ�һ���϶����ͼ��ֻ�������λ������ɫ����
            MarkPixelsToColour(pixelPos);
        }
        else
        {
            // �������ϴθ��º��е�λ���ϵ���ɫ
            ColourBetween(lastPos, pixelPos);
        }

        ApplyMarkedPixelChanges();

        lastPos = pixelPos;
    }

    /// <summary>
    /// ͨ��ƫ�Ƽ�������
    /// </summary>
    /// <param name="localPos"></param>
    /// <returns></returns>
    private Vector2Int CalculatePixelPositionByOffset(Vector2 localPos)
    {
        // ��Ҫ�����ǵ��������
        float centered_x = localPos.x + maskWidth / 2;
        float centered_y = localPos.y + maskHeight / 2;

        // ������ƶ�����������ص�
        return new Vector2Int(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));
    }


    // ����ʼ��һֱ���յ㣬�����ص���ɫ����Ϊֱ�ߣ���ȷ���м���������ݶ��ǲ�ɫ��        
    public void ColourBetween(Vector2 startPos, Vector2 endPos)
    {
        // �ӿ�ʼ�������ľ���
        float distance = Vector2.Distance(startPos, endPos);

        Vector2 curPos = startPos;

        // �����ڿ�ʼ��ͽ�����֮�������ٴΣ������ϴθ���������ʱ����
        float lerpSteps = brushWidth / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerpSteps)
        {
            curPos = Vector2.Lerp(startPos, endPos, lerp);
            MarkPixelsToColour(curPos);
        }
    }


    public void MarkPixelsToColour(Vector2 center_pixel)
    {
        // ���������Ҫ��ÿ����������ɫ���ٸ����أ�x��y��
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;

        foreach (var item in brushPixelPosList)
        {
            MarkPixelToChange(center_x + item.x, center_y + item.y, brushColor);
        }
    }

    /// <summary>
    /// �ڶ�Ӧλ����ɫ
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void MarkPixelToChange(int x, int y, Color color)
    {
        // ��Ҫ��x��y����ת��Ϊ�����ƽ������
        long array_pos = y * (int)maskSprite.rect.width + x;

        // ������Ƿ���һ����Ч��λ��
        if (array_pos > curMaskColors.Length || array_pos < 0)
        {
            //Debug.Log("un pos");
            return;

        }

        //��¼�Ѿ���ɫ��λ��
        if (x > rewardPixelRect.xMin && x < rewardPixelRect.xMax && y > rewardPixelRect.yMin && y < rewardPixelRect.yMax)
        {
            changedPos.Add(array_pos);
        }

        curMaskColors[array_pos] = color;
    }

    /// <summary>
    /// Ӧ�øı�
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

    //��ԭ��������ͼƬ��ɫ
    private void ResetColor()
    {
        maskTeture.SetPixels(originColors);
        maskTeture.Apply();
    }

}
