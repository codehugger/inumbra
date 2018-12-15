using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour {

	public TextAsset cutSceneTextFile;
	public bool disableAfterTrigger = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("Talk", cutSceneTextFile.text);
			other.gameObject.GetComponent<PlayerMovement>().enabled = false;
			if (disableAfterTrigger) {
				gameObject.SetActive(false);
			}
		}
	}
}
