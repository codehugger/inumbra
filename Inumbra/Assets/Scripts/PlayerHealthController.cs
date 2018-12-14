using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

	public float healingRate = 1;

	float hitPoints;
	float currentHitPoints;
	Color initialColor;

	// Use this for initialization
	void Start () {
		hitPoints = PlayerPrefs.GetFloat("PlayerHitPoints", 30.0f);
		currentHitPoints = hitPoints;
		initialColor = GetComponentInChildren<SpriteRenderer>().color;
	}

	// Update is called once per frame
	void Update () {
		if (currentHitPoints <= 0) {
			GetComponentInChildren<SpriteRenderer>().color = Color.grey;
			GetComponent<PlayerMovement>().enabled = false;
		}
		else if (currentHitPoints < hitPoints) {
			GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(initialColor, Color.red, currentHitPoints / hitPoints);
		} else {
			GetComponentInChildren<SpriteRenderer>().color = initialColor;
		}

		PlayerPrefs.SetFloat("PlayerHitPoints", currentHitPoints);

		Regenerate();
	}

	void Regenerate() {
		// Regenerate if not dead
		if (currentHitPoints < hitPoints && currentHitPoints > 0) {
			currentHitPoints += healingRate * Time.deltaTime;
			currentHitPoints = Mathf.Clamp(currentHitPoints, 0.0f, hitPoints);
		}
	}

	public void TakeDamage(float damage) {
		currentHitPoints -= damage;
	}
}
