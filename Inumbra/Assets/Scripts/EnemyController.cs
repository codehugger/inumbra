using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	enum EnemyState
	{
		move = 0,
		follow = 1,
		attack = 2,
		retreat = 3,
		idle = 4,
		dead = 5
	}

	// public variables
	public GameObject sprite;
	public float reactionDistance = 10.0f;
	[Range(0.01f, 1.0f)]
	public float attackDistanceFactor = 0.5f;
	[Range(1.0f, 10.0f)]
	public float attackSpeedMultiplier = 2.0f;
	[Range(1.0f, 10.0f)]
	public float retreatSpeedMultiplier = 1.5f;
	public float movementSpeed = 1.0f;
	public bool moveWhenIdle = true;
	public float hitPoints = 10.0f;
	public float healingRate = 1.0f;
	public float damage = 10.0f;
	public float timeBetweenDirectionChanges = 3f;

	public Animator anim;

	public AudioClip idleSound;
	public AudioClip[] followSounds;
	public AudioClip[] attackSounds;
	public AudioClip[] deathSounds;
	public AudioClip[] retreatSounds;

	public bool movementEnabled = true;

	// private variables
	private GameObject player;
	bool takingDamage;
	EnemyState currentState;
	float damageFactor;
	float currentSpeed;
	float currentHitPoints;
	bool stateChanged;
	LanternController lantern;
	float timeToChange = 0;
	Vector2 direction;

	ParticleSystem ps;
	AudioSource audioSource;
	bool playingActivitySound = false;
	bool playingDamageSound = false;
	AudioClip attackSound;
	AudioClip followSound;
	AudioClip retreatSound;
	AudioClip deathSound;
	bool handlingDeath = false;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		currentState = EnemyState.move;
		currentHitPoints = hitPoints;
		ps = gameObject.GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();
		RotateRandom();

		// Pick a random distortion at startup
		audioSource.pitch = Random.Range(0.5f, 1f);

		// Pick a damage sound for this shadow
		attackSound = attackSounds[Random.Range(0, attackSounds.Length)];
		followSound = followSounds[Random.Range(0, followSounds.Length)];
		retreatSound = retreatSounds[Random.Range(0, retreatSounds.Length)];
		deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
	}

	void Update() {
		UpdateState();

		if (currentState == EnemyState.dead && !handlingDeath) {
			StartCoroutine(HandleDeath(true));
		} else {
			if (takingDamage) {
				if (Random.Range(-0.2f, 1f) < lantern.rayIntensity){
					ps.Emit(1);
				}
				HandleDamage();
			} else {
				Regenerate();
			}

			UpdateSpeed();
			UpdateRotation();

			if (movementEnabled) {
				Vector3 old_position = transform.position;
				transform.position += (transform.up * currentSpeed);
				float length = Vector3.Magnitude(transform.position - old_position);
				anim.SetFloat("Speed", length);
			}

			if (stateChanged) {
				StartCoroutine(PlayActivitySound());
			}
		}

		// Debug.Log(string.Format("Speed: {0}", currentSpeed));
		// Debug.Log(string.Format("Hit Points: {0}", currentHitPoints));
		// Debug.Log(string.Format("State: {0}", currentState));
	}

	void UpdateState() {
		// EnemyState oldState = currentState;
		stateChanged = false;
		var oldState = currentState;
		var playerDistance = Vector2.Distance(player.transform.position, transform.position);

		// player out of reach => move
		if (currentHitPoints <= 0) {
			currentState = EnemyState.dead;
		} else if (playerDistance > reactionDistance) {
			if (moveWhenIdle) {
				currentState = EnemyState.move;
			} else {
				currentState = EnemyState.idle;
			}
		}
		// inside attack range => attack
		else if (playerDistance < (reactionDistance * attackDistanceFactor)) {
			currentState = EnemyState.attack;
		}
		// taking damage and not within attack distance => retreat
		else if (takingDamage && playerDistance > (reactionDistance * attackDistanceFactor)) {
			currentState = EnemyState.retreat;
		}
		// player within reaction range => follow
		else if (playerDistance < reactionDistance) {
			currentState = EnemyState.follow;
		}

		// keep track of state updates
		if (oldState != currentState) {
			stateChanged = true;
			StopCoroutine("PlayActivitySound");
			audioSource.Stop();
			StartCoroutine(PlayActivitySound());
		}
	}

	void UpdateRotation() {
		switch (currentState) {
			case EnemyState.follow:
			case EnemyState.attack:
				RotateTowardsPlayer();
				break;
			case EnemyState.retreat:
				RotateAwayFromPlayer();
				break;
			default:
				if (Time.time >= timeToChange) {
					RotateRandom();
					timeToChange += timeBetweenDirectionChanges;
				}
				break;
		}
	}

	void UpdateSpeed() {
		// if in a fully focused beam halt completely
		currentSpeed = movementSpeed * Time.deltaTime;

		switch (currentState)
		{
			case EnemyState.idle:
				currentSpeed = 0.0f;
				break;
			case EnemyState.attack:
				currentSpeed *= attackSpeedMultiplier;
				break;
			case EnemyState.retreat:
				currentSpeed *= retreatSpeedMultiplier;
				break;
		}

		// when taking damage slow down
		if (takingDamage) {
			currentSpeed *= (1 - damageFactor);
		}
	}

	void HandleDamage() {
		// use the lantern damage and time factor to produce damage per second
		if (lantern != null && !handlingDeath) {
			currentHitPoints -= Time.deltaTime * lantern.currentDamage;
		}
	}

	void Regenerate() {
		if (currentHitPoints < hitPoints) {
			currentHitPoints += healingRate * Time.deltaTime;
			currentHitPoints = Mathf.Clamp(currentHitPoints, 0.0f, hitPoints);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Lantern") {
			lantern = collider.gameObject.GetComponentInParent<LanternController>();
			takingDamage = true;
		}
		if (collider.gameObject.tag == "Player") {
			collider.gameObject.GetComponent<PlayerHealthController>().TakeDamage(damage);
			StartCoroutine(HandleDeath(false));
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Lantern") {
			lantern = null;
			takingDamage = false;
		}
	}

	IEnumerator HandleDeath(bool withSound) {
		if (!handlingDeath) {
			handlingDeath = true;
			sprite.SetActive(false);
			StopCoroutine("PlayActivitySound");
			ps.Stop(true);
			audioSource.Stop();
			if (withSound) { audioSource.PlayOneShot(deathSound); }
			yield return new WaitForSeconds(deathSound.length);
			Destroy(gameObject);
		}
	}

	IEnumerator PlayActivitySound() {
		switch (currentState) {
			case EnemyState.follow:
			audioSource.PlayOneShot(followSound);
			yield return new WaitForSeconds(followSound.length);
			break;
			case EnemyState.attack:
			audioSource.PlayOneShot(attackSound);
			yield return new WaitForSeconds(attackSound.length);
			break;
			case EnemyState.idle:
			case EnemyState.move:
			audioSource.PlayOneShot(idleSound);
			yield return new WaitForSeconds(idleSound.length);
			break;
		}
	}

	void RotateRandom() {
		transform.Rotate(Vector3.forward, Random.Range(0, 360));
	}

	void RotateTowardsPlayer() {
		var target = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
		// only rotate on the z-axis so we don't skew the sprite
		target.x = 0; target.y = 0;
		transform.rotation = Quaternion.Lerp(transform.rotation, target, currentSpeed);
	}

	void RotateAwayFromPlayer() {
		var target = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
		// only rotate on the z-axis so we don't skew the sprite
		target.x = 0; target.y = 0; target.z *= -1;
		transform.rotation = Quaternion.Lerp(transform.rotation, target, currentSpeed);
	}

	private void OnDrawGizmos(){
		#if UNITY_EDITOR
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, reactionDistance);
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, reactionDistance * attackDistanceFactor);
		#endif
	}
}
