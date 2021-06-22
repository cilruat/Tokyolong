using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerToken : MonoBehaviour 
{
    [Tooltip("The tile the PlayerToken will start on.")]
    public Tile startingTile;
    public DiceManager diceManager;
    public Text valueText;
    public UnityEvent EndMoveEvent;

    Tile currentTile;

    Tile[] moveQueue;
    int moveQueueIndex;

    Vector3 targetPosition;
    Vector3 velocity;

    Tile finalTile;
	
    void Awake()
    {
        finalTile = startingTile;
        targetPosition = this.transform.position;
    }

	// Update is called once per frame
    /// <summary>
    /// Moves this object to the desired position which is set by the dice roll.
    /// </summary>
	void Update () 
    {
        if (Vector3.Distance(this.transform.position, targetPosition) > 0.03f)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, 0.2f);
        }
        else
        {
            if (moveQueue != null && moveQueueIndex < moveQueue.Length)
            {
                Tile nextTile = moveQueue[moveQueueIndex];
                SetNewTargetPosition(nextTile.transform.position);
                moveQueueIndex++;
            }
        }
	}

    void SetNewTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
        velocity = Vector3.zero;
    }

    /// <summary>
    /// Moves the player 1-6 spaces depending on value of the dice roll.
    /// </summary>
    /// 
    public void MovePlayerToken()
    {
        int spacesToMove = diceManager.totalValue;
        valueText.text = "" + spacesToMove.ToString();

        if (spacesToMove == 0)
        {
            return;
        }            

        moveQueue = new Tile[spacesToMove];

        for (int i = 0; i < spacesToMove; i++)
        {
            finalTile = finalTile.nextTile;
            moveQueue[i] = finalTile;
        }
        moveQueueIndex = 0;
        EndMoveEvent.Invoke();
    }

}