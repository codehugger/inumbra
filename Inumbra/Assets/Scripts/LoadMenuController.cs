using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuController : MonoBehaviour {

	public float waitForSeconds = 10;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		StartCoroutine(LoadMenu());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadMenu() {
		yield return new WaitForSeconds(waitForSeconds);
		SceneManager.LoadScene("Menu");
	}
}
