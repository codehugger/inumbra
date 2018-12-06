using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour {

	public int fuelAmount = 0;

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
			if (fuelAmount >= 100) {
				PlayerPrefs.SetString("Talk", "I can't carry more coal!");
				fuelAmount = 100;
			} else if (fuelAmount >= 75) {
				PlayerPrefs.SetString("Talk", "That's definitely enough coal! I really should head back!");
			} else if (fuelAmount >= 50) {
				PlayerPrefs.SetString("Talk", "That's probably enough coal for now.");
			} else if (fuelAmount >= 25) {
				PlayerPrefs.SetString("Talk", "That's probably not enough coal.");
			}

			PlayerPrefs.SetInt("Fuel", fuelAmount);
			StartCoroutine(CoalPickup(other));
        }
    }

	IEnumerator CoalPickup(Collider2D other) {
		Destroy(other.gameObject);
		audioSource.PlayOneShot(coalPickupSound);
		yield return new WaitForSeconds(coalPickupSound.length);
	}
}
