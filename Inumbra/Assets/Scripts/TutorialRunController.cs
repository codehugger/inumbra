using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialRunController : MonoBehaviour {

	public TextMeshProUGUI tutorialText;

	bool activated = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2") || Input.GetAxis("XBox L2") > 0.2){
				tutorialText.SetText("");
				gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			tutorialText.SetText("press L2 to run");
			activated = true;
		}
	}
}
