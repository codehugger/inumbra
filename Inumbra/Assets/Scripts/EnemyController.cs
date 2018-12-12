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
		idle = 4
	}

	// public variables
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
	bool dead = false;

	ParticleSystem ps;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		currentState = EnemyState.move;
		currentHitPoints = hitPoints;
		ps = gameObject.GetComponent<ParticleSystem>();
		RotateRandom();
	}

	void Update() {
		if (takingDamage){
			if (Random.Range(-0.2f, 1f) < lantern.rayIntensity){
				ps.Emit(1);
			}
		}
		// We don't do anything when dead
		if (dead) { return; }

		UpdateState();

		if (takingDamage) { HandleDamage(); }
		else { Regenerate(); }

		UpdateSpeed();
		UpdateRotation();

		// Debug.Log(string.Format("Speed: {0}", currentSpeed));
		// Debug.Log(string.Format("Hit Points: {0}", currentHitPoints));
		// Debug.Log(string.Format("State: {0}", currentState));

		Vector3 old_position = transform.position;
		transform.position += (transform.up * currentSpeed);
		float length = Vector3.Magnitude(transform.position - old_position);
        anim.SetFloat("Speed", length);
	}

	void UpdateState() {
		// EnemyState oldState = currentState;
		stateChanged = false;
		var oldState = currentState;
		var playerDistance = Vector2.Distance(player.transform.position, transform.position);

		// player out of reach => move
		if (playerDistance > reactionDistance) {
			currentState = EnemyState.move;
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
		if (oldState != currentState) { stateChanged = true; }
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
		if (lantern != null) {
			currentHitPoints -= Time.deltaTime * lantern.currentDamage;
		}

		if (currentHitPoints <= 0) {
			dead = true;
			StartCoroutine(HandleDeath());
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
			StartCoroutine(HandleDeath());
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Lantern") {
			lantern = null;
			takingDamage = false;
		}
	}

	IEnumerator HandleDeath() {
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
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
   		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, reactionDistance);
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, reactionDistance * attackDistanceFactor);
	}
}
