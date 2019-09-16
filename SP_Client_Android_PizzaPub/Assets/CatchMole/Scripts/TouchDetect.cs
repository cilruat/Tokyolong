using UnityEngine;

public class TouchDetect : MonoBehaviour {

    public bool MobileControll;
    public static TouchDetect touchDetectScript;

    // Use this for initialization
    void Start () {
        touchDetectScript = gameObject.GetComponent<TouchDetect>();
    }
	
	// Update is called once per frame
	void Update () {
        TouchDetected();
    }

    void TouchDetected() { 
        if (MobileControll)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                DuplicateCode(worldPoint);
            }
        }
        else {
            if (Input.GetMouseButtonUp(0)) {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DuplicateCode(worldPoint);
            }
        }

    }

    void DuplicateCode(Vector2 worldPoint) { 
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null) {
            if (HealPoint.GameOver == false) {
                hit.collider.gameObject.GetComponent<PlaceScript>().HitMe(true);
            }
            else {
                hit.collider.gameObject.GetComponent<StartGame>().HitMe(true);
            }
        }
    }
}
