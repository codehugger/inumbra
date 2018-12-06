using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour {

	public float fuelAmount = 0;

	public AudioClip coalPickupSound;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coal")
        {
			fuelAmount += 25;
			PlayerPrefs.SetInt("Fuel", PlayerPrefs.GetInt("Fuel", 0) + 25);
			PlayerPrefs.SetString("Talk", "Found Fuel!");
			StartCoroutine(CoalPickup(other));
        }
    }

	IEnumerator CoalPickup(Collider2D other) {
		Destroy(other.gameObject);
		audioSource.PlayOneShot(coalPickupSound);
		yield return new WaitForSeconds(coalPickupSound.length);
	}
}
