using UnityEngine;

namespace ZombieDriveGame
{
    /// <summary>
    /// This script defines an object which can interact with the player in various ways. A touchable may be a rock or a railing that
    /// bounces the player back, or it can be a zombie that the player can run over, or it can be an item that can be collected.
    /// </summary>
    public class ZDGForceToCenter : MonoBehaviour
    {
        void Start()
        {
            transform.position = new Vector3( 0, 0, transform.position.z);
        }
    }
}