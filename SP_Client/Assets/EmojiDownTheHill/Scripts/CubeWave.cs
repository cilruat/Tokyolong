using UnityEngine;
using System.Collections;

namespace Emoji2
{
    public class CubeWave : MonoBehaviour
    {
        public GameObject[] myCubes;

        int countVacancy;
        public bool has5Cubes;
        [HideInInspector]	public	float value;
        public bool has3Holes;
        public bool isBeingReduced;
        public bool isStartingWaves;

        // Use this for initialization
        void Start()
        {
            foreach (GameObject o in myCubes)
            {
                o.GetComponent<TheTerrian>().isOwningASpring = true;
            }
            value = 0.45f; // the starting % to have a hole in this line
            PunchHolesOnANewWave();


            GameManager.Instance.boundsX =	myCubes[0].GetComponent<SpriteRenderer>().bounds.size.x;
            GameManager.Instance.boundsY =	myCubes[0].GetComponent<SpriteRenderer>().bounds.size.y;
        }


        public void DeleteCube(GameObject o)
        {
            o.GetComponent<SpriteRenderer>().enabled = false;
            o.transform.tag = "vacant";
            var k = o.transform.Find("Coin");
            var a = o.transform.Find("Spring");
            var l = o.transform.Find("Refill Cube");
            var m = o.transform.Find("tree");
            if (k != null)
                k.gameObject.SetActive(false);
            if (a != null)
                a.gameObject.SetActive(false);
            if (l != null)
                l.gameObject.SetActive(false);
            if (m != null)
            {
                m.gameObject.SetActive(false);
            }
            o.GetComponent<TheTerrian>().isOwningASpring = false;
        }

        public void PunchHolesOnANewWave()
        {
            DeleteCube(myCubes[0]);
            DeleteCube(myCubes[myCubes.Length - 1]);
            if (!has5Cubes)
            {

                for (int index = 1; index < myCubes.Length - 1; index++)
                {
                    // refill cube appearance rate
                    bool n = Random.value < (1 - GameManager.Instance.treeFrequency);
                    if (n)
                    {
                        var a = myCubes[index].transform.Find("tree");
                        if (a != null)
                        {
                            a.gameObject.SetActive(false);
                        }
                    }


                    // tree appearance rate
                    bool z = Random.value < (1 - GameManager.Instance.refillCubeFrequency);
                    if (z)
                    {
                        var i = myCubes[index].transform.Find("Refill Cube");
                        if (i != null)
                        {
                            i.gameObject.SetActive(false);
                        }
                    }

                    //spring appear rate
                    bool g = Random.value < (1 - GameManager.Instance.springFrequency);
                    if (g)
                    {
                        var o = myCubes[index].transform.Find("Spring");
                        if (o != null)
                        {
    						
                            o.gameObject.SetActive(false);
    					
                        }
                        myCubes[index].GetComponent<TheTerrian>().isOwningASpring = false;
                    }

                    if (!isBeingReduced)
                    {
                        if (countVacancy < 3)
                        {

                            bool k = Random.value < value;
                            if (k)
                            {
                                DeleteCube(myCubes[index]);
                                countVacancy++;
                                value -= 0.35f;
                            }

                        }
                        if (countVacancy == 3)
                        {
                            has3Holes = true;
                        }
                    }
                    else
                    {
                        if (countVacancy < 2)
                        {

                            bool k = Random.value < value;
                            if (k)
                            {
                                DeleteCube(myCubes[index]);
                                countVacancy++;
                                value -= 0.95f;
                            }
                        }
                    }
                }
            }
            else
            {
                //the drop rate on a 5 cube wave (aka a starting wave)
                for (int index = 1; index < myCubes.Length - 1; index++)
                {
                    // refill cube appearance rate
                    bool n = Random.value < (1 - GameManager.Instance.treeFrequency);
                    if (n)
                    {
                        var a = myCubes[index].transform.Find("tree");
                        if (a != null)
                        {

                            a.gameObject.SetActive(false);
                        }
                    }
                    if (isStartingWaves)
                    {
                        var a = myCubes[index].transform.Find("tree");
                        if (a != null)
                        {

                            a.gameObject.SetActive(false);
                        }
                    }

                    // tree appearance rate
                    bool z = Random.value < (1 - GameManager.Instance.refillCubeFrequency);
                    if (z)
                    {
                        var i = myCubes[index].transform.Find("Refill Cube");
                        if (i != null)
                        {

                            i.gameObject.SetActive(false);
                        }
                    }

                    //spring appearance rate
                    bool g = Random.value < (1 - GameManager.Instance.springFrequency);
                    if (g)
                    {
                        var o = myCubes[index].transform.Find("Spring");
                        if (o != null)
                        {
    						
                            o.gameObject.SetActive(false);

                        }
                        myCubes[index].GetComponent<TheTerrian>().isOwningASpring = false;
                    }
                }
            }
        }

        public void Refill()
        {

            for (int i = 0; i < myCubes.Length; i++)
            {
                if (i > 0 && i < myCubes.Length - 1)
                {
                    if (myCubes[i].GetComponent<SpriteRenderer>().enabled == false)
                    {
                        myCubes[i].GetComponent<SpriteRenderer>().enabled = true;
                        myCubes[i].transform.tag = "ground";
                        myCubes[i].GetComponent<Animator>().SetTrigger("Appear");
                    }
                }
            }
        }

    }
}