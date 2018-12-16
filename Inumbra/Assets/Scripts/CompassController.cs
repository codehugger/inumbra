using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour {

	public GameObject needle;

	GameObject player;

	GameObject key;

	int coalCount;
	int oldCount = 1000;
	int currentCoal;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		coalCount = GameObject.FindGameObjectsWithTag("coal").Length;
		key = GameObject.FindGameObjectWithTag("Key");
	}

	// Update is called once per frame
	void Update () {
		GameObject[] coals = GameObject.FindGameObjectsWithTag("coal");
		coalCount = coals.Length;
		if (player == null || coalCount == 0) { 
			if (key == null) {
				GameObject she = GameObject.FindGameObjectWithTag("her");
				if (she != null) {
					Vector3 final_rot = player.transform.position - she.transform.position;
					float final_look_angle = Mathf.Atan2(final_rot.y, final_rot.x) * Mathf.Rad2Deg;
					Quaternion final_new_rot = Quaternion.AngleAxis(final_look_angle, Vector3.forward);
					needle.transform.rotation = final_new_rot;
				}
				// Image[] list = gameObject.GetComponentsInChildren<Image>();
				// foreach (Image l in list)
				// {
				// 	l.enabled = false;
				// }
			} else {
				// needle.GetComponent<Image>().color = new Color(0, 1, 1, 1);
				Vector3 key_rot = player.transform.position - key.transform.position;
				float key_look_angle = Mathf.Atan2(key_rot.y, key_rot.x) * Mathf.Rad2Deg;
				Quaternion key_new_rot = Quaternion.AngleAxis(key_look_angle, Vector3.forward);
				needle.transform.rotation = key_new_rot;
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
