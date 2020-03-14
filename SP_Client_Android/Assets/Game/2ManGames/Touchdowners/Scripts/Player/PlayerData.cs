using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {

    public Vector2 velocityRight = new Vector2(6, 10);
    public Vector2 velocityLeft = new Vector2(-6, 10);

    [Range(1, 180)]
    public float minAngleForBalanceInJump = 180;


}
