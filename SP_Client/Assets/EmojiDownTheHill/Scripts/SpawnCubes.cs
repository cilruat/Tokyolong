using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This class will spawn new wave of block
/// Actually the blocks were created when Start() was called
/// not until the  camera view port reached the blocks did they become visible
/// </summary>
public class SpawnCubes : MonoBehaviour {
	public GameObject wave;
	public Transform theWall;
	public List<GameObject> myWaves;
	public int length=20;
	private const float thethreeFourthLengthOfTheSprite = 0.715f;
	private const float startPositionForTheNextWave = 0.425f;
	int lastIndex =0;
	Vector2 lastWave= new Vector2();

	// Use this for initialization
	void Awake () {
		bool itHas3Holes = false;
		for (int i = 0; i < length; i++) {

			var f = Instantiate (wave) as GameObject;
			f.transform.SetParent (theWall);
			// starting waves aren't supposed to have holes
			if(i<8){
				f.GetComponent<CubeWave> ().has5Cubes = true;
			}
			if (i < 3) {
				f.GetComponent<CubeWave> ().isStartingWaves = true;
			}
			// every wave that is divisible to 11  has no vacant places
			if (i % 11 == 0 ) {
				f.GetComponent<CubeWave> ().has5Cubes = true;
			}

			// If the above wave already has 3 holes, the next wave can only have maximum 2 holes
			// this is to prevent generating of hill with no way to go through.
			if (itHas3Holes) {
				f.GetComponent<CubeWave> ().has5Cubes = true;
				itHas3Holes = false;
			}
			if (f.GetComponent<CubeWave> ().has3Holes == true) {
				itHas3Holes = true;
			}

			myWaves.Add (f);

			lastIndex++;
		}
	}


	void Start(){

		for (int i = 0; i <myWaves.Count; i++) {
			if (i % 2 != 0) {

				myWaves [i].transform.position = new Vector2 (-startPositionForTheNextWave, i * -thethreeFourthLengthOfTheSprite);

			} else {
				myWaves [i].transform.position = new Vector2 (0, i * -thethreeFourthLengthOfTheSprite);

			}
		}
	}

	public void CreateNewWave(){

		for (int i = 0; i < myWaves.Count; i++) {
			if (myWaves [i].activeInHierarchy == false) {
				myWaves [i].gameObject.SetActive (true);
				break;
			}
		}
		return;
	}

	public void InstantiateNewWave(){
		var f = Instantiate (wave);
		lastWave = myWaves [myWaves.Count-1].transform.position;
	
		if (lastIndex % 2 != 0) {
			f.transform.position = new Vector2 (-startPositionForTheNextWave,lastWave.y -thethreeFourthLengthOfTheSprite);
		} else {
			f.transform.position = new Vector2 (0,lastWave.y -thethreeFourthLengthOfTheSprite);
		}
	
		f.transform.SetParent (theWall);
		myWaves.Add (f);
		lastIndex++;
	}
		
}
