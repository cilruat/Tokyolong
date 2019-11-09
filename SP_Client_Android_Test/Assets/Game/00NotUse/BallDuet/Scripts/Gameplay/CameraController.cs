using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
	public class CameraController : MonoBehaviour {

	    [Header("Camera Config")]
	    [SerializeField] private float smoothTime = 0.1f;

	    private Vector3 velocity = Vector3.zero;
	    private float originalXDistance = 0;
	    private float originalZ = 0;
	    private void Start()
	    {
	        originalXDistance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(PlayerController.Instance.transform.position.x));
	        originalZ = transform.position.z;
	    }
	    private void Update()
	    {
	        if (PlayerController.Instance.PlayerState == PlayerState.Living)
	        {
	            float currentXDistance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(PlayerController.Instance.transform.position.x));

	            float newDistance = currentXDistance - originalXDistance;

	            Vector3 targetPos = transform.position + Vector3.right * newDistance;
	            targetPos.z = originalZ;
	            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
	        }
	    }
	}
}