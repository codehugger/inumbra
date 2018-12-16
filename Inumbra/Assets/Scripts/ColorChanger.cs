using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponent<TextMeshProUGUI>().color, new Color(0.1f, 0.1f, 0.1f, 1), 0.5f * Time.deltaTime);
	}
}
