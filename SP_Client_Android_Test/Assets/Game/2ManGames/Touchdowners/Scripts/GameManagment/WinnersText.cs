using UnityEngine;

namespace Touchdowners
{

    // Text that will be shown when game finished
    public class WinnersText : MonoBehaviour
    {
        [SerializeField] private GameObject _1PlayerWinsObject;
        [SerializeField] private GameObject _2PlayerWinsObject;

        private void Awake()
        {
            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
            gameManager.OnGameFinishedEvent += SetWinnerText;
        }

        private void SetWinnerText(PlayerType winnerPlayerType)
        {
            if (winnerPlayerType == PlayerType.LeftPlayer)
                _1PlayerWinsObject.SetActive(true);
            else if (winnerPlayerType == PlayerType.RightPlayer)
                _2PlayerWinsObject.SetActive(true);
        }
    }

}