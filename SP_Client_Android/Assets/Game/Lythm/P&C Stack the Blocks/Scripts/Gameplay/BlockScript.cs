using UnityEngine;

/// <summary>
/// Moves the block back and forth
/// </summary>
public class BlockScript : MonoBehaviour
{
    /// <summary> Blocks oscilltes between these points </summary>
    Vector3 pointA, pointB;

    /// <summary> To store the current oscillation target </summary>
    Vector3 target;

    /// <summary>Speed of the block</summary>
    [HideInInspector]
    public float speed;

    /// <summary>
    /// Sets the two points.
    /// </summary>
    /// <param name="point1">start position.</param>
    /// <param name="point2">end position.</param>
    public void setPositions(Vector3 point1, Vector3 point2)
    {
        pointA = point1;
        pointB = point2;
        target = pointA;
    }


    void Update()
    {
        //If it has reached the target (approximately), switch the target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointB) ? pointA : pointB;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
