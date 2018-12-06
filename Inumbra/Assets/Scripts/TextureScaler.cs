using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScaler : MonoBehaviour {


	public int pixels = 128;
	int initialHeight, initialWidth;
	UnityEngine.UI.RawImage rawImage;
	RenderTexture renderTexture;

	// Use this for initialization
	void Start()
    {
		rawImage = gameObject.GetComponentInChildren<UnityEngine.UI.RawImage>();
		renderTexture = (RenderTexture) rawImage.texture;
		initialHeight = Screen.height;
		initialWidth = Screen.width;
		float ratio =  (float) Screen.height / (float) Screen.width;
		renderTexture.height = (int) (ratio * pixels);
		renderTexture.width = pixels;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
