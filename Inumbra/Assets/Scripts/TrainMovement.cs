using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrainMovement : MonoBehaviour {

    public Vector2 target;
    public float speed;

    public GameObject fadeScreen;
    Vector2 position;

    public TextMeshProUGUI helpText;

    // FOR ALPHA
    public GameObject startTrain;

    public string nameOfNextScene;
    public bool useTrainScene = true;


	// Use this for initialization
	void Start() {
        position = gameObject.transform.position;
        Cursor.visible = false;
    }

	// Update is called once per frame
	void Update() {
        if(PlayerPrefs.GetInt("Fuel") > 0){
            startTrain.SetActive(true);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            //Check for fuel and fill tank
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && speed <= 0) {
            if (PlayerPrefs.GetInt("Fuel") > 0) {
                helpText.gameObject.SetActive(true);
                helpText.SetText("Press E to start train");

                if (Input.GetKeyDown(KeyCode.E)) {
                    //PlayerPrefs.SetString("Talk", "THE END!");
                    speed = 1f;
                    helpText.SetText("Thank you for playing the Alpha version");
                    StartCoroutine(EndScene());
                }
            }
        }
    }

    private void OnTriggerExit2D() {
        helpText.gameObject.SetActive(false);
    }

    IEnumerator EndScene() {
        yield return new WaitForSeconds(5);
        fadeScreen.GetComponent<ScreenBlackout>().startFade = true;
        if (useTrainScene) {
            SceneManager.LoadScene("Train");
        } else {
            SceneManager.LoadScene(nameOfNextScene);
        }
    }
}
