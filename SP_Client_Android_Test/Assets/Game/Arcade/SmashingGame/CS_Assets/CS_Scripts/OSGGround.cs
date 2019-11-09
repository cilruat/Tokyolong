using UnityEngine;
using System.Collections;

namespace ObjectSmashingGame
{
    public class OSGGround : MonoBehaviour
    {
        internal Transform thisTransform;
        
        // A referemce to the Game Controller, which is taken by the first time this script runs, and is remembered across all other scripts of this type
        static OSGGameController gameController;
        
        [Tooltip("The effect created when this object is hit")]
        public Transform hitEffect;
        
        // Use this for initialization
        void Start()
        {
            // Hold the gamcontroller object in a variable for quicker access
            if (gameController == null) gameController = GameObject.FindObjectOfType<OSGGameController>();
        }
        
        
        void HitObject( Vector3 hitPosition )
        {
            if (hitEffect) Instantiate(hitEffect, hitPosition, Quaternion.identity);

            gameController.ChangeLives(-1);
        }
    }
}
