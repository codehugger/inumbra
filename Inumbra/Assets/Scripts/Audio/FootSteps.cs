using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour {
	public AudioClip gravelStepSound;
	public AudioClip grassStepSound;
	public AudioClip woodenFloorStepSound;
	public AudioClip stoneStepSound;
	public AudioClip dirtStepSound;

	GameStateController gameStateController;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		var gameState = GameObject.FindGameObjectWithTag("GameState");
		gameStateController = gameState.GetComponent<GameStateController>();
		audioSource = GetComponent<AudioSource>();
	}

	public void FootStep() {
		if (gameStateController) {
			audioSource.pitch = Random.Range(0.8f, 1.1f);
			audioSource.volume = Random.Range(0.8f, 1.0f);
			AudioClip stepSound;

			Debug.Log(gameStateController.groundMaterial);

			switch(gameStateController.groundMaterial) {
				case "grass":
				stepSound = grassStepSound;
				break;
				case "wood":
				stepSound = woodenFloorStepSound;
				break;
				case "stone":
				stepSound = stoneStepSound;
				break;
				case "dirt":
				stepSound = dirtStepSound;
				break;
				default:
				stepSound = gravelStepSound;
				break;
			}

			audioSource.PlayOneShot(stepSound);
		}
	}
}
