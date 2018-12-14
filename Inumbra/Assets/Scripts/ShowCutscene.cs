using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCutscene : MonoBehaviour {

	public TextAsset cutSceneTextFile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("Talk", cutSceneTextFile.text);
			gameObject.SetActive(false);
		}
	}
}
