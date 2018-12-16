using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMaterial : MonoBehaviour {

	public string materialName;

	private GameStateController gameStateController;
	private string previousMaterialName;

	// Use this for initialization
	void Start () {
		var gameState = GameObject.FindGameObjectWithTag("GameState");
		if (gameState) {
			gameStateController = gameState.GetComponent<GameStateController>();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" && gameStateController) {
			previousMaterialName = gameStateController.groundMaterial;
			gameStateController.groundMaterial = materialName;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player" && gameStateController) {
			gameStateController.groundMaterial = previousMaterialName;
		}
	}
}
