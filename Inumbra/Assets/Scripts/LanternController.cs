using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class LanternController : MonoBehaviour {

	public GameObject lanternSpotlight;
	public GameObject lanternAura;
	public GameObject lanternAreaOfEffect;
	public float focusSpeed = 2.0f;
	public float damagePerSecond = 5.0f;
	public float minSpotlightScale = 0.5f;
	public float minSpotlightLevel = 0.2f;
	public float maxSpotlightLevel = 1.0f;
	public float minAuraScale = 0.9f;
	public float minAuraLevel = 0.2f;
	public float maxAuraLevel = 0.5f;

	[HideInInspector]
	public float currentDamage = 0.0f;

	[HideInInspector]
	public float rayIntensity = 0.0f;

	LightSprite spotlightSprite;
	LightSprite auraSprite;
	float currentSpotlightLevel;
	float currentAuraLevel;
	Vector3 auraScale;
	Vector3 spotlightScale;
	Vector3 aoeScale;

	void Start() {
		spotlightSprite = lanternSpotlight.GetComponent<LightSprite>();
		auraSprite = lanternAura.GetComponent<LightSprite>();

		// Store the initial scale of objects
		auraScale = lanternAura.transform.localScale;
		spotlightScale = lanternSpotlight.transform.localScale;
		aoeScale = lanternAreaOfEffect.transform.localScale;

		// Set initial light level
		currentSpotlightLevel = minSpotlightLevel;
		currentAuraLevel = maxAuraLevel;
	}

	void Update() {
		bool sprintEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("XBox L2") > 0.2 || Input.GetButton("Fire2");
		float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
		bool isMoving = (in_x != 0 || in_y != 0);
		bool isRunning = sprintEnabled && isMoving;

		// Increase light level
		if ((Input.GetButton("Fire1") || Input.GetAxis("XBox R2") > 0.2)) {
			currentSpotlightLevel += focusSpeed * Time.deltaTime;
			currentAuraLevel -= focusSpeed * Time.deltaTime;
		} else {
			currentSpotlightLevel = minSpotlightLevel;
			currentAuraLevel = maxAuraLevel;
		}

		if (isRunning) {
			currentSpotlightLevel = minSpotlightLevel;
			currentAuraLevel = minAuraLevel;
			lanternSpotlight.SetActive(false);
		} else {
			lanternSpotlight.SetActive(true);
		}

		// Make sure we never go out of bounds with the light level
		currentSpotlightLevel = Mathf.Clamp(currentSpotlightLevel, minSpotlightLevel, maxSpotlightLevel);
		currentAuraLevel = Mathf.Clamp(currentAuraLevel, minAuraLevel, maxAuraLevel);

		// Set intensity of lantern "raygun"
		rayIntensity = Mathf.Clamp(1 - (currentSpotlightLevel - minSpotlightLevel) / (maxSpotlightLevel - minSpotlightLevel), 0.01f, 1.0f);
		currentDamage = rayIntensity * damagePerSecond;

		// Set the scale of the lights
		lanternAura.transform.localScale = auraScale * Mathf.Clamp(rayIntensity, minAuraScale, 1.0f);
		lanternSpotlight.transform.localScale = new Vector3(spotlightScale.x * Mathf.Clamp(rayIntensity, minSpotlightScale, 1.0f), spotlightScale.y, 0);
		lanternAreaOfEffect.transform.localScale = new Vector3(aoeScale.x, aoeScale.y * Mathf.Clamp(rayIntensity, minSpotlightScale, 1.0f), 0);

		// Assign alpha color
		spotlightSprite.Color.a = currentSpotlightLevel;
		auraSprite.Color.a = currentAuraLevel;
	}
}
