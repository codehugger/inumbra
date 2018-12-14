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
		player = GameObject.FindGameObjectWithTag("Player");
		audioSource = player.GetComponent<AudioSource>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		fuelLevel = PlayerPrefs.GetFloat("FuelLevel", 0.0f);
	}

	void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("Collision with COAL");
		if (other.transform.tag == "Player" && fuelLevel < 1.0f) {
			CoalPickup();
		}
	}

	void CoalPickup() {
		PlayerPrefs.SetFloat("FuelLevel", fuelLevel += fuelAmount);
		spriteRenderer.enabled = false;
		audioSource.PlayOneShot(coalPickupSound);
		Destroy(gameObject);
	}
}
