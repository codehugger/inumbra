using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour {

	public GameObject needle;
	
	GameObject player;

	int coalCount;
	int oldCount = 1000;
	int currentCoal;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		coalCount = GameObject.FindGameObjectsWithTag("coal").Length;
	}

	// Update is called once per frame
	void Update () {
		GameObject[] coals = GameObject.FindGameObjectsWithTag("coal");
		coalCount = coals.Length;
		if (player == null || coalCount == 0) { 
			Image[] list = gameObject.GetComponentsInChildren<Image>();
			foreach (Image l in list)
			{
				l.enabled = false;
			}
			return;
		}

		if (coalCount != oldCount) {
			currentCoal = Random.Range(0, coalCount);
			oldCount = coalCount;
		}

		Vector3 rot = player.transform.position - coals[currentCoal].transform.position;
		float look_angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        Quaternion new_rot = Quaternion.AngleAxis(look_angle, Vector3.forward);
		needle.transform.rotation = new_rot;
	}
}
