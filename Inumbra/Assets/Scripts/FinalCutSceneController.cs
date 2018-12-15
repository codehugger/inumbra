using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutSceneController : MonoBehaviour {


	public TextAsset cutSceneTextFile;
	public bool disableAfterTrigger = true;

	public GameObject finalLight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("Talk", cutSceneTextFile.text);
			finalLight.SetActive(true);
			if (disableAfterTrigger) {
				gameObject.SetActive(false);
			}
		}
	}
}
