using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class UIController : MonoBehaviour {

	public GameObject ai_enemy;
	public Animator cloundAnim;
	public Animator mainUiAnim;
	public CloundMove cloundMove;
    public GameObject chatTextWoltf;
    public GameObject chatTextPlayer;
    public PlayerController player;
	public SoundController soundController;

	// Call from animation
	public void AIJumpInSCreen(){
        float x = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        ai_enemy.transform.position = new Vector2(x - 0.5f, ai_enemy.transform.position.y);
        cloundMove.enabled = true;
		cloundAnim.enabled = true;
		ai_enemy.GetComponent<PlayerController> ().Jump (PlayerController.TypeJump.DoubleJump);
        player.ResetSprite();
		soundController.SoundBackgroundOn ();

		StartCoroutine (SHowChatText ());
	}

    // SHow chat text when home
    IEnumerator SHowChatText()
    {
        yield return new WaitForSeconds(0.5f);
        chatTextWoltf.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        chatTextPlayer.SetActive(true);

		mainUiAnim.SetTrigger ("canPlay");

    }
	}
}
