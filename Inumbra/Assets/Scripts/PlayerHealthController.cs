using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

	public int hitsToDie = 5;

	public float recoverySpeed = 1;

	float damage = 0;
	float maxDamage;
	Color initialColor;

	// Use this for initialization
	void Start () {
		maxDamage = hitsToDie;
		initialColor = GetComponentInChildren<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(damage);
		if (damage >= maxDamage) {
			GetComponentInChildren<SpriteRenderer>().color = Color.gray;
			GameObject.FindGameObjectWithTag("FadeScreen").GetComponent<ScreenBlackout>().Fade();
			GetComponent<PlayerMovement>().enabled = false;
		}
		else {
			GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(initialColor, Color.red, damage / maxDamage);
			damage = Mathf.Max(damage - Time.deltaTime * recoverySpeed, 0);
		}
	}

	public void TakeDamage () {
		damage += 1;
	}
}
