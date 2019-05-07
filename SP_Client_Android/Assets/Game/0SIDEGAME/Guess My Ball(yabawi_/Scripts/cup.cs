using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class cup : MonoBehaviour {




	public int currentpointnum;  //index of current cup

	private gamemanager gamemanagerscript;

	private List<Vector3> path = new List<Vector3>();

	public float padding;



	// Use this for initialization
	void Start () {
	
		gamemanagerscript = GameObject.Find ("GameManager").GetComponent<gamemanager> ();



	}
	
	// Update is called once per frame
	void Update () {
	



	}


	void OnMouseDown() {
		if (gamemanagerscript.ifcupcanclick == true) {

			if (gameObject.transform.childCount != 0) {
				
				gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled=true;
				
				gameObject.transform.DetachChildren ();
			
				gamemanagerscript.isgamewin = true;
				gamemanagerscript.lukecount++;
				gamemanagerscript.insballaddscore(transform.position);
				gamemanagerscript.moveballback();
				Invoke("movecupback",0.6f);
				if(gamemanagerscript.lukecount<1){	
					gamemanagerscript.goon.SetActive(true);
					gamemanagerscript.countnumber.GetComponent<Text>().text=gamemanagerscript.lukecount.ToString();
				}else if (gamemanagerscript.lukecount==1){
					gamemanagerscript.youwinpanel.SetActive(true);
				}
				
			} else {
				
				gamemanagerscript.isgameover=true;
				gamemanagerscript.gameover.SetActive(true);
			}
			
			
			iTween.MoveBy (gameObject, iTween.Hash ("y", 1, "easetype", iTween.EaseType.easeOutBack, "time", 0.25f));
			gamemanagerscript.ifcupcanclick = false;
		}

	}

	private void movecupback()
	{
	
		iTween.MoveBy (gameObject, iTween.Hash ("y", -1, "easetype", iTween.EaseType.easeOutBack, "time", 0.2f));
	}

	public void move(Vector3 targetpos)
	{
		
		float midx;
		float midy;
		float timer;

		path.Clear ();
		
		midx = (transform.position.x + targetpos.x) * 0.5f;
		midy=(transform.position.y + targetpos.y) * 0.5f;
		

		midy = midy +padding;
		
		path.Add(transform.position);  
		path.Add(new Vector3(midx, midy, 0));  
		path.Add(targetpos);  
		
		timer = Random.Range (0.2f, 0.6f);
		
		iTween.MoveTo(this.gameObject, iTween.Hash("path", path.ToArray(), "time", timer,  
		                                           "easetype", iTween.EaseType.easeOutSine));  
		

	}

	}
	