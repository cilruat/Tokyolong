using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PhotonGame
{

    public class GameManager : Photon.PunBehaviour
    {
        #region Public Variables
        static public GameManager Instance;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        private GameObject[] spawnSpots;
        private GameObject spawnSpotToUse;

        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            Instance = this;
            spawnSpots = GameObject.FindGameObjectsWithTag("Respawn");
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red>Missing</Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    spawnSpotToUse = spawnSpots[UnityEngine.Random.Range(0, spawnSpots.Length)];
                    Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, spawnSpotToUse.transform.position, spawnSpotToUse.transform.rotation, 0);
                }
                else
                {
                    Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        #endregion

        #region Photon Messages
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom() { }
        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
                LoadArena();
            }
        }
        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
                LoadArena();
            }
        }
        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.room.PlayerCount);
        }
        #endregion
    }
}