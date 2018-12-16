using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour {

	AudioSource audioSource;

	public AudioClip gravelStepSound;
	public AudioClip grassStepSound;
	public AudioClip woodenFloorStepSound;
	public AudioClip stoneStepSound;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void FootStep() {
		audioSource.pitch = Random.Range(0.8f, 1.1f);
		audioSource.volume = Random.Range(0.8f, 1.0f);
		audioSource.PlayOneShot(gravelStepSound);
	}
}
