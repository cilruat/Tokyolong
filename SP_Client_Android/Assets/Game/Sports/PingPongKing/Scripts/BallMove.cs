using UnityEngine;
using System.Collections;

namespace Takgu
{
    /// <summary>
    /// This script controls movement of ball
    /// </summary>
    public class BallMove : MonoBehaviour
    {
        //speed at which ball moves
        int speed;
        //speed at which ball bounces
        int BounceSpeed;
        void Start()
        {
            //getting value of speed and BounceSpeed from the GameController
            GameObject go = GameObject.Find("GameController");
            GameController speedController = go.GetComponent<GameController>();
            speed = speedController.speed;
            BounceSpeed = speedController.BounceSpeed;
        }
        // Update is called once according to physics
        void FixedUpdate()
        {
            //moving the ball
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, speed);
        }
        void BounceStart()
        {
            //after bouncing from racket ball bounces in opposite direction
            speed = BounceSpeed;
        }
    }
}