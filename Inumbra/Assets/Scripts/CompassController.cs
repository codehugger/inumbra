using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour {

	public GameObject needle;

	public float showAfterDistance;
	GameObject player;
	GameObject train;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		train = GameObject.FindGameObjectWithTag("Train");
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position, train.transform.position) > showAfterDistance){
			SpriteRenderer[] list = gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer l in list)
			{
				l.enabled = true;
			}
		} else {
			SpriteRenderer[] list = gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer l in list)
			{
				l.enabled = false;
			}
		}
		Vector3 rot = player.transform.position - train.transform.position;
		float look_angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        Quaternion new_rot = Quaternion.AngleAxis(look_angle, Vector3.forward);
		needle.transform.rotation = new_rot;
	}
}
