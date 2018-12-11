using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	enum EnemyState
	{
		move = 0,
		attack = 1,
		retreat = 2,
		idle = 3
	}

	// public variables
	public float reactionDistance = 10.0f;
	[Range(0.01f, 1.0f)]
	public float attackDistanceFactor = 0.5f;
	[Range(1.0f, 2.0f)]
	public float attackSpeedFactor = 1.5f;
	[Range(1.0f, 2.0f)]
	public float retreatSpeedFactor = 1.5f;
	public float movementSpeed = 1.0f;
	public bool moveWhenIdle = true;
	public float hitPoints = 10.0f;
	public float healingRate = 1.0f;
	public float damage = 10.0f;

	// private variables
	private GameObject player;
	bool takingDamage;
	EnemyState currentState;
	Transform target;
	float damageFactor;
	float currentSpeed;
	float currentHitPoints;
	bool stateChanged;
	LanternController lantern;
	float timeToChange = 0;
	Vector2 direction;
	bool dead = false;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		// We don't do anything when dead
		if (dead) { return; }

		UpdateState();

		switch (currentState)
		{
			case EnemyState.move:
			PerformMoveAction();
			break;
			case EnemyState.attack:
			PerformAttackAction();
			break;
			case EnemyState.retreat:
			PerformRetreatAction();
			break;
			case EnemyState.idle:
			PerformIdleAction();
			break;
		}

		if (takingDamage) { HandleDamage(); }
		else { Regenerate(); }

		// if in a fully focused beam halt completely
		currentSpeed = movementSpeed * (1 - damageFactor);

		Debug.Log(string.Format("Speed: {0}", currentSpeed));
		Debug.Log(string.Format("Hit Points: {0}", currentHitPoints));
		Debug.Log(string.Format("State: {0}", currentState));

		transform.position = Vector2.MoveTowards(transform.position, direction, currentSpeed);
	}

	void UpdateState() {
		EnemyState oldState = currentState;
		stateChanged = false;
		var playerDistance = Vector3.Distance(player.transform.position, transform.position);

		// player out of reach => move
		if (playerDistance > reactionDistance) {
			if (moveWhenIdle) {
				currentState = EnemyState.move;
			} else {
				currentState = EnemyState.idle;
			}

			// reset the movement speed when just moving around
			currentSpeed = movementSpeed;

			// change direction after a period of time
			if (Time.time >= timeToChange) {
				var randomX = Random.Range(-1.0f, 1.0f);
				var randomY = Random.Range(-1.0f, 1.0f);
				direction = (new Vector3(randomX, randomY, 0) - transform.position).normalized;
			}
		}
		// taking damage and not within attack distance => retreat
		else if (takingDamage && playerDistance > (reactionDistance * attackDistanceFactor)) {
			direction = -(player.transform.position - transform.position).normalized;
			currentState = EnemyState.retreat;
			currentSpeed = movementSpeed * retreatSpeedFactor;
		}
		// inside attack range => attack
		else if (playerDistance < (reactionDistance * attackDistanceFactor)) {
			direction = (player.transform.position - transform.position).normalized;
			currentState = EnemyState.attack;
			currentSpeed = movementSpeed * attackSpeedFactor;
		}
	}

	void PerformMoveAction() {}

	void PerformAttackAction() {}

	void PerformRetreatAction() {}

	void PerformIdleAction() {
		transform.right = target.position - transform.position;
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
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Lantern") {
			lantern = null;
			takingDamage = false;
		}
	}

	IEnumerator HandleDeath() {
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
}
