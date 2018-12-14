using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControlsOnClick : MonoBehaviour {
	public GameObject controlsImage;
	public GameObject mainMenu;

	public void ShowControls() {
		controlsImage.SetActive(true);
		mainMenu.SetActive(false);
	}

	public void HideControls() {
		controlsImage.SetActive(false);
		mainMenu.SetActive(true);
	}
}
