using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {

	public TextAsset cutSceneTextFile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("Talk", cutSceneTextFile.text);
			StartCoroutine(LoadCredits());
		}
	}

	IEnumerator LoadCredits() {
		yield return new WaitForSeconds(4f);
		SceneManager.LoadScene("Credits");
	}
}
