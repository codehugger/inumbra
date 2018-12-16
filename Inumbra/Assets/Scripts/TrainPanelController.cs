using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TrainPanelController : MonoBehaviour {

    [Range(0.01f, 1.0f)]
    public float requiredFuel = 1.0f;

    public TextMeshProUGUI helpText;
    public GameObject fadeScreen;
    public string nameOfNextScene;
    public bool useTrainScene = true;
    public float fadeOutTime = 5.0f;
    public float timeOnTrain = 10.0f;

    public AudioClip[] trainMusic;
    public AudioClip trainTrackSound;

    AudioClip currentMusicTrack;
    AudioSource audioSource;

    bool inTrainScene;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        inTrainScene = SceneManager.GetActiveScene().name == "Train";

        if (inTrainScene) {
            currentMusicTrack = trainMusic[Random.Range(0, trainMusic.Length)];
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(trainTrackSound);
            audioSource.PlayOneShot(currentMusicTrack);
        }
	}

	// Update is called once per frame
	void Update () {

	}

	  private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" ) {
            if (inTrainScene) {
                StartCoroutine(EndScene());
            } else if (PlayerPrefs.GetFloat("FuelLevel") >= requiredFuel) {
                helpText.gameObject.SetActive(true);
                helpText.SetText("Press X to start train");

                if (Input.GetButton("Submit") || Input.GetKey(KeyCode.X) || Input.GetButton("Square")) {
                    StartCoroutine(EndScene());
                    GameObject.FindGameObjectWithTag("Train").GetComponent<TrainMovement>().doorsClosed = true;
                }
            }
        }
    }

	private void OnTriggerExit2D() {
        if (helpText != null) { helpText.gameObject.SetActive(false); }
    }

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
            // PlayerPrefs.SetString("NextScene", nameOfNextScene);
            SceneManager.LoadScene("Train");
        } else if (inTrainScene) {
            // load the next scene stored in NextScene or reload current scene if not set
            SceneManager.LoadScene(PlayerPrefs.GetString("NextScene"));
        } else {
            // load next scene directly skipping the train intermediary scene
            SceneManager.LoadScene(nameOfNextScene);
        }
    }
}
