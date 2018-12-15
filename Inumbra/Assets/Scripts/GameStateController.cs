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

	// Use this for initialization
	void Start () {
		if (wipeOnStart) { PlayerPrefs.DeleteAll(); }
		PlayerPrefs.SetFloat("FuelLevel", startingFuelLevel);
		PlayerPrefs.SetFloat("PlayerHitPoints", startingPlayerHitPoints);
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
	}

	// Update is called once per frame
	void Update () {
		HandleChanges();
		HandleTextDisplay();
		HandlePlayerDeath();
	}

	void HandleChanges() {
		var tempFuelLevel = PlayerPrefs.GetFloat("FuelLevel", 0.0f);
		var tempTalk = PlayerPrefs.GetString("Talk", "");
		var tempPlayerHitPoints = PlayerPrefs.GetFloat("PlayerHealth", 0.0f);

		fuelLevelChanged = tempFuelLevel != fuelLevel;
		talkChanged = talk != tempTalk;
		playerHitPointsChanged = playerHitPoints != tempPlayerHitPoints;

		fuelLevel = tempFuelLevel;
		talk = tempTalk;
		playerHitPoints = tempPlayerHitPoints;
	}

	void HandleTextDisplay() {
		if (!talkChanged) { return; }
		if (textUI != null && talkChanged && !displayingText) {
			StartCoroutine(DisplayText());
		}
	}

	void HandlePlayerDeath() {
		if (playerHitPoints <= 0) {
			// StartCoroutine(ReloadOnDeath());
		}
	}

	IEnumerator ReloadOnDeath(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	IEnumerator DisplayText() {
		//yield return new WaitForSeconds(1);
		if (talk != ""){
			textUI.SetText(talk);
			displayingText = true;
			background.SetActive(true);
			text.SetActive(true);

			// Disable game objects (pause)
			if (gauge != null) { gauge.SetActive(false); }
			if (compass != null) { compass.SetActive(false); }
			if (player != null) { player.GetComponent<PlayerMovement>().enabled = false; }
			enemies = GameObject.FindGameObjectsWithTag("Shade");
			foreach (var enemy in enemies) {
				var spriteRenderer = enemy.GetComponent<EnemyController>();
				spriteRenderer.enabled = false;
			}

			yield return new WaitForSeconds(3);
			continueText.SetActive(true);
			while(!Input.anyKeyDown){
				yield return null;
			}

			// Disable text related components
			background.SetActive(false);
			text.SetActive(false);
			displayingText = false;
			continueText.SetActive(false);

			// Enable game objects (resume)
			if (gauge != null) { gauge.SetActive(true); }
			if (compass != null) { compass.SetActive(true); }
			if (player != null) { player.GetComponent<PlayerMovement>().enabled = true; }
			foreach (var enemy in enemies) {
				var spriteRenderer = enemy.GetComponent<EnemyController>();
				spriteRenderer.enabled = true;
			}
			PlayerPrefs.SetString("Talk", "");
		}
	}
}
