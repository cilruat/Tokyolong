using UnityEngine;

public class AnimExit : MonoBehaviour
{
    public void OnAnimEnd()
    {
        gameObject.SetActive(false);
    }
}
