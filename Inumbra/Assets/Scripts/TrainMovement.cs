using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour {

    public Vector2 target;
    public float speed;
    Vector2 position;
    
    // FOR ALPHA
    public GameObject startTrain;


	// Use this for initialization
	void Start () {
        position = gameObject.transform.position;
    }

	// Update is called once per frame
	void Update () {
        if(PlayerPrefs.GetInt("Fuel") > 0){
            startTrain.SetActive(true);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);	
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Check for fuel and fill tank
            
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(other.GetComponent<PlayerInventoryController>().fuelAmount > 0){
                if(Input.GetKeyDown(KeyCode.E))
                { 
                speed = 1f;
                }
            }
            
        }
    }
    
   
}
