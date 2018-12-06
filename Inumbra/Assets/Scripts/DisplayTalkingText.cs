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
		yield return new WaitForSeconds(2);
		background.SetActive(false);
		text.SetActive(false);
		displayingText = false;
	}
}
