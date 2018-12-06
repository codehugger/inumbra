using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour {

	public float fuelAmount = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coal")
        {
		fuelAmount += 25;
		Destroy(other.gameObject);

        }
        
    }
	
}
