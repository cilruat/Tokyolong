using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour {
    public List<EnemyParam> enemyParam; //The list keeps in itself all the data on the enemies, their prefabs, coins for murder and the controller of animations
    public List<GameObject> place; // list of all places where enemies appear
    public float timeToRespawn; //Time before the next enemy is come
    public float moveSpeed; // speed of movement of enemies


    public static PlaceManager placeManagerScript;

    int lastRadomCount; // contains in itself, the last place where the enemy appeared
    int simultaneouslyEnemies; // responsible for the number of enemies appearing simultaneously
    float waitSomeTimes = 0.05f;
    float timeCount;

    // Use this for initialization
    void Start () {
        timeCount = timeToRespawn;
        placeManagerScript = gameObject.GetComponent<PlaceManager>();
        for (int i = 0; i < place.Count; i++) {
            place[i].GetComponent<BaseEnemy>().boxColliderParent.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (HealPoint.GameOver == false) {
            TimerForRespawn();
            WaitSomeTimes();
        }
    }

    void TimerForRespawn() { // The method reads the time until the appearance of the next enemy. at the end of the time counter, we check for free space, if it is, we create an enemy, if not, we add it to the time counter of 1 second
        if (timeCount > 0) {
            timeCount -= Time.deltaTime;
        }
        else {
            if (CheckAllPosition()) {
                SimultaneouslyEnemiesCount();
                RespawnEnemy();
                timeCount = timeToRespawn;
            }
            else {
                timeCount++;
            }
        }
    }

    void SimultaneouslyEnemiesCount() { // Randomly choose the number of simultaneously appearing monsters for 1 respawn
        int count = Random.Range(0, 100);
        if (count < 70) {
            simultaneouslyEnemies = 1;
        } else if (count >= 70 && count < 90) {
            simultaneouslyEnemies = 2;
        } else if (count >= 90 && count < 95) {
            simultaneouslyEnemies = 3;
        } else if (count >= 95 && count < 98) {
            simultaneouslyEnemies = 4;
        } else if (count >= 98 && count < 100) {
            simultaneouslyEnemies = 5;
        }
    }

    void RespawnEnemy() { // The method for the selected position initializes the enemy. If the variable "simultaneouslyEnemies" is greater than 1, it will be called again and again until the variable "simultaneouslyEnemies" becomes 0. Thus, we create several enemies simultaneously
        if (simultaneouslyEnemies > 0) {
            RandomCount();
            int enemyParamCount = Random.Range(0, enemyParam.Count);
            place[lastRadomCount].GetComponent<BaseEnemy>().Init(enemyParam[enemyParamCount].animatorController, enemyParam[enemyParamCount].enemyType, moveSpeed, enemyParam[enemyParamCount].scoreForKill);
            simultaneouslyEnemies--;
            RespawnEnemy();
        }
    }

    void RandomCount() { // The method selects the position of the appearance of the enemy. If we are in the previous place or it is already occupied, then we will call again, after a short time
        int randomCount = Random.Range(0, place.Count);
        if (randomCount == lastRadomCount)
        {
            waitSomeTimes = 0.05f;
        }
        else if (place[randomCount].GetComponent<BaseEnemy>().enemyState != EnumEnemyState.INST) {
            waitSomeTimes = 0.05f;
        } else {
            lastRadomCount = randomCount;
        }
    }
    void WaitSomeTimes() // Delay before restarting the method RandomCount(). This is necessary so that the method is not called a large number of times in the empty
    {
        if (waitSomeTimes > 0) {
            waitSomeTimes -= Time.deltaTime;
        }
        else {
            RandomCount();
        }
    }

    // We check all positions for available space for the appearance of the enemy. The variable "count" is responsible for the number of occupied places. If busy, we add +1. If the variable is equal to the total number of places, then we send false, which means that all the seats are occupied and you need to wait a short time.
    bool CheckAllPosition() { 
        int count = 0;
        for (int i = 0; i < place.Count; i++) {
            if (place[i].GetComponent<BaseEnemy>().enemyState == EnumEnemyState.MOVE) {
                count++;
                if (count == place.Count) {
                    return false;
                }
            }
        }
        return true;
    }

    public void StopGame() { // The method is responsible for stopping the game.We sort through all the enemies and call the method ReInit();
        for (int i = 0; i < place.Count; i++) {
            place[i].GetComponent<BaseEnemy>().ReInit();
        }
    }
}
