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
		audioSource.PlayOneShot(lanternSound);
		yield return new WaitForSeconds(lanternSound.length);
		playerObject.SetActive(true);
		ActivateEnemies();
	}
}
