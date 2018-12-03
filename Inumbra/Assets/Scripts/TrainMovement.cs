using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour {

    public Vector2 target;
    public float speed;
    Vector2 position;

	// Use this for initialization
	void Start () {
        position = gameObject.transform.position;
    }

	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);	
	}
}
