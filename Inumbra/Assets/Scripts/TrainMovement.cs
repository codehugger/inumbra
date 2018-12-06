using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainMovement : MonoBehaviour {

    public Vector2 target;
    public float speed;
    Vector2 position;

    public TextMeshProUGUI helpText;

    // FOR ALPHA
    public GameObject startTrain;


	// Use this for initialization
	void Start () {
        position = gameObject.transform.position;
    }

	// Update is called once per frame
	void Update () {
        if(PlayerPrefs.GetInt("Fuel") > 0){
            Debug.Log(PlayerPrefs.GetInt("Fuel"));
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
            if(PlayerPrefs.GetInt("Fuel") > 0){
                helpText.gameObject.SetActive(true);
                helpText.SetText("Press E to start train");
                if(Input.GetKeyDown(KeyCode.E))
                {
                    PlayerPrefs.SetString("Talk", "THE END!");
                    speed = 1f;
                }

                StartCoroutine(EndGame());
            }
        }
    }

    private void OnTriggerExit2D(){
        helpText.gameObject.SetActive(false);
    }

    IEnumerator EndGame() {
        yield return new WaitForSeconds(1);
    }
}
