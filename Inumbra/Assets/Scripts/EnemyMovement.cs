using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float rotationSpeed = 10;
	public float movementSpeed = 1;
	public float directionChangeInterval = 0.5f;
	public float playerDistance = 3;

	Quaternion target;
	float time = 0;
	float last_time = 0;
	GameObject player;

	// Use this for initialization
	void Start () {
		target = GetRandomRotation();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.time - last_time;

		if (Vector3.Distance(player.transform.position, transform.position) < playerDistance)
		{
			target = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
			target.x = 0;
			target.y = 0;
			time = 0;
		}
		else if (time > directionChangeInterval)
		{
			target = GetRandomRotation();
			time = 0;
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
		transform.position += transform.up * movementSpeed * Time.deltaTime;

		last_time = Time.time;
	}

	Quaternion GetRandomRotation()
	{	
		Quaternion random_rotation = Random.rotation;
		random_rotation.x = 0;
		random_rotation.y = 0;
		return random_rotation;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}
		
	}
}
