using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class BackgroundManager : MonoBehaviour {

    // Current default screen: 1920x1080

    void Awake()
    {
        float per = (float)(Screen.width) / (float)(Screen.height);

        // Scale if screen change
        transform.localScale = new Vector2( per/ ((float)1920 / (float)1080), 1);
    }
}
}