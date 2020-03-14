using UnityEngine;
using System.Collections;
namespace Takgu
{
    /// <summary>
    /// This script controls the movement of racket
    /// </summary>
    public class PlayerController : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0) && Time.timeScale != 0)
            {
                //Change targetPos to mouse position
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //move the racket's x positon to targerPos
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, -4.5f, 0), 10);
            }
        }
    }
}