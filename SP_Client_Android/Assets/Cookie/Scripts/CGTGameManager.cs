using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class CGTGameManager : MonoBehaviour {

    public static CGTGameManager instance = null;

    

    internal bool isGameOver = false;
    internal bool isGamePaused = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}
