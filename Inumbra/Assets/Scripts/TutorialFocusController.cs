using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialFocusController : MonoBehaviour {

	public TextMeshProUGUI tutorialText;

	bool activated = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			if (Input.GetButton("Fire1") || Input.GetAxis("XBox R2") > 0.2){
				tutorialText.SetText("");
				gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			tutorialText.SetText("press R2 to focus your lantern to fight enemies");
			activated = true;
		}
	}
}
