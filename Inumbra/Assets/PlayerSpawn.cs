using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject playerObject;
	public AudioClip lanternSound;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator TurnLanternOn() {
		playerObject.SetActive(false);
		audioSource.PlayOneShot(lanternSound);
		yield return new WaitForSeconds(lanternSound.length);
		playerObject.SetActive(true);
	}
}
