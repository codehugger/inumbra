using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour {


	public GameObject worldMapImage1;
	public GameObject worldMapImage2;
	public GameObject worldMapImage3;
	public GameObject worldMapImage4;
	public GameObject worldMapImage5;
	public GameObject worldMapImage6;
	public GameObject worldMapImage7;

	private GameObject currentWorldMap;
	
	
	// Use this for initialization
	void Start () {
		string nextScene = PlayerPrefs.GetString("NextScene");
		string currentScene = SceneManager.GetActiveScene().name;
		switch(nextScene){
			case "Forest":
				if(!(currentScene == "Train")){
					currentWorldMap = worldMapImage1;
				}else{
					currentWorldMap = worldMapImage2;
				}
				break;
			case "Wasteland":
				if(!(currentScene == "Train")){
					currentWorldMap = worldMapImage3;
				}else{
					currentWorldMap = worldMapImage4;
				}
				break;
			case "Town":
				if(!(currentScene == "Train")){
					currentWorldMap = worldMapImage5;
				}else{
					currentWorldMap = worldMapImage6;
				}
				break;
			default:
				if(currentScene == "Town"){
					currentWorldMap = worldMapImage7;
				}else{
					currentWorldMap = worldMapImage1;
				}
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Player")
        {
			currentWorldMap.gameObject.SetActive(true);
		}

	}

	private void OnTriggerExit2D(Collider2D other) {

		if (other.gameObject.tag == "Player")
        {
			currentWorldMap.gameObject.SetActive(false);
		}

	}
}
