using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PhotonGame
{

    public class PlayerUI : MonoBehaviour
    {
        #region Public Properties
        [Tooltip("UI Text to display Player's Name")]
        public Text PlayerNameText;
        [Tooltip("UI Slider to display Player's Health")]
        public Slider PlayerHealthSlider;
        [Tooltip("Pixel offset from the player target")]
        public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);
        private Vector3 currentVelocity;
        public float smooth = 0;
        #endregion

        #region Private Properties
        PlayerManager _target;
        Transform _targetTransform;
        SpriteRenderer _targetRenderer;
        Vector3 _targetPosition;
        #endregion

        #region MonoBehaviour Messages
        void Awake()
        {
            //this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        }
        void Update()
        {
            // Reflect the Player Health
            if (PlayerHealthSlider != null)
            {
                PlayerHealthSlider.value = _target.Health;
            }
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (_target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        /// <summary>
        /// MonoBehaviour method called after all Update functions have been called. This is useful to order script execution.
        /// In our case since we are following a moving GameObject, we need to proceed after the player was moved during a particular frame.
        /// </summary>
        void LateUpdate()
        {
            // #Critical
            // Follow the Target GameObject on screen.
            if (_targetTransform != null)
            {
                //_targetPosition = _targetTransform.position;
                //_targetPosition.y += _targetTransform.position.y;
                //this.transform.position = Camera.main.WorldToViewportPoint(_targetPosition);

                // Vector3 newCameraPosition = new Vector3(_target.transform.position.x, _target.transform.position.y, _target.transform.position.z);
                //transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref currentVelocity, smooth);
            }
        }
        #endregion

        #region Public Methods
        public void SetTarget(PlayerManager target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red>Missing</Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            _target = target;
            _targetTransform = _target.GetComponent<Transform>();
            _targetRenderer = _target.GetComponent<SpriteRenderer>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (PlayerNameText != null)
            {
                PlayerNameText.text = _target.photonView.owner.NickName;
            }
        }
        #endregion
    }
}