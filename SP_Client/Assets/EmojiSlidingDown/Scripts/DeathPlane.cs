using UnityEngine;
using System.Collections;
using Emoji;

public class DeathPlane : MonoBehaviour
{
    public float moveDownDistance;

    public void MoveDown()
    {
        transform.Translate(Vector3.down * moveDownDistance);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.playerController.Die();
        }
    }
}
