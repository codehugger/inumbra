using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

	enum PlayerHealthState
	{
		alive = 1,
		damaged = 2,
		regenerating = 3,
		dead = 4,
	}

	public float healingRate = 1;

	public AudioClip damageSound;
	public AudioClip heartbeatSound;
	public AudioClip deathSound;

	float hitPoints;
	float currentHitPoints;
	Color initialColor;
	PlayerHealthState currentState;
	AudioSource audioSource;

	bool playingHeartbeatSound = false;
	bool stateChanged = false;

	// Use this for initialization
	void Start () {
		currentState = PlayerHealthState.alive;
		hitPoints = PlayerPrefs.GetFloat("InitialHitPoints", 30.0f);
		currentHitPoints = hitPoints;
		initialColor = GetComponentInChildren<SpriteRenderer>().color;

		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		UpdateState();

		if (currentState == PlayerHealthState.dead) {
			GetComponentInChildren<SpriteRenderer>().color = Color.grey;
			GetComponent<PlayerMovement>().enabled = false;
		}
		else if (currentState == PlayerHealthState.regenerating) {
			GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(initialColor, Color.red, 1 - currentHitPoints / hitPoints);
			Regenerate();
		} else {
			GetComponentInChildren<SpriteRenderer>().color = initialColor;
		}

		if (currentState == PlayerHealthState.regenerating && stateChanged) {
			StartCoroutine(PlayHeartbeatSound());
		}

		PlayerPrefs.SetFloat("CurrentHitPoints", currentHitPoints);
	}

	void UpdateState() {
		stateChanged = false;
		var previousState = currentState;

		if (currentHitPoints <= 0) {
			currentState = PlayerHealthState.dead;
		} else if (currentHitPoints < hitPoints) {
			currentState = PlayerHealthState.regenerating;
		} else {
			currentState = PlayerHealthState.alive;
		}

		if (previousState != currentState) { stateChanged = true; }
	}

	void Regenerate() {
		// Regenerate if not dead
		if (currentHitPoints < hitPoints && currentHitPoints > 0) {
			currentHitPoints += healingRate * Time.deltaTime;
			currentHitPoints = Mathf.Clamp(currentHitPoints, 0.0f, hitPoints);
		}
	}

	IEnumerator PlayHeartbeatSound() {
		if (!playingHeartbeatSound) {
			playingHeartbeatSound = true;
			audioSource.PlayOneShot(heartbeatSound);
			yield return new WaitForSeconds(heartbeatSound.length);
			playingHeartbeatSound = false;
		}
	}

	public void TakeDamage(float damage) {
		currentHitPoints -= damage;
		currentState = PlayerHealthState.damaged;
		audioSource.Stop();
		audioSource.PlayOneShot(damageSound);
	}
}
