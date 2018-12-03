using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float speed = 1;
	public float directionChangeInterval = 0.5f;
	public float playerDistance = 3;

	Vector3 target;
	float time = 0;
	float last_time = 0;
	GameObject player;

	// Use this for initialization
	void Start () {
		target = GetRandomDirection();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.time - last_time;
		if (time > directionChangeInterval || (transform.position == target))
		{
			target = GetRandomDirection();
			time = 0;
		}
		last_time = Time.time;
		Vector3 new_pos;
		if (Vector3.Distance(player.transform.position, transform.position) < playerDistance)
		{
			new_pos = player.transform.position;
		}
		else 
		{
			new_pos = transform.position + target;
		}
        transform.position = Vector3.Lerp(transform.position, new_pos ,Random.Range(1f, 3f) * speed * Time.deltaTime);
		// var dir = new_pos + target ;//- transform.position;
        // var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	Vector3 GetRandomDirection()
	{
		return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("DEAD!!");
		}
		Destroy(gameObject);
	}
}
