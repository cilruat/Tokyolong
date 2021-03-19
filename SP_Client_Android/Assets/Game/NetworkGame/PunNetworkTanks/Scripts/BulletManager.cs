using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PhotonGame
{

    public class BulletManager : Photon.MonoBehaviour
    {

        private bool facingLeft, facingRight, facingUp, facingDown = false;
        public float movingSpeed = 4f;
        public float destroyTime = 2f;


        [PunRPC]
        public void changeDirection_Left()
        {
            facingLeft = true;
        }
        [PunRPC]
        public void changeDirection_Right()
        {
            facingRight = true;
        }
        [PunRPC]
        public void changeDirection_Down()
        {
            facingDown = true;
        }
        [PunRPC]
        public void changeDirection_Up()
        {
            facingUp = true;
        }
        [PunRPC]
        private void destroyOBJ()
        {
            Destroy(gameObject);
        }

        void Update()
        {
            if (facingRight)
            {
                transform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
            }
            else if (facingLeft)
            {
                transform.Translate(Vector2.left * movingSpeed * Time.deltaTime);
            }
            else if (facingDown)
            {
                transform.Translate(Vector2.down * movingSpeed * Time.deltaTime);
            }
            else if (facingUp)
            {
                transform.Translate(Vector2.up * movingSpeed * Time.deltaTime);
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!photonView.isMine)
            {
                return;
            }
            PhotonView target = other.gameObject.GetComponent<PhotonView>();
            if (target != null && (!target.isMine || target.isSceneView))
            {
                if (other.tag == "Player")
                {
                    other.GetComponent<PhotonView>().RPC("reduceHealth", PhotonTargets.All);
                    this.GetComponent<PhotonView>().RPC("destroyOBJ", PhotonTargets.All);
                }
            }
        }
    }
}