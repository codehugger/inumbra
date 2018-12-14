using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoalPickup : MonoBehaviour {

	public GameObject enemies;
	public GameObject tutorialCutscenes2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			enemies.SetActive(true);
			tutorialCutscenes2.SetActive(true);
		}
	}
}
