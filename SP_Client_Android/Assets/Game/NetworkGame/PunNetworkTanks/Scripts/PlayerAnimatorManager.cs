using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PhotonGame
{

    public class PlayerAnimatorManager : Photon.MonoBehaviour
    {

        #region PUBLIC PROPERTIES
        public float movementSpeed = 1.5f;
        public AudioClip audioDriving = null;
        public GameObject bulletPrefab = null;
        public Transform socket = null;
        #endregion

        #region PRIVATE PROPERTIES
        private bool shootCooldown = false;
        #endregion

        #region MONOBEHAVIOUR MESSAGES
        void Update()
        {
            if (photonView.isMine == false && PhotonNetwork.connected == true)
            {
                return;
            }
            Move();
            if (Input.GetButtonDown("Fire1"))
            {
                if (shootCooldown == false)
                {
                    Shoot();
                    Invoke("ResetCooldown", 0.8f);
                    shootCooldown = true;
                }
            }
        }
        #endregion

        #region Private Methods
        private void GetMovement()
        {
            transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
            AudioSource.PlayClipAtPoint(audioDriving, transform.position, 1.0f);
        }
        private void Move()
        {
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
            {
                transform.localRotation = Quaternion.Euler(180, 0, 0);
                GetMovement();
                return;
            }
            if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                GetMovement();
                return;
            }
            if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                GetMovement();
                return;
            }
            if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                GetMovement();
                return;
            }
        }
        private void Shoot()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                PhotonNetwork.Instantiate(bulletPrefab.name, socket.position, socket.rotation, 0);
            }
        }
        private void ResetCooldown()
        {
            shootCooldown = false;
        }
        #endregion
    }
}