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
		PlayerPrefs.SetInt("Fuel",0);
		shades = GameObject.FindGameObjectsWithTag("Shade");
		audioSource = GetComponent<AudioSource>();
		DeactivateEnemies();
		StartCoroutine(TurnLanternOn());
	}

	// Update is called once per frame
	void Update () {

	}

	void DeactivateEnemies() {
		for (int i = 0; i < shades.Length; i++)
		{
			shades[i].SetActive(false);
		}
	}

	void ActivateEnemies() {
		for (int i = 0; i < shades.Length; i++)
		{
			shades[i].SetActive(true);
		}
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
