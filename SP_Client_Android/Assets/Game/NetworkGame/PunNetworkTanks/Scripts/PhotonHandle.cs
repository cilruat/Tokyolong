using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PhotonGame
{

    public class PhotonHandle : MonoBehaviour
    {

        public PhotonButtons photonB;
        public byte numberOfPlayers = 4;
        public GameObject mainPlayer;


        private void Awake()
        {
            DontDestroyOnLoad(this.transform);

            PhotonNetwork.sendRate = 30;
            PhotonNetwork.sendRateOnSerialize = 20;

            SceneManager.sceneLoaded += OnSceneFinishLoading;
        }
        public void createNewRoom()
        {
            PhotonNetwork.CreateRoom(photonB.createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
        }
        public void joinOrCreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = numberOfPlayers;
            PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput.text, roomOptions, TypedLobby.Default);
        }

        public void moveScene()
        {
            PhotonNetwork.LoadLevel("Room for 1");
        }
        private void OnJoinedRoom()
        {
            moveScene();

            Debug.Log("We are connected to the room!");
        }
        private void OnSceneFinishLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Room for 1")
            {
                spawnPlayer();
            }
        }
        private void spawnPlayer()
        {
            PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);
        }
    }
}