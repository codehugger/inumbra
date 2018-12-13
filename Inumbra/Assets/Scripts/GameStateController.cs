using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {


	// Global "variables"
	[Range(0.0f, 1.0f)]
	public float startingFuelLevel = 0.0f;
	public bool turnOnLantern = true;
	public bool enemiesCanMove = true;
	public bool wipeOnStart = false;

	// Use this for initialization
	void Start () {
		if (wipeOnStart) { PlayerPrefs.DeleteAll(); }
		PlayerPrefs.SetFloat("FuelLevel", startingFuelLevel);
		PlayerPrefs.SetInt("EnemiesCanMove", enemiesCanMove ? 1 : 0);
		PlayerPrefs.SetInt("TurnOnLanter", turnOnLantern ? 1 : 0);
	}

	// Update is called once per frame
	void Update () {

	}
}
