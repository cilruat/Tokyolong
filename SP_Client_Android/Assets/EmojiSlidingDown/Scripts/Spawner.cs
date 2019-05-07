using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Emoji
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner Instance;

        List<GameObject> clouds;
        //-1 for left, 0 for center, 1 for right
        float sign;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        public void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (oldState == GameState.Prepare && newState == GameState.Playing)
            {
                clouds = new List<GameObject>();
                BunkSpawn(5);
            }
        }

        public void SpawnCloud()
        {
            Vector3 nextPos;
            bool contactFlag = false;
            if (clouds.Count == 0)
            {
                nextPos = new Vector3(0, GameManager.Instance.cloudInititalPosY, 0);
                sign = 0;
                contactFlag = true;
            }
            else
            {
                Vector3 lastPos = clouds[clouds.Count - 1].transform.position;
                nextPos = new Vector3(0, lastPos.y - GameManager.Instance.cloudVerticleOffset, 0);
                if (sign == -1 || sign == 1)
                    sign = 0;
                else
                    sign = Mathf.Sign(Random.Range(-10, 10));
            }

            GameObject g = Instantiate(GameManager.Instance.cloudPrefab.gameObject, nextPos, Quaternion.identity) as GameObject;
            g.transform.GetChild(0).Translate(new Vector3(sign * GameManager.Instance.cloudHorizontalOffset, 0, 0));
            CloudController controller = g.GetComponentInChildren<CloudController>();
            controller.contactFlag = contactFlag;
            bool hasCoin = Random.Range(0f, 1f) < GameManager.Instance.coinFrequency && !contactFlag ? true : false;
            controller.SetActiveCoin(hasCoin);
            if (clouds.Count == 0)
                controller.noObstacle = true;
            if (sign == 0)
            {
                Vector3 translation = new Vector3(Mathf.Sign(Random.Range(-10, 10)) * GameManager.Instance.cloudHorizontalOffset, 0, 0);
                controller.obstacle.transform.Translate(translation);

            }
            clouds.Add(g);

            if (clouds.Count == 2)
                GameManager.Instance.firstRotationDirection = -sign;
            GameManager.Instance.playerDeathPlane.MoveDown();

        }

        public void BunkSpawn(int quantity)
        {
            for (int i = 0; i < quantity; ++i)
            {
                SpawnCloud();
            }
        }

        public void OnDrawGizmos()
        {
            if (GameManager.Instance != null)
            {
                float camPosY = Camera.main.transform.position.y;
                float length = Camera.main.orthographicSize;
                Gizmos.DrawRay(new Vector3(-GameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.up * length);
                Gizmos.DrawRay(new Vector3(-GameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.down * length);

                Gizmos.DrawRay(new Vector3(0, camPosY, 0), Vector3.up * length);
                Gizmos.DrawRay(new Vector3(0, camPosY, 0), Vector3.down * length);

                Gizmos.DrawRay(new Vector3(GameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.up * length);
                Gizmos.DrawRay(new Vector3(GameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.down * length);
            }
        }

    }
}