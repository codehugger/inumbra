using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTalkingText : MonoBehaviour {
	public GameObject text;
	public GameObject background;
	private TextMeshProUGUI textUI;

	private string talkingText;
	private bool textChanged;
	private bool displayingText;

	void Start() {
		PlayerPrefs.SetString("Talk", "");
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
			Debug.Log("Text Changed");
			Debug.Log(talkingText);
		}

		if (textUI != null && textChanged && !displayingText) {
			StartCoroutine(DisplayText());
		} else {
			Debug.Log(PlayerPrefs.GetString("Talk"));
		}
	}

	IEnumerator DisplayText() {
		textUI.SetText(talkingText);
		displayingText = true;
		background.SetActive(true);
		text.SetActive(true);
		yield return new WaitForSeconds(4);
		background.SetActive(false);
		text.SetActive(false);
		displayingText = false;
	}
}
