using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float rotationSpeed = 10;
	public float movementSpeed = 1;
	public float timeToKill = 2;
	public float playerDistance = 3;
	public float fleeSpeed = 5;

	Quaternion target;
	float time = 0;
	float lastTime = 0;
	GameObject player;
	float directionChangeInterval;
	float exposureTime;
	int internalState;
	bool inLantern;

	// Use this for initialization
	void Start () {
		target = GetRandomRotation();
		player = GameObject.FindGameObjectWithTag("Player");
		directionChangeInterval = Random.value * 3;
		internalState = 0;
	}
	
	// Update is called once per frame
	void Update () {

		UpdateState();

		if (internalState == 2)
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
		}
		else if (internalState == 1)
		{
			target = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
			time = 0;
		}
		else 
		{
			if (time > directionChangeInterval)
			{
				if (Random.Range(0f, 1f) > 0.7f)
				{
					movementSpeed = 0;
				}
				else
				{
					movementSpeed += 0.2f;
				}
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
		if (inLantern)
		{
			// RETREAT
			internalState = 2;
		}
		else if(Vector3.Distance(player.transform.position, transform.position) < playerDistance)
		{
			// ATTACK
			internalState = 1;
		}
		else 
		{
			// MOVE
			internalState = 0;
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
			Destroy(gameObject);
		}	
		else if (other.gameObject.tag == "Lantern")
		{
			exposureTime = 0;
			inLantern = true;
		}	
	}

	void OnTriggerStay2D(Collider2D other)
	{
		// Debug.Log(Physics.Raycast(transform.position, player.transform.position - transform.position, 100));
		if (other.gameObject.tag == "Lantern")// && !Physics.Raycast(transform.position, player.transform.position - transform.position, 100))
		{
			exposureTime += Time.fixedDeltaTime;
			if (exposureTime >= timeToKill)
			{
				Destroy(gameObject);
			}
			else
			{
				transform.localScale += new Vector3(0.01f, 0.01f, 0);
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
}
