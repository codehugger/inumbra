using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class LanternController : MonoBehaviour {

	public GameObject lightCollider;
	public GameObject lightVisual;

	public float maxRay = 25;
	public float minRay = 5;
	public bool activated = false;

	public float speed = 75;

	Vector3 spotlightChangeVector = new Vector3(1, 0, 0);
	Vector3 aoeChangeVector = new Vector3(0, 1, 0);

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && lightVisual.transform.localScale.x > minRay) {
			lightVisual.transform.localScale -= spotlightChangeVector * Time.deltaTime * speed;
			lightCollider.transform.localScale -= aoeChangeVector * Time.deltaTime * speed / 3;
			lightVisual.GetComponent<LightSprite>();
		} else if (!Input.GetButton("Fire1") && lightVisual.transform.localScale.x < maxRay) {
			lightVisual.transform.localScale += spotlightChangeVector * Time.deltaTime * speed * 3;
			lightCollider.transform.localScale += aoeChangeVector * Time.deltaTime * speed;
		}
		if (lightVisual.transform.localScale.x > maxRay) {
			Vector3 tmp = lightVisual.transform.localScale;
			tmp.x = maxRay;
			lightVisual.transform.localScale = tmp;
		} else if (lightVisual.transform.localScale.x < minRay) {
			Vector3 tmp = lightVisual.transform.localScale;
			tmp.x = minRay;
			lightVisual.transform.localScale = tmp;
		}
		if (lightCollider.transform.localScale.y > 8) {
			Vector3 tmp = lightCollider.transform.localScale;
			tmp.y = 8;
			lightCollider.transform.localScale = tmp;
		} else if (lightCollider.transform.localScale.y < 1.8) {
			Vector3 tmp = lightCollider.transform.localScale;
			tmp.y = 1.8f;
			lightCollider.transform.localScale = tmp;
		}
	}
}
