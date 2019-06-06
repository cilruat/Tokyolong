using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockJumpRabbit
{
 public class SelfDeactive:MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 2.5f);
    }
}
}
