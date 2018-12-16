using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalController : MonoBehaviour {

	public AudioClip coalPickupSound;
	public float fuelAmount = 0.25f;

	GameObject player;
	AudioSource audioSource;
	SpriteRenderer spriteRenderer;
	float fuelLevel;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.FindGameObjectWithTag("Player");
		if (player) {
			audioSource = player.GetComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
			if (audioSource) {
				audioSource = player.GetComponent<AudioSource>();
			}
		}
		fuelLevel = PlayerPrefs.GetFloat("FuelLevel", 0.0f);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Player" && fuelLevel < 1.0f) {
			CoalPickup();
		} else {
			PlayerPrefs.SetString("Talk", "I cannot carry more coal.");
		}
	}

	void CoalPickup() {
		PlayerPrefs.SetFloat("FuelLevel", fuelLevel += fuelAmount);
		spriteRenderer.enabled = false;
		audioSource.PlayOneShot(coalPickupSound);
		Destroy(gameObject);
	}
}
