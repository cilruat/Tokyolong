using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour {

	public static gamemanager instance;

	public GameObject[] points;   //points of cup
	private GameObject[] cups;
	public GameObject ball;
	private List<GameObject> temppoints = new List<GameObject>();
	private List<GameObject> usefultargetpoints = new List<GameObject>();  
	private int targetpointindex;
	public float paddingy=1.5f;
	public bool isgameover = false;
	public bool isgamewin=false;
	public bool ifcupcanclick = false;

	public GameObject goon;
	public GameObject gameover;
	private GameObject readygo; 
	public GameObject youwinpanel;
	public GameObject countnumber;
	public int lukecount = 0;
	public GameObject[] levels;
	public int levelindex = 0;
	private GameObject templevel;
	public GameObject levelnumber ;
	public GameObject objCanvas;
	public GameObject objSendServer;
	public GameObject objGameover;

	private Vector3 ballinipos;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;		

		Application.targetFrameRate = 60;
	    templevel=	Instantiate (levels [levelindex],new Vector3 (2, 0, 0), Quaternion.identity) as GameObject;
		points = GameObject.FindGameObjectsWithTag("point");
		cups=GameObject.FindGameObjectsWithTag("cup");
		goon.SetActive (false);
		gameover.SetActive (false);
		youwinpanel.SetActive (false);
		readygo = GameObject.Find("readygo");
		ballinipos = ball.transform.position;
		levelnumber.GetComponent<Text> ().text = (levelindex+1).ToString ();
	}

	IEnumerator  move4time (){
		moveballtocup ();
		yield return new WaitForSeconds(0.7f);
		ball.GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds(0.5f);
		movecup ();
		yield return new WaitForSeconds(0.6f);
		movecup ();
		yield return new WaitForSeconds(0.6f);
		movecup ();
		yield return new WaitForSeconds(0.6f);
		movecup ();
		ifcupcanclick = true;
	}


	public void test()
	{
        if (Info.GamePlayCnt >= 1)
        {
            StartCoroutine(move4time());
            readygo.SetActive(false);
        }

        else
            SystemMessage.Instance.Add("코인이 부족합니다 1개라도 있어야해요");
    }

	public void Retry(){
		SceneChanger.LoadScene ("Main", objCanvas);
	}

	public void GoOnPlay(){

		ball.transform.SetParent (null);
		readygo.SetActive (true);	
		goon.SetActive (false);

	}

	public void nextlevelbutton(){
	
		Destroy (templevel);
		Invoke ("instatiatelevel", 0.03f);
	}

	public void insballaddscore(Vector3 pos){
	
		GameObject balltemp= Instantiate (ball, pos, Quaternion.identity) as GameObject;
		iTween.MoveTo(balltemp, iTween.Hash("position", new Vector3 (-2.26f, 4.22f, 0), "time", 0.35f,  
		                                           "easetype", iTween.EaseType.easeOutSine));  

		Destroy (balltemp, 0.35f);

	}


	void instatiatelevel(){
		levelindex++;
		levelnumber.GetComponent<Text> ().text = (levelindex+1).ToString ();
		templevel= Instantiate (levels [levelindex],new Vector3 (2, 0, 0), Quaternion.identity) as GameObject;
		youwinpanel.SetActive (false);
		readygo.SetActive (true);
		ball.transform.position = ballinipos;
		cups=GameObject.FindGameObjectsWithTag("cup");
		points = GameObject.FindGameObjectsWithTag("point");
		lukecount = 0;
	}

	private void moveballtocup()
	{
	
		int cupindex;

		cupindex = Random.Range (0, cups.Length);

		ball.transform.SetParent (cups [cupindex].transform);

		Vector3 cuppos = new Vector3 (cups [cupindex].transform.position.x, cups [cupindex].transform.position.y - 1, cups [cupindex].transform.position.z);

		iTween.MoveTo (ball, iTween.Hash ("position", cuppos, "easetype", iTween.EaseType.easeInCubic, "time", 0.6f));

	
	}

	public void moveballback()
	{
		
		Invoke ("realmoveballback", 0.4f);	

		
	}


	private void realmoveballback()
	{
	
		iTween.MoveTo (ball, iTween.Hash ("position", ballinipos, "easetype", iTween.EaseType.easeInCubic, "time", 0.2f));

	}


	private void movecup()
	{

		usefultargetpoints.Clear ();

		for (int j=0; j<points.Length; j++) {
			usefultargetpoints.Add(points[j]);
		}


		for(int i=0;i<cups.Length;i++)
		{


			temppoints.Clear();


			for (int k=0; k<usefultargetpoints.Count; k++) {


				if(cups[i].GetComponent<cup>().currentpointnum!=usefultargetpoints[k].GetComponent<point>().pointnum )
				{
					temppoints.Add(usefultargetpoints[k]);
					
				}
				
				
			}

			if(temppoints.Count>0)
			{

			targetpointindex = Random.Range (0, temppoints.Count);

			cups[i].GetComponent<cup>().currentpointnum=temppoints[targetpointindex].GetComponent<point>().pointnum;



			paddingy=-1*paddingy;

			cups[i].GetComponent<cup>().padding=paddingy;

			cups[i].GetComponent<cup>().move(temppoints[targetpointindex].transform.position);
			

			usefultargetpoints.Remove(temppoints[targetpointindex]);
			}

		}



	}

	public void Win()
	{
		youwinpanel.SetActive(true);

		if (levelindex == 2)
			StartCoroutine (_Win ());
	}

	IEnumerator _Win()
	{
		yield return new WaitForSeconds (1f);
		objSendServer.SetActive (true);

		yield return new WaitForSeconds (1f);
		objSendServer.SetActive (false);
        NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, +3);

        yield return new WaitForSeconds(0.3f);
        ReturnHome();
	}

	public void GameOver()
	{
		isgameover=true;
		gameover.SetActive(true);
        NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, -1);

        StartCoroutine(_GameOver ());
	}

	IEnumerator _GameOver()
	{
		yield return new WaitForSeconds (1f);
		objGameover.SetActive (true);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objCanvas);
	}
}
