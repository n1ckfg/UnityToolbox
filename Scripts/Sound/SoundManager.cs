using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource[] dialogue;
	public string[] cueList;
	public bool multiTrigger = false;

	private int counter = 0;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyUp(KeyCode.Space)) {
			triggerSound(cueList[counter]);
			counter++;
			if (counter >= cueList.Length) counter = 0;
		}
	}

	void triggerSound(string cue) {
		if (!multiTrigger) {
			bool cancelFoley = false;
			for (int i=0; i<dialogue.Length; i++) {
				if (dialogue[i].isPlaying) {
					cancelFoley = true; 
					dialogue[i].GetComponent<AudioSource>().Stop();
				}
			}
			if (cancelFoley) triggerSound("cancel");
		}

		for (int i=0; i<cueList.Length; i++) {
			if (cue == cueList[i]) dialogue[i].GetComponent<AudioSource>().Play();
		}
	}
}
