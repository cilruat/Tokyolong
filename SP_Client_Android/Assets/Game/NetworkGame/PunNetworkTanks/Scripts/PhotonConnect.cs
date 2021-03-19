using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PhotonGame
{

    public class PhotonConnect : MonoBehaviour
    {

        public string versionName = "1.0";
        public GameObject sectionView1, sectionView2, sectionView3;

        public void connectToPhoton()
        {
            PhotonNetwork.ConnectUsingSettings(versionName);

            Debug.Log("Connecting to photon...");
        }

        private void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);

            Debug.Log("We are connected to photon master");
        }

        private void OnJoinedLobby()
        {
            sectionView1.SetActive(false);
            sectionView2.SetActive(true);

            Debug.Log("On Joined Lobby");
        }

        private void OnDisconnectedFromPhoton()
        {
            if (sectionView1.active)
            {
                sectionView1.SetActive(false);
            }
            if (sectionView2.active)
            {
                sectionView2.SetActive(false);
            }
            sectionView3.SetActive(true);

            Debug.Log("Disconnected from photon service");
        }
    }
}