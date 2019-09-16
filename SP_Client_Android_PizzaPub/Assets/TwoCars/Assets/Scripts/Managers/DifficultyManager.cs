//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                 *
//   * Facebook: https://goo.gl/5YSrKw											     *
//   * Contact me: https://goo.gl/y5awt4								             *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    public float obstacleSpeed;
    public float spawnInterval;

    public float difficultyModifier;

    public void IncreaseDifficulty()
    {
        spawnInterval -= difficultyModifier;
    }

    public void ResetDifficulty()
    {
        obstacleSpeed = 8;
        spawnInterval = 1;
    }
}
