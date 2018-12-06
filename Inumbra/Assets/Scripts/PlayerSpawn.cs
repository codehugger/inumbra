using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject playerObject;
	public AudioClip lanternSound;
	AudioSource audioSource;
	GameObject[] shades;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("Fuel", 0);
		audioSource = GetComponent<AudioSource>();
		DeactivateEnemies();
		StartCoroutine(TurnLanternOn());
	}

	// Update is called once per frame
	void Update () {

	}

	void DeactivateEnemies() {
		PlayerPrefs.SetInt("EnemiesMove", 0);
	}

	void ActivateEnemies() {
		PlayerPrefs.SetInt("EnemiesMove", 1);
	}

	IEnumerator TurnLanternOn() {
		playerObject.SetActive(false);
		PlayerPrefs.SetString("Talk", "It is so dark! I will probably need to find some sort of fuel to get the train moving again.");
		yield return new WaitForSeconds(4);
		audioSource.PlayOneShot(lanternSound);
		yield return new WaitForSeconds(lanternSound.length);
		playerObject.SetActive(true);
		ActivateEnemies();
	}
}
