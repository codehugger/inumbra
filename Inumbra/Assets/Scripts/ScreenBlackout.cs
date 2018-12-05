using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlackout : MonoBehaviour {


	public bool startFade = false;
	public float fadeSpeed = 1;

	float alpha = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startFade && alpha < 1){
			Color color = GetComponent<Image>().color;	
			color.a = alpha;	
			GetComponent<Image>().color = color;
			alpha += fadeSpeed * Time.deltaTime;
		}
		else if (!startFade){
			alpha = 0;
			GetComponent<Image>().color = new Color(0, 0, 0, alpha);
		}
	}

	public void Fade() {
		startFade = true;
	}
}
