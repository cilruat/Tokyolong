using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// YOU BETTER RUN By BITBOYS STUDIO.
public class ScrollRectSnap : MonoBehaviour { // Please be carefull editing this script to avoid bad behaviors.

	// Public variables.
	public RectTransform panel; // TO hold the ScrollPanel
	public Button[] buttons;
	public RectTransform center; // Center to comprare the distance for eache button.

	public int startButton = 1;

	//Private Variables.
	private float[] distance; // All buttons distance to the center
	public float[] distanceReposition;
	private bool dragging = false; // Will be true, while we drag the panel
	private int buttonDistance; // Will hold tje distance between buttons
	private int minButtonNum; // To hold the number of the button, with smallest distance to center.
	private int buttonLenght;
	public float movementForce = 2.5f;
	private bool messageSend = false;
	public float distanceToMessage = 15f;

	public bool overCharacter01 = false;
	public bool overCharacter02 = false;
	public bool overCharacter03 = false;
	public bool overCharacter04 = false;
	public bool overCharacter05 = false;
	public bool overCharacter06 = false;
	public bool overCharacter07 = false;
	public bool overCharacter08 = false;
	public bool overCharacter09 = false;
	public bool overCharacter10 = false;
	public bool overCharacter11 = false;
	public bool overCharacter12 = false;
	public bool overCharacter13 = false;


	void Start () {


		buttonLenght = buttons.Length;
		distance = new float [buttonLenght];
		distanceReposition = new float[buttonLenght];

		// Get distance between buttons

		buttonDistance = (int)Mathf.Abs (buttons [1].GetComponent<RectTransform> ().anchoredPosition.x - buttons [0].GetComponent<RectTransform> ().anchoredPosition.x);
		//print (buttonDistance);


			panel.anchoredPosition = new Vector2 ((startButton - 1) * -300, 0f);



	} 
	
	void Update () {


		for (int i = 0; i < buttons.Length; i++)
		{
			distanceReposition[i] = center.GetComponent<RectTransform>().position.x - buttons[i].GetComponent<RectTransform>().position.x;
			distance [i] = Mathf.Abs (distanceReposition[i]);


			if (distanceReposition[i] > 2500) {

				float curX = buttons [i].GetComponent<RectTransform> ().anchoredPosition.x;
				float curY = buttons [i].GetComponent<RectTransform> ().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX + (buttonLenght * buttonDistance), curY);
				buttons [i].GetComponent<RectTransform> ().anchoredPosition = newAnchoredPos; 
			}

			if (distanceReposition [i] < -2500) {

				float curX = buttons [i].GetComponent<RectTransform> ().anchoredPosition.x;
				float curY = buttons [i].GetComponent<RectTransform> ().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX - (buttonLenght * buttonDistance), curY);
				buttons [i].GetComponent<RectTransform> ().anchoredPosition = newAnchoredPos; 

			}

				
		}

		float minDistance = Mathf.Min (distance); // Get the minimum distance.

		for (int a = 0; a < buttons.Length; a++)
		{

			if (minDistance == distance [a]) {

				minButtonNum = a;

			}
		}

		if (!dragging) {

			//LerpToButtons (minButtonNum * -buttonDistance);
			LerpToButtons (-buttons[minButtonNum].GetComponent<RectTransform>().anchoredPosition.x);
		}

		///////////////////////////////////////////////////////////////////////////

		// We set this to know if the center of the character selection is over one character or other. And use it to change the "Select/Unclock" button.
		//BUTTON1
		if (buttons [minButtonNum].name == ("Button1")) {
			overCharacter01 = true;
		} else {
			overCharacter01 = false;
		}
		//BUTTON2
		if (buttons [minButtonNum].name == ("Button2")) {
			overCharacter02 = true;
		} else {
			overCharacter02 = false;
		}
		//BUTTON3
		if (buttons [minButtonNum].name == ("Button3")) {
			overCharacter03 = true;
		} else {
			overCharacter03 = false;
		}
		//BUTTON4
		if (buttons [minButtonNum].name == ("Button4")) {
			overCharacter04 = true;
		} else {
			overCharacter04 = false;
		}
		//BUTTON5
		if (buttons [minButtonNum].name == ("Button5")) {
			overCharacter05 = true;
		} else {
			overCharacter05 = false;
		}
		//BUTTON6
		if (buttons [minButtonNum].name == ("Button6")) {
			overCharacter06 = true;
		} else {
			overCharacter06 = false;
		}
		//BUTTON7
		if (buttons [minButtonNum].name == ("Button7")) {
			overCharacter07 = true;
		} else {
			overCharacter07 = false;
		}
		//BUTTON8
		if (buttons [minButtonNum].name == ("Button8")) {
			overCharacter08 = true;
		} else {
			overCharacter08 = false;
		}
		//BUTTON9
		if (buttons [minButtonNum].name == ("Button9")) {
			overCharacter09 = true;
		} else {
			overCharacter09 = false;
		}
		//BUTTON10
		if (buttons [minButtonNum].name == ("Button10")) {
			overCharacter10 = true;
		} else {
			overCharacter10 = false;
		}
		//BUTTON11
		if (buttons [minButtonNum].name == ("Button11")) {
			overCharacter11 = true;
		} else {
			overCharacter11 = false;
		}
		//BUTTON12
		if (buttons [minButtonNum].name == ("Button12")) {
			overCharacter12 = true;
		} else {
			overCharacter12 = false;
		}
		//BUTTON13
		if (buttons [minButtonNum].name == ("Button13")) {
			overCharacter13 = true;
		} else {
			overCharacter13 = false;
		}


	


		///////////////////////////////////////////////////////////////////////////


	}

	void LerpToButtons (float position){

		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.unscaledDeltaTime * movementForce);

		if (Mathf.Abs (position - newX) < distanceToMessage)
			newX = position;

		if (Mathf.Abs (newX) >= Mathf.Abs (position) - 1f && Mathf.Abs (newX) <= Mathf.Abs (position) + 1 && !messageSend) {
			
			messageSend = true;
		//	Debug.Log (buttons [minButtonNum].name);

		} 

		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag(){

		messageSend = false;
		dragging = true;
	}

	public void EndDrag(){

		dragging = false;
	}

	public void UpdateButton(){

		if (CharacterSelection.characterSelected == 1) {

			panel.anchoredPosition = new Vector2 ((startButton - 1) * -300, 0f);

		}

		if (CharacterSelection.characterSelected == 2) {

			panel.anchoredPosition = new Vector2 ((startButton + 1) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 3) {

			panel.anchoredPosition = new Vector2 ((startButton + 2) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 4) {

			panel.anchoredPosition = new Vector2 ((startButton + 4) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 5) {

			panel.anchoredPosition = new Vector2 ((startButton + 6) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 6) {

			panel.anchoredPosition = new Vector2 ((startButton + 8) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 7) {

			panel.anchoredPosition = new Vector2 ((startButton + 10) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 8) {

			panel.anchoredPosition = new Vector2 ((startButton + 12) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 9) {

			panel.anchoredPosition = new Vector2 ((startButton + 14) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 10) {

			panel.anchoredPosition = new Vector2 ((startButton + 16) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 11) {

			panel.anchoredPosition = new Vector2 ((startButton + 18) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 12) {

			panel.anchoredPosition = new Vector2 ((startButton + 20) * -300, 0f);

		}
		if (CharacterSelection.characterSelected == 13) {

			panel.anchoredPosition = new Vector2 ((startButton + 22) * -300, 0f);

		}
	}
}
