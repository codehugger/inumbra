using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

	public float healingRate = 1;
	public float hitPoints = 30;

	float currentHitPoints;
	Color initialColor;

	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
		initialColor = GetComponentInChildren<SpriteRenderer>().color;
	}

	// Update is called once per frame
	void Update () {
		if (currentHitPoints <= 0) {
			GetComponentInChildren<SpriteRenderer>().color = Color.grey;
			GameObject.FindGameObjectWithTag("FadeScreen").GetComponent<ScreenBlackout>().Fade();
			GetComponent<PlayerMovement>().enabled = false;
			StartCoroutine(reloadOnDeath());
		}
		else if (currentHitPoints < hitPoints) {
			GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(initialColor, Color.red, currentHitPoints / hitPoints);
		} else {
			GetComponentInChildren<SpriteRenderer>().color = initialColor;
		}

		Regenerate();
	}

	IEnumerator reloadOnDeath(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Regenerate() {
		if (currentHitPoints < hitPoints) {
			currentHitPoints += healingRate * Time.deltaTime;
			currentHitPoints = Mathf.Clamp(currentHitPoints, 0.0f, hitPoints);
		}
	}

	public void TakeDamage(float damage) {
		damage += 1;
	}
}
