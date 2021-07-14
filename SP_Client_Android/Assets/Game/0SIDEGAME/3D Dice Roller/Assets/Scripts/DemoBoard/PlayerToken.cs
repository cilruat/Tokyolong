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
    public int stackSoju = 0;
    public Text textSoju;
    public Text textSoju2;



    public List<GameObject> TileImageList = new List<GameObject>();

    public GameObject ShowPanel;

    void Awake()
    {
        finalTile = startingTile;
        targetPosition = this.transform.position;
        stackSoju = 0;
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

        // 이곳에 코루틴 넣어서 SpaceTomove만큼의 wating을 주고 Endmoveenvent를 invoke 하는게 맞지않을까?
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
        StartCoroutine(WatingMove());
    }


    public void TileCheck()
    {
        //Debug.Log(finalTile.name);

        if(finalTile.name == "Tile_Holder_1")
        {
            GameObject Tile1 = ShowPanel.transform.GetChild(1).gameObject;
            Tile1.SetActive(true);
            Debug.Log("1");

        }
        else if (finalTile.name == "Tile_Holder_2")
        {
            GameObject Tile2 = ShowPanel.transform.GetChild(2).gameObject;
            Tile2.SetActive(true);
            Debug.Log("2");

        }

        else if (finalTile.name == "Tile_Holder_3")
        {
            GameObject Tile3 = ShowPanel.transform.GetChild(3).gameObject;
            Tile3.SetActive(true);
            Debug.Log("3");

        }

        else if (finalTile.name == "Tile_Holder_4")
        {
            GameObject Tile4 = ShowPanel.transform.GetChild(4).gameObject;
            Tile4.SetActive(true);
            Debug.Log("4");
        }

        else if (finalTile.name == "Tile_Holder_5")
        {
            GameObject Tile5 = ShowPanel.transform.GetChild(5).gameObject;
            Tile5.SetActive(true);
            Debug.Log("5");
        }

        else if (finalTile.name == "Tile_Holder_6")
        {
            GameObject Tile6 = ShowPanel.transform.GetChild(6).gameObject;
            Tile6.SetActive(true);


            textSoju2.text = stackSoju.ToString();



            Debug.Log("6");
        }

        else if (finalTile.name == "Tile_Holder_7")
        {
            GameObject Tile7 = ShowPanel.transform.GetChild(7).gameObject;
            Tile7.SetActive(true);
            Debug.Log("7");
        }

        else if (finalTile.name == "Tile_Holder_8")
        {
            GameObject Tile8 = ShowPanel.transform.GetChild(8).gameObject;
            Tile8.SetActive(true);
            Debug.Log("8");
        }

        else if (finalTile.name == "Tile_Holder_9")
        {
            GameObject Tile9 = ShowPanel.transform.GetChild(9).gameObject;
            Tile9.SetActive(true);
            Debug.Log("9");
        }

        else if (finalTile.name == "Tile_Holder_10")
        {
            GameObject Tile10 = ShowPanel.transform.GetChild(10).gameObject;
            Tile10.SetActive(true);
            Debug.Log("10");
        }

        else if (finalTile.name == "Tile_Holder_11")
        {
            GameObject Tile11 = ShowPanel.transform.GetChild(11).gameObject;
            Tile11.SetActive(true);
            Debug.Log("11");
        }

        else if (finalTile.name == "Tile_Holder_12")
        {
            GameObject Tile12 = ShowPanel.transform.GetChild(12).gameObject;
            Tile12.SetActive(true);
            Debug.Log("12");
        }

        else if (finalTile.name == "Tile_Holder_13")
        {
            GameObject Tile13 = ShowPanel.transform.GetChild(13).gameObject;
            Tile13.SetActive(true);
            Debug.Log("13");
        }

        else if (finalTile.name == "Tile_Holder_14")
        {
            GameObject Tile14 = ShowPanel.transform.GetChild(14).gameObject;
            Tile14.SetActive(true);
            Debug.Log("14");
        }

        else if (finalTile.name == "Tile_Holder_15")
        {
            GameObject Tile15 = ShowPanel.transform.GetChild(15).gameObject;
            Tile15.SetActive(true);
            Debug.Log("15");
        }

        else if (finalTile.name == "Tile_Holder_16")
        {
            GameObject Tile16 = ShowPanel.transform.GetChild(16).gameObject;
            Tile16.SetActive(true);
            Debug.Log("16");
        }

        else if (finalTile.name == "Tile_Holder_17")
        {
            GameObject Tile17 = ShowPanel.transform.GetChild(17).gameObject;
            Tile17.SetActive(true);
            Debug.Log("17");
        }

        else if (finalTile.name == "Tile_Holder_18")
        {
            GameObject Tile18 = ShowPanel.transform.GetChild(18).gameObject;
            Tile18.SetActive(true);
            Debug.Log("18");
        }

        else if (finalTile.name == "Tile_Holder_19")
        {
            GameObject Tile19 = ShowPanel.transform.GetChild(19).gameObject;
            Tile19.SetActive(true);
            Debug.Log("19");
        }

        else if (finalTile.name == "Tile_Holder_20")
        {
            GameObject Tile20 = ShowPanel.transform.GetChild(20).gameObject;
            Tile20.SetActive(true);
            Debug.Log("20");
        }

        else if (finalTile.name == "Tile_Holder_21")
        {
            GameObject Tile21 = ShowPanel.transform.GetChild(21).gameObject;
            Tile21.SetActive(true);
            Debug.Log("21");
        }

        else if (finalTile.name == "Tile_Holder_22")
        {
            GameObject Tile22 = ShowPanel.transform.GetChild(22).gameObject;
            Tile22.SetActive(true);
            Debug.Log("22");
        }

        else if (finalTile.name == "Tile_Holder_23")
        {
            GameObject Tile23 = ShowPanel.transform.GetChild(23).gameObject;
            Tile23.SetActive(true);
            Debug.Log("23");
        }

        else if (finalTile.name == "Tile_Holder_24")
        {
            GameObject Tile24 = ShowPanel.transform.GetChild(24).gameObject;
            Tile24.SetActive(true);
            stackSoju++;
            textSoju.text = stackSoju.ToString();

            Debug.Log("24");
        }

        else if (finalTile.name == "Tile_Holder_25")
        {
            GameObject Tile24 = ShowPanel.transform.GetChild(25).gameObject;
            Tile24.SetActive(true);
            Debug.Log("25");
        }




        else if (finalTile.name == "Tile_Holder_Start")
        {
            GameObject Tile0 = ShowPanel.transform.GetChild(0).gameObject;
            Tile0.SetActive(true);
            Debug.Log("0");
        }






        /*
        switch (finalTile.name)
        {

            case "Tile_Holder_1":

                GameObject Tile1 = ShowPanel.transform.GetChild(0).gameObject;
                Tile1.SetActive(true);
                break;

            case "Tile_Holder_2":

                GameObject Tile2 = ShowPanel.transform.GetChild(1).gameObject;
                Tile2.SetActive(true);
                break;

            case "Tile_Holder_3":

                GameObject Tile3 = ShowPanel.transform.GetChild(2).gameObject;
                Tile3.SetActive(true);
                break;

            case "Tile_Holder_4":

                GameObject Tile4 = ShowPanel.transform.GetChild(3).gameObject;
                Tile4.SetActive(true);
                break;

            case "Tile_Holder_5":

                GameObject Tile5 = ShowPanel.transform.GetChild(4).gameObject;
                Tile5.SetActive(true);
                break;


            case "details":
                //TBD
                break;
        }*/

    }


    public void ShowTile()
    {


    }



    IEnumerator WatingMove()
    {
        yield return new WaitForSeconds(diceManager.totalValue * 0.5f);
        EndMoveEvent.Invoke();
    }
}