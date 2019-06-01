using UnityEngine;

namespace GameBench
{
public class AnimExit : MonoBehaviour
{
    public void OnAnimEnd()
    {
        gameObject.SetActive(false);
    }
}
}