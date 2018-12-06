using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	enum EnemyState
	{
		move = 0,
		attack = 1,
		retreat = 2
	}

	public float rotationSpeed = 10;
	public float movementSpeed = 1;
	// public float timeToKill = 2;
	public float playerDistance = 3;
	public float fleeSpeed = 5;

	public float dieCondition = 0.2f;
	public float fadeSpeed = 0.1f;

	Quaternion target;
	float time = 0;
	float lastTime = 0;
	GameObject player;
	float directionChangeInterval;
	EnemyState internalState;
	bool inLantern;
	bool stateChanged;
	bool dying;

	float alpha = 1;

	// Audio
	private AudioSource audioSource;
	public AudioClip moveSound;
	public AudioClip attackSound;
	public AudioClip retreatSound;
	public AudioClip damageSound;
	public AudioClip deathSound;

	// Use this for initialization
	void Start () {
		target = GetRandomRotation();
		player = GameObject.FindGameObjectWithTag("Player");
		directionChangeInterval = Random.value * 3;
		internalState = 0;
		audioSource = GetComponent<AudioSource>();
		dying = false;

		// Play all sounds as coroutines
		StartCoroutine(PlayMovementSound());
		StartCoroutine(PlayDamageSound());
	}

	// Update is called once per frame
	void Update () {

		UpdateState();

		if (internalState == EnemyState.retreat)
		{
			if (Vector3.Distance(player.transform.position, transform.position) < playerDistance/2)
			{
				if (Random.Range(0f, 1f) > 0.5f)
				{
					target = Quaternion.LookRotation(transform.position + transform.right - transform.up, -Vector3.forward);
				}
				else
				{
					target = Quaternion.LookRotation(transform.position - transform.right - transform.up, -Vector3.forward);
				}
			}
			else
			{
				target = Quaternion.LookRotation(transform.position - transform.up, -Vector3.forward);
			}
			time = 0;
			movementSpeed = fleeSpeed;
			//StartCoroutine(PlayRetreatSound());
		}
		else if (internalState == EnemyState.attack)
		{
			target = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
			time = 0;
		}
		else
		{
			if (time > directionChangeInterval)
			{
				// if (Random.Range(0f, 1f) > 0.7f)
				// {
				// 	movementSpeed = 0;
				// }
				// else
				// {
				// 	movementSpeed += 0.2f;
				// }
				target = GetRandomRotation();
				time = 0;
				directionChangeInterval = Random.value * 3;
			}
		}

		target.x = 0;
		target.y = 0;

		transform.rotation = Quaternion.Lerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
		transform.position += transform.up * movementSpeed * Time.deltaTime;

		lastTime = Time.time;
	}

	void UpdateState()
	{
		EnemyState oldState = internalState;
		stateChanged = false;

		if (inLantern)
		{
			// RETREAT
			internalState = EnemyState.retreat;
		}
		else if(Vector3.Distance(player.transform.position, transform.position) < playerDistance)
		{
			// ATTACK
			internalState = EnemyState.attack;
		}
		else
		{
			// MOVE
			internalState = EnemyState.move;
		}

		if (oldState != internalState) {
			stateChanged = true;
		}
	}

	Quaternion GetRandomRotation()
	{
		Quaternion randomRotation = Random.rotation;
		return randomRotation;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			player.GetComponent<PlayerHealthController>().TakeDamage();
			Destroy(gameObject);
		}
		else if (other.gameObject.tag == "Lantern")
		{
			inLantern = true;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		// Debug.Log(Physics.Raycast(transform.position, player.transform.position - transform.position, 100));
		if (other.gameObject.tag == "Lantern")// && !Physics.Raycast(transform.position, player.transform.position - transform.position, 100))
		{
			alpha -= Time.deltaTime / dieCondition * fadeSpeed;
			if (alpha < dieCondition)
			{
				dying = true;
				Debug.Log("DEAD");
			}
			else
			{
				Color color = GetComponentInChildren<SpriteRenderer>().color;
				color.a = alpha;//Mathf.Min(color.a - Time.deltaTime * exposureTime/timeToKill, baseFade);
				GetComponentInChildren<SpriteRenderer>().color = color;
				transform.localScale += new Vector3(0.005f, 0.005f, 0);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Lantern")
		{
			inLantern = false;
		}
	}

	IEnumerator PlayMovementSound() {
		while (true) {
			switch (internalState)
			{
				case EnemyState.move:
				if (moveSound != null) {
					int rand = Random.Range(0, 1000);
					if (rand == 1) {
						// audioSource.PlayOneShot(attackSound);
						// yield return new WaitForSeconds(attackSound.length);
					} else if (rand == 2) {
						audioSource.PlayOneShot(moveSound);
						yield return new WaitForSeconds(moveSound.length);
					}
				}
				break;
				case EnemyState.attack:
				if (attackSound != null && stateChanged) {
					audioSource.PlayOneShot(attackSound);
					yield return new WaitForSeconds(attackSound.length);
				}
				break;
				case EnemyState.retreat:
				if (retreatSound != null && stateChanged) {
					audioSource.PlayOneShot(retreatSound);
					yield return new WaitForSeconds(retreatSound.length);
				}
				break;
			}
			yield return null;
		}
	}

	IEnumerator PlayDamageSound() {
		while (true) {
			if (dying) {
				audioSource.PlayOneShot(deathSound);
				yield return new WaitForSeconds(deathSound.length);
				Destroy(gameObject);
			}
			else if (damageSound && inLantern) {
				audioSource.PlayOneShot(damageSound);
				yield return new WaitForSeconds(damageSound.length);
			}
			yield return null;
		}
	}
}
