using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickupController : MonoBehaviour {

	public AudioClip keyPickupSound;
	GameObject player;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		audioSource = player.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.tag == "Player") {
			KeyPickup();
		}
	}

	void KeyPickup() {
		PlayerPrefs.SetInt("HasKey", 1);
		audioSource.PlayOneShot(keyPickupSound);
		Destroy(gameObject);
	}
}
