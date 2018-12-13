using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTalkingText : MonoBehaviour {
	public GameObject text;
	public GameObject continueText;
	public GameObject background;
	public GameObject compass;
	public GameObject gauge;
	public GameObject player;
	public GameObject enemies;
	private TextMeshProUGUI textUI;

	private string talkingText;
	private bool textChanged;
	private bool displayingText;

	void Start() {
		textUI = text.GetComponentInChildren<TextMeshProUGUI>();
		talkingText = PlayerPrefs.GetString("Talk");
		background.SetActive(false);
		text.SetActive(false);
	}

	void Update() {
		textChanged = false;

		if (talkingText != PlayerPrefs.GetString("Talk")) {
			textChanged = true;
			talkingText = PlayerPrefs.GetString("Talk");
		}

		if (textUI != null && textChanged && !displayingText) {
			StartCoroutine(DisplayText());
		}
	}

	IEnumerator DisplayText() {
		yield return new WaitForSeconds(1);
		textUI.SetText(talkingText);
		displayingText = true;
		background.SetActive(true);
		text.SetActive(true);
		gauge.SetActive(false);
		compass.SetActive(false);
		player.SetActive(false);
		yield return new WaitForSeconds(3);
		continueText.SetActive(true);
		while(!Input.anyKeyDown){
			yield return null;
		}
		background.SetActive(false);
		text.SetActive(false);
		displayingText = false;
		continueText.SetActive(false);
		gauge.SetActive(true);
		compass.SetActive(true);
		player.SetActive(true);
	}
}
