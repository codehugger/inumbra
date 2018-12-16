using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {

	public TextAsset cutSceneTextFile;

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("Talk", cutSceneTextFile.text);
			StartCoroutine(FoundHer());
			StartCoroutine(LoadCredits());
		}
	}

	IEnumerator FoundHer() {
		yield return new WaitForSeconds(2f);
		audioSource.Play();
	}

	IEnumerator LoadCredits() {
		yield return new WaitForSeconds(7f);
		SceneManager.LoadScene("Credits");
	}
}
