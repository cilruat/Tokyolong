using UnityEngine;

namespace Touchdowners
{

    public class TargetedFrameRate : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = _frameRate;
        }
    }

}