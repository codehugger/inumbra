using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AudioSceneSwitcher : MonoBehaviour {
	public string sceneName;
	public TextMeshProUGUI firstText;
	public TextMeshProUGUI secondText;
	public float transitionDuration;

	void Start() {
		firstText.color = new Color(0,0,0,0);
		secondText.color = new Color(0,0,0,0);
		StartCoroutine(playSoundThenLoad());

		Cursor.visible = false;
	}

	IEnumerator playSoundThenLoad()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
		//yield return new WaitForSeconds(audio.clip.length);

		float textFade = audio.clip.length * 0.25f;

		// fade first text out
		float elapsed = 0.0f;
		while (elapsed <= textFade)
		{
			firstText.color = Color.Lerp(new Color(0,0,0,0), Color.white, elapsed / textFade);
			elapsed += Time.deltaTime;

			yield return null;
		}

		// fade first text out
		elapsed = 0.0f;
		while (elapsed <= textFade)
		{
			firstText.color = Color.Lerp(Color.white, new Color(0,0,0,0), elapsed / textFade);
			elapsed += Time.deltaTime;

			yield return null;
		}

		// fade second text in
		elapsed = 0.0f;
		while (elapsed <= textFade)
		{
			secondText.color = Color.Lerp(new Color(0,0,0,0), Color.white, elapsed / textFade);
			elapsed += Time.deltaTime;

			yield return null;
		}

		// fade second text out
		elapsed = 0.0f;
		while (elapsed <= textFade)
		{
			secondText.color = Color.Lerp(Color.white, new Color(0,0,0,0), elapsed / textFade);
			elapsed += Time.deltaTime;

			yield return null;
		}

		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
