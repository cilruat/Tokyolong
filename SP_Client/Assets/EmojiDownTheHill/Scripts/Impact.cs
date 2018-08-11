using UnityEngine;
using System.Collections;
using SgLib;
using Emoji2;

/// <summary>
/// This class is attached to the movingGameObject GameObject
/// since the movingGameObject 's the  actual collision checker, not the ball itself
/// </summary>
public class Impact : MonoBehaviour
{

    public GameObject theBall;
    public GameObject theBallsSprite;
    // the  ball sprite of the ball
    public PlayerController playerController;
    public GameObject fakeBall;

    //Instead of switching off the colliders and other component
    //For the sake of simplicity , instantiate a new one
    GameObject endBall;
    // the ball that appears in the end , after the player has crashing  or falling
    public GameObject spawner;

    bool firstGround = true;

    void OnTriggerEnter2D(Collider2D coll)
    {
		
        // Not checking collision while jumping
        if (playerController.isJumping)
        {
            return;
        }

        if (coll.tag == "ground")
        {

            if (firstGround)
            {
                firstGround = false;
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.jump);
            }
			
            coll.gameObject.GetComponent<TheTerrian>().ChangeColor();
            ScoreManager.Instance.AddScore(1);
            theBall.transform.position = coll.gameObject.transform.Find("flag").transform.position;
            transform.position = theBall.transform.position;
            if (coll.GetComponent<TheTerrian>().isOwningASpring)
            {
                playerController.isreadyForBigJump = true;
            }

        }
        else if (coll.tag == "vacant")
        {
            playerController.Die();
            theBall.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            endBall = Instantiate(fakeBall, transform.position, Quaternion.identity) as GameObject;
            endBall.GetComponent<SpriteRenderer>().sprite = CharacterManager.Instance.character;
            endBall.GetComponent<Renderer>().sortingOrder = -1;    // fall behind the hill
            gameObject.SetActive(false);
           
        }
        else if (coll.tag == "refill")
        {
            Destroy(coll.gameObject);

            foreach (GameObject c in spawner.GetComponent<SpawnCubes>().myWaves)
            {
                c.GetComponent<CubeWave>().Refill();
            }

        }
        else if (coll.tag == "tree")
        {
            playerController.Die();
            theBall.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            endBall = Instantiate(fakeBall, transform.position, Quaternion.identity) as GameObject;
            endBall.GetComponent<SpriteRenderer>().sprite = CharacterManager.Instance.character;
            gameObject.SetActive(false);
        }

        playerController.isCheckingCollision = false;
    }
}
