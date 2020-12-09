using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KickTheBuddy
{
    public class LoadingScene : MonoBehaviour
    {
        public string mainScene;
        // Use this for initialization
        void Start()
        {
            SceneManager.LoadSceneAsync(mainScene);//Loading end jump scene
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}