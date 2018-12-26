using UnityEngine;

public class PlaceScript : MonoBehaviour { // the class is responsible for accepting a mouse or finger click

    public BaseEnemy myChild; // Model of the enemy in this place

    public void HitMe(bool flag) {
        if (flag) {
            myChild.HitMe(flag); // Send the touch to the class BaseEnemy for processing
        }
    }
}
