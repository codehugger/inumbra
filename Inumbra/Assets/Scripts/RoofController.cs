using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofController : MonoBehaviour {

	public GameObject roof;

    bool auraColliding = false;
    bool lanternColliding = false;
    bool playerColliding = false;

	 private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Aura") { auraColliding = true; }
        if (other.gameObject.tag == "Player") { playerColliding = true; }
        if (other.gameObject.tag == "Lantern") { lanternColliding = true; }

        if (playerColliding || auraColliding || lanternColliding ) { roof.SetActive(false); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Aura") { auraColliding = false; }
        if (other.gameObject.tag == "Player") { playerColliding = false; }
        if (other.gameObject.tag == "Lantern") { lanternColliding = false; }

        if (!auraColliding && !playerColliding && !lanternColliding) { roof.SetActive(true); }
    }
}
