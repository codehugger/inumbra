using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelGaugeController : MonoBehaviour {

	private GameObject needle;
	float fuelLevel = 0.0f;
	float currentRotation;

	public float minAngle = 0.0f;
	public float maxAngle = -120.0f;

	void Start() {}

	// Use this for initialization
	void Awake () {
		fuelLevel = PlayerPrefs.GetFloat("FuelLevel");
		currentRotation = minAngle;
		needle = GameObject.FindGameObjectWithTag("FuelNeedle");
	}

	// Update is called once per frame
	void Update () {
		// ($normalized * ($max - $min) + $min);
		fuelLevel = PlayerPrefs.GetFloat("FuelLevel");
		currentRotation = fuelLevel * (maxAngle - minAngle) + minAngle;
		needle.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
	}
}
