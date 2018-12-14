using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainPanelController : MonoBehaviour {


    public GameObject fadeScreen;

    public TextMeshProUGUI helpText;

    // FOR ALPHA
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	  private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" ) {
            if (PlayerPrefs.GetInt("FuelLevel") == 1.0f) {
                helpText.gameObject.SetActive(true);
                helpText.SetText("Press E to start train");

                if (Input.GetKeyDown(KeyCode.E)) {
                    //PlayerPrefs.SetString("Talk", "THE END!");
                    // StartCoroutine(EndScene());
                }
            }
        }
    }

	 private void OnTriggerExit2D() {
        helpText.gameObject.SetActive(false);
        
    }
}
