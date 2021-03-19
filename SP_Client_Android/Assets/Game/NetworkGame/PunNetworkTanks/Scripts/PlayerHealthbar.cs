using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace PhotonGame
{

    public class PlayerHealthbar : Photon.MonoBehaviour
    {

        public PlayerMove PlayerMove;
        public PhotonView photonView;
        public GameObject localPlayer_Canvas;
        public GameObject otherPlayer_Canvas;
        public Image localPlayer_HealthBar;
        public Image otherPlayer_HealthBar;
        public Vector3 localPlayerName_pos;
        public Vector3 otherPlayerName_pos;

        void Awake()
        {
            if (!PlayerMove.devTesting)
            {
                setCorrectCanvas();
            }
        }

        void setCorrectCanvas()
        {
            if (photonView.isMine)
            {
                PlayerMove.playerName.GetComponent<RectTransform>().anchoredPosition = localPlayerName_pos;
                localPlayer_Canvas.SetActive(true);
            }
            else
            {
                PlayerMove.playerName.GetComponent<RectTransform>().anchoredPosition = otherPlayerName_pos;
                otherPlayer_Canvas.SetActive(true);
            }
        }

        [PunRPC]
        public void reduceHealt()
        {
            reduceHealthAmount(0.2f);
        }


        public void reduceHealthAmount(float hit)
        {
            if (photonView.isMine)
            {
                localPlayer_HealthBar.fillAmount -= hit;
            }
            else
            {
                otherPlayer_HealthBar.fillAmount -= hit;
            }
        }

    }
}
