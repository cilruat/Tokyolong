using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PhotonGame
{

    public class PlayerMove : Photon.MonoBehaviour
    {
        public PhotonView photonView;
        [Header("General booleans")]
        public bool devTesting = false;
        [Space]
        [Header("General floats")]
        public float movementSpeed = 1.5f;
        [Space]
        public GameObject playerCam;
        public Text playerName;
        private Vector3 selfPos;
        private GameObject sceneCam;
        public Color enemyTextColor;
        public GameObject bulletPrefab;
        [Space]
        [Header("Sprite swaps")]
        public Sprite spriteLeft;
        public Sprite spriteRight;
        public Sprite spriteUp;
        public Sprite spriteDown;
        private bool facingLeft, facingRight, facingUp, facingDown = false;

        void Awake()
        {
            if (!devTesting && photonView.isMine)
            {
                playerName.text = PhotonNetwork.playerName;
            }

            if (!devTesting && !photonView.isMine)
            {
                playerName.text = photonView.owner.name;
                playerName.color = enemyTextColor;
            }
        }
        private void Update()
        {
            if (!devTesting)
            {
                if (photonView.isMine)
                {
                    checkInput();
                }
                else
                {
                    smoothNetMovement();
                }
            }
            else
            {
                checkInput();
            }

        }
        private void checkInput()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            transform.position += move * movementSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteRight;
                photonView.RPC("onSpriteFlipRight", PhotonTargets.Others);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteLeft;
                photonView.RPC("onSpriteFlipLeft", PhotonTargets.Others);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteUp;
                photonView.RPC("onSpriteFlipUp", PhotonTargets.Others);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteDown;
                photonView.RPC("onSpriteFlipDown", PhotonTargets.Others);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                shoot();
            }
        }

        [PunRPC]
        private void onSpriteFlipRight()
        {
            this.GetComponent<SpriteRenderer>().sprite = spriteRight;
            facingRight = true;
        }
        [PunRPC]
        private void onSpriteFlipLeft()
        {
            this.GetComponent<SpriteRenderer>().sprite = spriteLeft;
            facingLeft = true;
        }
        [PunRPC]
        private void onSpriteFlipUp()
        {
            this.GetComponent<SpriteRenderer>().sprite = spriteUp;
            facingUp = true;
        }
        [PunRPC]
        private void onSpriteFlipDown()
        {
            this.GetComponent<SpriteRenderer>().sprite = spriteDown;
            facingDown = true;
        }

        private void shoot()
        {
            if (!devTesting)
            {
                if (facingRight == true)
                {
                    GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                }
                else if (facingLeft == true)
                {
                    GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                    obj.GetComponent<PhotonView>().RPC("changeDirection_Left", PhotonTargets.AllBuffered);
                }
                else if (facingDown == true)
                {
                    GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                    obj.GetComponent<PhotonView>().RPC("changeDirection_Down", PhotonTargets.AllBuffered);
                }
                else if (facingUp == true)
                {
                    GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                    obj.GetComponent<PhotonView>().RPC("changeDirection_Up", PhotonTargets.AllBuffered);

                }
            }
        }
        private void smoothNetMovement()
        {
            transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 10);
        }
        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                selfPos = (Vector3)stream.ReceiveNext();
            }
        }
    }
}