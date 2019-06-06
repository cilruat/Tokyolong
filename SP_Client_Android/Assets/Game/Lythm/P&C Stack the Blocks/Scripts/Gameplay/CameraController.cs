using System.Collections;
using UnityEngine;

namespace PnCCasualGameKit
{
/// <summary>
/// Controls Camera movement and effects.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Speed at which camera moves to the next position and zooms out at game over")]
    [SerializeField]
    private float speed;

    [Tooltip("The block from which game starts")]
    [SerializeField]
    private Transform startBaseBlock;

    /// <summary> Camera moves up by this value</summary>
    private float heightDelta;

    private Vector3 startPos, targetPos;
  
    private Camera cam;

    [Tooltip("Camera zooms out by this factor multuplied with number of stacked blocks")]
    [SerializeField]
    private float zoomOutFactor;

    /// <summary>
    /// So that we can reset
    /// </summary>
    private float defaultZoomValue;

    [Tooltip("The minimum zoom value so that there is some zoom out even at zero score")]
    [SerializeField]
    private float minZoomValue;

    [Tooltip("The maximum zoom value so that we don't end up with a hardly visible tower of blocks")]
    [SerializeField]
    private float maxZoomValue;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        startPos = transform.position;
        targetPos = transform.position;
        heightDelta = startBaseBlock.lossyScale.y;
        defaultZoomValue = cam.orthographicSize;

        GameManager.Instance.GameInitialized += ResetCam;
        GameManager.Instance.GameOver += GameOverHandler;
    }



    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

    /// <summary>
    /// Update the target position for camera on scoring
    /// </summary>
    public void UpdatePos()
    {
        targetPos += Vector3.up * heightDelta;
    }

    /// <summary>
    /// This couroutine zooms out the camera. Zoooming out is done by simply increasing the orthographicSize of the camera
    /// </summary>
    IEnumerator ZoomOutCouroutine()
    {
        //Zoom out to this value
        float targetOrthographicSize = ScoreAndCashManager.Instance.currentScore * zoomOutFactor;

        //clamp between minimum and maximum zoom value. 
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minZoomValue, maxZoomValue);

        //Lerp might not reach the exact value. So keeping a margin of 0.3. 
        while (cam.orthographicSize < targetOrthographicSize - 0.3f)
        {
            yield return new WaitForEndOfFrame();
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthographicSize, Time.deltaTime * speed);
        }

    }

    /// <summary>
    /// Do this at game over
    /// </summary>
    void GameOverHandler()
    {
        StartCoroutine("ZoomOutCouroutine");
    }


    /// <summary>
    /// Resets the cam for game start
    /// </summary>
    void ResetCam()
    {
        StopCoroutine("ZoomOutCouroutine");
        transform.position = startPos;
        targetPos = startPos;
        cam.orthographicSize = defaultZoomValue;
    }

}
}
