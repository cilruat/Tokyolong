using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



namespace PhotonGame
{

    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte MaxPlayersPerRoom = 4;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        public GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        public GameObject progressLabel;
        #endregion


        #region Private Variables
        string gameVersion = "1";
        bool isConnecting;
        #endregion


        #region MonoBehaviour CallBacks
        void Awake()
        {
            // #Critical
            // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;
            // #NotImportant
            // Force LogLevel
            PhotonNetwork.logLevel = Loglevel;
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;
        }
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.connected)
            {
                Debug.Log("Joining Room...");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("Connecting...");
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }
        #endregion


        #region Photon.PunBehaviour CallBacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);
            Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room. 
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnectedFromPhoton()
        {
            isConnecting = false;
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.MaxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            Debug.Log("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.room.PlayerCount + " Player(s)");
            // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
                // #Critical
                // Load the Room Level. 
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }
        #endregion
    }
}