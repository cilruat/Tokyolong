using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
[RequireComponent(typeof(SpriteRenderer))]
public class CheckSizeSprite : MonoBehaviour {

    [ContextMenu("CheckSize")]

    void CheckSize()
    {
        Debug.Log(GetComponent<SpriteRenderer>().bounds.size);
        
    }
}
}