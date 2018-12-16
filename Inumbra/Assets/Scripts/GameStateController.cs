using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStateController : MonoBehaviour {

	// Text display objects
	TextMeshProUGUI textUI;
	string talkingText;
	bool textChanged;
	bool displayingText = false;
	bool talkChanged = false;
	string talk = "";

	// The main game objects
	GameObject gauge;
	GameObject compass;
	GameObject player;
	GameObject[] enemies;

	// Global behaviour
	[Range(0.0f, 1.0f)]
	public float startingFuelLevel = 0.0f;
	public float startingPlayerHitPoints = 30.0f;
	public bool turnOnLantern = true;
	public bool enemiesCanMove = true;
	public bool wipeOnStart = false;
	public GameObject text;
	public GameObject continueText;
	public GameObject background;

	// Player stats
	public float healingRate = 1;
	public float playerHitPoints;
	public bool playerHitPointsChanged = false;

	// Fuel management
	float fuelLevel = 0.0f;
	bool fuelLevelChanged = false;
	bool playerDead = false;
	bool handlingPlayerDeath = false;
	public string groundMaterial = "gravel";

	// Use this for initialization
	void OnEnable() {
		if (wipeOnStart) { PlayerPrefs.DeleteAll(); }
		PlayerPrefs.SetFloat("FuelLevel", startingFuelLevel);
		PlayerPrefs.SetFloat("InitialHitPoints", startingPlayerHitPoints);
		PlayerPrefs.SetFloat("CurrentHitPoints", startingPlayerHitPoints);
		PlayerPrefs.SetInt("EnemiesCanMove", enemiesCanMove ? 1 : 0);
		PlayerPrefs.SetInt("TurnOnLanter", turnOnLantern ? 1 : 0);
		PlayerPrefs.SetString("Talk", "");
		PlayerPrefs.SetInt("HasKey", 0);

		// Get the relevant game objects through tags
		gauge = GameObject.FindGameObjectWithTag("FuelGauge");
		compass = GameObject.FindGameObjectWithTag("Compass");
		player = GameObject.FindGameObjectWithTag("Player");
		// enemies = GameObject.FindGameObjectsWithTag("Shade");

		// Text display
		if (text != null) {
			textUI = text.GetComponentInChildren<TextMeshProUGUI>();
			text.SetActive(false);
		}

		if (background != false) { background.SetActive(false); }

		talk = "";
		playerHitPoints = startingPlayerHitPoints;
		handlingPlayerDeath = false;
	}

	// Update is called once per frame
	void Update () {
		HandleTextDisplay();
		HandleChanges();
		HandlePlayerDeath();
	}

	void HandleChanges() {
		// Record current state
		var tempFuelLevel = PlayerPrefs.GetFloat("FuelLevel", 0.0f);
		var tempTalk = PlayerPrefs.GetString("Talk", "");
		var tempPlayerHitPoints = PlayerPrefs.GetFloat("CurrentHitPoints", 0.0f);

		// Compare current state to previous state
		fuelLevelChanged = tempFuelLevel != fuelLevel;
		talkChanged = talk != tempTalk;
		playerHitPointsChanged = playerHitPoints != tempPlayerHitPoints;

		// Update current state
		fuelLevel = tempFuelLevel;
		talk = tempTalk;
		playerHitPoints = tempPlayerHitPoints;

		if (fuelLevelChanged && fuelLevel == 1 && SceneManager.GetActiveScene().name != "Train") {
			PlayerPrefs.SetString("Talk", "That's enough coal, I should get back to the train.");
		}
	}

	void HandleTextDisplay() {
		if (!talkChanged) { return; }
		if (textUI != null && talkChanged && !displayingText && !handlingPlayerDeath) {
			StartCoroutine(DisplayText());
		}
	}

	void HandlePlayerDeath() {
		if (playerHitPoints <= 0 && !handlingPlayerDeath) {
			StartCoroutine(ReloadOnDeath());
		}
	}

	void EnemiesEnabled(bool enabled) {
		enemies = GameObject.FindGameObjectsWithTag("Shade");
		foreach (var enemy in enemies) {
			var enemyController = enemy.GetComponent<EnemyController>();
			enemyController.enabled = enabled;
		}
	}

	void UIEnabled(bool enabled) {
		if (gauge != null) { gauge.SetActive(enabled); }
		if (compass != null) { compass.SetActive(enabled); }
		if (player != null) {
			player.GetComponent<PlayerMovement>().enabled = enabled;
			GameObject.FindGameObjectWithTag("Legs").GetComponentInChildren<Animator>().enabled = enabled;
		}
	}

	void TextEnabled(bool enabled) {
		displayingText = enabled;
		background.SetActive(enabled);
		text.SetActive(enabled);
	}

	IEnumerator ReloadOnDeath(){
		if (!handlingPlayerDeath) {
			handlingPlayerDeath = true;
			Debug.Log("Reloading on death");
			textUI.SetText("You're dead!");
			UIEnabled(false);
			EnemiesEnabled(false);
			TextEnabled(true);
			yield return new WaitForSeconds(1f);
			continueText.SetActive(true);
			while(!Input.anyKeyDown){
				yield return null;
			}
			textUI.SetText("");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		}
	}

	IEnumerator DisplayText() {
		//yield return new WaitForSeconds(1);
		if (talk != ""){
			// Disable game objects (pause)
			UIEnabled(false);
			EnemiesEnabled(false);

			// Display the text
			textUI.SetText(talk);
			TextEnabled(true);

			// Wait for user to acknowledge text
			yield return new WaitForSeconds(1.5f);
			continueText.SetActive(true);
			while(!Input.anyKeyDown){
				yield return null;
			}

			// Disable text related components
			TextEnabled(false);
			continueText.SetActive(false);

			// Enable game objects (resume)
			UIEnabled(true);
			EnemiesEnabled(true);

			// Set a clean slate for future talking
			PlayerPrefs.SetString("Talk", "");
		}
	}
}
