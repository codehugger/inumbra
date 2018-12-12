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
	public float damagePerSecond = 5;
	[HideInInspector]
	public float currentDamage = 0;
	[HideInInspector]
	public float rayIntensity = 0f;

	public AudioClip inAudio;
	public AudioClip outAudio;

	Vector3 spotlightChangeVector = new Vector3(1, 0, 0);
	Vector3 aoeChangeVector = new Vector3(0, 1, 0);

	bool focusStarted = false;
	bool inFocus = false;
	bool exitFocus = false;
	bool soundDone = true;

	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

		bool sprintEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("XBox L2") > 0.2 || Input.GetButton("Fire2");
		float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
		bool isMoving = (in_x != 0 || in_y != 0);

		if (sprintEnabled && isMoving) {
			lightVisual.GetComponent<LightSprite>().Color.a = 0;
			currentDamage = 0;
			rayIntensity = 0;
			lightCollider.SetActive(false);
			return;
		} else {
			lightCollider.SetActive(true);
		}

		// XBox triggers are 0 to 1
		// PS4 triggers are -1 to 1

		if ((Input.GetButton("Fire1") || Input.GetAxis("XBox R2") > 0.2) && lightVisual.transform.localScale.x > minRay) {
			lightVisual.transform.localScale -= spotlightChangeVector * Time.deltaTime * speed;
			lightCollider.transform.localScale -= aoeChangeVector * Time.deltaTime * speed / 3;
			float cur_alpha = lightVisual.GetComponent<LightSprite>().Color.a;
			lightVisual.GetComponent<LightSprite>().Color.a = Mathf.Lerp(cur_alpha, 0.8f, Time.deltaTime * speed/10);
			if (!focusStarted){
				focusStarted = true;
				audio.clip = inAudio;
				audio.loop = true;
				exitFocus = false;
				StartCoroutine(PlayLanternSound());
			}
		} else if (!Input.GetButton("Fire1") && Input.GetAxis("XBox R2") < 0.2 && lightVisual.transform.localScale.x < maxRay) {
			lightVisual.transform.localScale += spotlightChangeVector * Time.deltaTime * speed * 3;
			lightCollider.transform.localScale += aoeChangeVector * Time.deltaTime * speed;
			float cur_alpha = lightVisual.GetComponent<LightSprite>().Color.a;
			lightVisual.GetComponent<LightSprite>().Color.a = Mathf.Lerp(cur_alpha, -0.3f, Time.deltaTime * speed/10);
			if (!exitFocus) {
				focusStarted = false;
				exitFocus = true;
				audio.clip = outAudio;
				audio.loop = false;
				StartCoroutine(PlayLanternSound());
			}
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

		float clamp_alpha = Mathf.Clamp(lightVisual.GetComponent<LightSprite>().Color.a, 0.3f, 0.8f);
		lightVisual.GetComponent<LightSprite>().Color.a = clamp_alpha;
		// convert light to normalized rayIntensity
		float lightX = Mathf.Clamp(lightVisual.transform.localScale.x, minRay, maxRay);
		rayIntensity = Mathf.Clamp(1 - (lightX-minRay)/(maxRay-minRay), 0.01f, 1.0f);
		currentDamage = rayIntensity * damagePerSecond;
	}

	IEnumerator PlayLanternSound(){
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		
	}
}
