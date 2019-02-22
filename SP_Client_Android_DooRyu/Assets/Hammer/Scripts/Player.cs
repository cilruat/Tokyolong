using System.Collections;
using UnityEngine;

namespace Hammer
{
	public class Player : MonoBehaviour
	{
	    public Rigidbody2D rb;
	    public Animator _anim;
	    public static bool firstTap = true;
	    public static float xPos = 1.1f, yPos = -2.4f;
	    Vector2 leftPos = new Vector2(-xPos, yPos), rightPos = new Vector2(xPos, yPos);
	    Vector2 leftP = new Vector2(-1f, 1f), rightP = new Vector2(1f, 1f);

	    public Vector2 GetPlayerPos(bool isLeft)
	    {
	        return isLeft ? leftPos : rightPos;
	    }
	    public static bool isInProcess = false;

	    private void Update()
	    {
			if (UIManager.Instance.isStart == false)
				return;

			if (UIManager.Instance.objGameOver.activeInHierarchy)
	            return;
			
			if (Input.GetKeyDown(KeyCode.LeftArrow))
	        {
	            AttackOnClick(true);
	        }
			if (Input.GetKeyDown(KeyCode.RightArrow))
	        {
	            AttackOnClick(false);
	        }
	    }

		public void Click(bool left)
		{
			if (UIManager.Instance.isStart == false)
				return;

			if (UIManager.Instance.objGameOver.activeInHierarchy)
				return;

			AttackOnClick (left ? true : false);
		}

	    public void AttackOnClick(bool isLeft)
	    {
	        if (!isInProcess)
	        {
	            isInProcess = true;
	            if (firstTap)
	            {
	                firstTap = false;
	                GameManager.Instance.tapObj.SetActive(false);
	                GameManager.Instance.timer.RunTimer();
	            }
	            else
	                GameManager.Instance.timer.IncrementTimer();

	            transform.localScale = isLeft ? leftP : rightP;	            
				transform.position = GetPlayerPos(isLeft);
	            if (GameManager.Instance.IsCurrentBlockHazard(isLeft))
	            {
	                Over();
	            }
	            else if (GameManager.Instance.IsNextBlockHazard(isLeft))
	            {
	                GameManager.Instance.PlaySmash();
	                _anim.SetTrigger("attack");
	                Over();
	            }
	            else
	            {
	                GameManager.Instance.PlaySmash();
	                _anim.SetTrigger("attack");
	            }

	        }
	    }
	    private void Over()
	    {
	        StartCoroutine(WaitNOver());
	    }
	    IEnumerator WaitNOver()
	    {
	        _anim.SetBool("death", true);
	        yield return new WaitForSeconds(0.2f);
	        GameManager.Instance.PlayAh();
	        GameManager.Instance.timer.EndTimer();
	        UIManager.Instance.GameOver();
	    }
	    //private void OnTriggerEnter2D(Collider2D other)
	    //{
	    //print("Collision Detected");
	    //if (other.gameObject.tag == "Hazard")
	    //{
	    //    StopAllCoroutines();

	    //}
	    //}
	}
}