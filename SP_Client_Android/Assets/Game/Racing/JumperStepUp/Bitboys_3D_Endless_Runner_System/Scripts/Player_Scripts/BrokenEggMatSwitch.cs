using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class BrokenEggMatSwitch : MonoBehaviour { // This script manages the material switching from the broken egg model. This means that that if we use the Character 03, when die, the broken egg will use the Character 03 colors.

	[Header("Broken Egg Material Switching")]


	public Material []brokenEggMaterials; // The materials list. (Managed under the editor)


	void Update () {

		if (CharacterSelection.characterSelected == 1) { // If we have selected the Character 01 we will show the Character 01 Broken egg Material.

				this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [0];
			}

		if (CharacterSelection.characterSelected == 2) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [1];

		}
		if (CharacterSelection.characterSelected == 3) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [2];

		}
		if (CharacterSelection.characterSelected == 4) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [3];

		}
		if (CharacterSelection.characterSelected == 5) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [4];

		}
		if (CharacterSelection.characterSelected == 6) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [5];

		}
		if (CharacterSelection.characterSelected == 7) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [6];

		}
		if (CharacterSelection.characterSelected == 8) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [7];

		}
		if (CharacterSelection.characterSelected == 9) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [8];

		}
		if (CharacterSelection.characterSelected == 10) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [9];

		}
		if (CharacterSelection.characterSelected == 11) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [10];

		}
		if (CharacterSelection.characterSelected == 12) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [11];

		}
		if (CharacterSelection.characterSelected == 13) {

			this.gameObject.GetComponent<Renderer> ().material = brokenEggMaterials [12];

		}
	}
}
}