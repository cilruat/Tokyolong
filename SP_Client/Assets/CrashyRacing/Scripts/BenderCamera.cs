using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenderCamera : MonoBehaviour {
    [Range(0, 0.5f)]
    public float extraCullHeight = 0.3f;
    public Camera _camera;

    private void Start()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();
    }

    private void OnPreCull()
    {
        float ar = _camera.aspect;
        float fov = _camera.fieldOfView;
        float viewPortHeight = Mathf.Tan(Mathf.Deg2Rad * fov * 0.5f);
        float viewPortwidth = viewPortHeight * ar;

        float newfov = fov * (1 + extraCullHeight);
        float newheight = Mathf.Tan(Mathf.Deg2Rad * newfov * 0.5f);
        float newar = viewPortwidth / (newheight);

        _camera.projectionMatrix = Matrix4x4.Perspective(newfov, newar, _camera.nearClipPlane, _camera.farClipPlane);
    }

    private void OnPreRender()
    {
        _camera.ResetProjectionMatrix();
    }
}
