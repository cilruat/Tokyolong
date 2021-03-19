using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PhotonGame
{

    public class PhotonButtons : MonoBehaviour
    {

        public PhotonHandle pHandle;
        public InputField joinRoomInput, createRoomInput;

        public void onClickCreateRoom()
        {
            pHandle.createNewRoom();
        }

        public void onClickJoinRoom()
        {
            pHandle.joinOrCreateRoom();
        }

    }
}