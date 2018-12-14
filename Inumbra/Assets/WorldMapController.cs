using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapController : MonoBehaviour {

	public GameObject worldMapImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Player")
        {
			worldMapImage.gameObject.SetActive(true);
		}

	}

	private void OnTriggerExit2D(Collider2D other) {

		if (other.gameObject.tag == "Player")
        {
			worldMapImage.gameObject.SetActive(false);
		}

	}
}
