using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrainMovement : MonoBehaviour {

    Vector2 position;
    bool inTrainScene;
    float currentSpeed;

    public bool doorsClosed = false;
    public GameObject doors;

    public Vector3 target;
    public float speed;

    public GameObject fadeScreen;

    public TextMeshProUGUI helpText;

    // FOR ALPHA
    public GameObject startTrain;

    public string nameOfNextScene;
    public bool useTrainScene = true;
    public float fadeOutTime = 5.0f;
    public float timeOnTrain = 10.0f;


	// Use this for initialization
	void Start() {
        position = gameObject.transform.position;
        Cursor.visible = false;
        currentSpeed = speed;
        inTrainScene = SceneManager.GetActiveScene().name == "Train";

        // if (inTrainScene) {
        //     currentSpeed = 1.0f;
        // }
    }

	// Update is called once per frame
	void Update() {
        if (!inTrainScene) {
            if(PlayerPrefs.GetInt("FuelLevel") == 1.0f){
                startTrain.SetActive(true);
            }
        } else {
            // StartCoroutine(EndScene());
            target = Vector3.up;
        }
        float step = currentSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + target, step);
        if (doors != null) {
            if (doorsClosed) {
                doors.SetActive(true);
            } else {
                doors.SetActive(false);
            }
        }
    }

    // private void OnTriggerStay2D(Collider2D other) {
    //     if (other.gameObject.tag == "Player" && currentSpeed <= 0) {
    //         if (PlayerPrefs.GetInt("FuelLevel") == 1.0f) {
    //             helpText.gameObject.SetActive(true);
    //             helpText.SetText("Press E to start train");

    //             if (Input.GetKeyDown(KeyCode.E)) {
    //                 //PlayerPrefs.SetString("Talk", "THE END!");
    //                 currentSpeed = 1f;
    //                 // StartCoroutine(EndScene());
    //             }
    //         }
    //     }
    // }

    // private void OnTriggerExit2D() {
    //     if (currentSpeed <= 0 && helpText != null) {
    //         helpText.gameObject.SetActive(false);
    //     }
    // }

    IEnumerator EndScene() {

        // if in the train scene wait until time on train has passed
        if (inTrainScene) {
            yield return new WaitForSeconds(timeOnTrain);
        }

        // wait and then start screen fade out
        yield return new WaitForSeconds(fadeOutTime);
        fadeScreen.GetComponent<ScreenBlackout>().startFade = true;

        // wait for the screen to go black
        yield return new WaitForSeconds(1);

        if (useTrainScene) {
            // load train scene as the next scene and store the name of the next scene
            // in an intermediary place through PlayerPrefs
            PlayerPrefs.SetString("NextScene", nameOfNextScene);
            SceneManager.LoadScene("Train");
        } else if (SceneManager.GetActiveScene().name == "Train") {
            // load the next scene stored in NextScene or reload current scene if not set
            SceneManager.LoadScene(PlayerPrefs.GetString("NextScene", SceneManager.GetActiveScene().name));
        } else {
            // load next scene directly skipping the train intermediary scene
            SceneManager.LoadScene(nameOfNextScene);
        }
    }
}
