using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform[] _players;
        // distance between last player and camera border (additional camera space)
        [SerializeField] private float _offsetX = 2f;
        [SerializeField] private Vector2 _minMaxCameraSize = new Vector2(6.5f, 7.25f);
        [SerializeField] private Vector2 _borderPositionX = new Vector2(-14, 14);
        [SerializeField] private Vector2 _borderPositionY = new Vector2(-7, 7);

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            Vector2 minMaxPositionX = GetMinMaxPositionX();

            ChangeCameraSize(minMaxPositionX);
            MoveCamera(minMaxPositionX);
        }

        private void ChangeCameraSize(Vector2 minMaxPositionX)
        {
            float width = minMaxPositionX.y - minMaxPositionX.x + _offsetX;

            float height = width / Screen.width * Screen.height / 2;
            height = Mathf.Clamp(height, _minMaxCameraSize.x, _minMaxCameraSize.y);
            _camera.orthographicSize = height;
        }

        private void MoveCamera(Vector2 minMaxPositionX)
        {
            float positionX = (minMaxPositionX.y + minMaxPositionX.x) / 2;

            Vector2 minMaxPositionY = GetMinMaxPositionY();
            float positionY = (minMaxPositionY.y + minMaxPositionY.x) / 2f;

            float w = _camera.orthographicSize * 2 / Screen.height * Screen.width;

            positionX = Mathf.Clamp(positionX, _borderPositionX.x + w / 2, _borderPositionX.y - w / 2);
            positionY = Mathf.Clamp(positionY, _borderPositionY.x + _camera.orthographicSize, _borderPositionY.y - _camera.orthographicSize);

            transform.position = new Vector3(positionX, positionY, transform.position.z);
        }

        private Vector2 GetMinMaxPositionX()
        {
            Vector2 position;

            float min = float.MaxValue;
            float max = float.MinValue;

            foreach(Transform t in _players)
            {
                if (t.position.x < min)
                    min = t.position.x;
                if (t.position.x > max)
                    max = t.position.x;
            }

            position.x = min;
            position.y = max;

            return position;
        }

        private Vector2 GetMinMaxPositionY()
        {
            Vector2 position;

            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (Transform t in _players)
            {
                if (t.position.y < min)
                    min = t.position.y;
                if (t.position.y > max)
                    max = t.position.y;
            }

            position.x = min;
            position.y = max;

            return position;
        }

    }

}