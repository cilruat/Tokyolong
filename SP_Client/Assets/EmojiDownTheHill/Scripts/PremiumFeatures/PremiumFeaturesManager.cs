using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiumFeaturesManager : MonoBehaviour
{
    public static PremiumFeaturesManager Instance { get; private set; }

    [Header("Check to enable premium features (require EasyMobile plugin)")]
    public bool enablePremiumFeatures = true;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
