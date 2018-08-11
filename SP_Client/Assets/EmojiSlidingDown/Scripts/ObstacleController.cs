using UnityEngine;
using System.Collections;
using Emoji;

public class ObstacleController : MonoBehaviour
{
    public float hitPushBack;
    public GameObject hitEffect;
    public Sprite[] sprites;

    public void Awake()
    {
        if (sprites.Length > 0)
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.GameOver)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    void Update()
    {
        //Quaternion targetRotation = Quaternion.Euler(0, 0, RotationVariables.direction * Mathf.Abs(RotationVariables.maxAngle));
        //transform.root.rotation = Quaternion.RotateTowards(transform.root.rotation, targetRotation, RotationVariables.rotationDelta);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            hitEffect.transform.position = col.contacts[0].point;
            hitEffect.gameObject.SetActive(true);

            GameManager.Instance.playerController.anim.Squeeze();
            GameManager.Instance.playerRigidbody.AddForce(col.contacts[0].normal * hitPushBack);
        }
    }
}
