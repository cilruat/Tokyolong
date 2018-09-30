using UnityEngine;
using System.Collections;

namespace Emoji
{
    public class CloudController : MonoBehaviour
    {
        public GameObject spriteObject;
        public bool noObstacle;
        public float obstacleDistance;
        public GameObject obstacle;
        public GameObject coin;
        public GameObject cloudEffect;
        public float distanceToDisable;

        public bool contactFlag;

        [Header("Animation")]
        public CloudAnim anim;

        public void Start()
        {
            bool hasObstacle = Random.Range(0f, 1f) < GameManager.Instance.obstacleFrequency && !noObstacle ? true : false;
            obstacle.SetActive(hasObstacle);
            obstacle.transform.Translate(0, obstacleDistance, 0, Space.Self);
        }

        public void Update()
        {
            Rotate();
            CheckDisable();
        }

        public void Rotate()
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, GameManager.Instance.rotationDirection * Mathf.Abs(GameManager.Instance.maxRotatingAngle));
            transform.root.rotation = Quaternion.RotateTowards(transform.root.rotation, targetRotation, GameManager.Instance.rotationDelta);
        }

        public void CheckDisable()
        {
            Vector3 playerPos = GameManager.Instance.playerController.transform.position;
            float sqrDistance = (transform.position - playerPos).sqrMagnitude;
            if (GameManager.Instance.GameState == GameState.Playing && sqrDistance >= distanceToDisable * distanceToDisable && transform.position.y > playerPos.y)
            {
                transform.root.gameObject.SetActive(false);
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.tag == "Player" && contactFlag == false)
            {
                contactFlag = true;
                cloudEffect.transform.position = col.contacts[0].point;
                cloudEffect.SetActive(true);

                //add score, camera shake, sound...
                anim.Bounce();
                GameManager.Instance.playerController.anim.Squeeze();
                Spawner.Instance.BunkSpawn(Random.Range(1, 3));
                ScoreManager.Instance.AddScore(1);
                CameraController.Instance.ShakeCamera();
                SoundManager.Instance.PlaySound(SoundManager.Instance.cloudHit);
            }
        }

        public void SetActiveCoin(bool active)
        {
            coin.gameObject.SetActive(active);
        }
    }
}