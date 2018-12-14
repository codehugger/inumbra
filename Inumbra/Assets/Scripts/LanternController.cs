using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class LanternController : MonoBehaviour {

	public GameObject lightCollider;
	public GameObject lightVisual;
	public GameObject lightAura;

	public float maxRay = 25;
	public float minRay = 8;
	public bool activated = false;

	public float speed = 75;
	public float damagePerSecond = 5;
	[HideInInspector]
	public float currentDamage = 0;
	[HideInInspector]
	public float rayIntensity = 0f;

	public AudioClip inAudio;
	public AudioClip outAudio;

	LightSprite lightSprite;
	Color originalLightColor;
	LightSprite lightAuraSprite;

	Color originalAuraColor;

	Vector3 spotlightChangeVector = new Vector3(1, 0, 0);
	Vector3 aoeChangeVector = new Vector3(0, 1, 0);
	Vector3 auraChangeVector = new Vector3(1, 1, 0);

	bool focusStarted = false;
	bool inFocus = false;
	bool exitFocus = false;
	bool soundDone = true;

	AudioSource audioSource;

	// Aura control
	Vector3 lightAuraOriginalScale;
	FlickeringLight lightFlicker;
	FlickeringLight lightAuraFlicker;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		lightAuraOriginalScale = lightAura.transform.localScale;
		lightSprite = lightVisual.GetComponent<LightSprite>();
		lightAuraSprite = lightAura.GetComponent<LightSprite>();

		lightFlicker = lightVisual.GetComponent<FlickeringLight>();
		lightAuraFlicker = lightAura.GetComponent<FlickeringLight>();

		originalAuraColor = lightAuraSprite.Color;
		originalLightColor = lightSprite.Color;
	}

	// Update is called once per frame
	void Update () {
		bool sprintEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("XBox L2") > 0.2 || Input.GetButton("Fire2");
		float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
		bool isMoving = (in_x != 0 || in_y != 0);

		if (sprintEnabled && isMoving) {
			lightSprite.Color.a = 0;
			currentDamage = 0;
			rayIntensity = 0;
			lightCollider.SetActive(false);
			audioSource.Stop();
			return;
		} else {
			lightCollider.SetActive(true);
		}

		// XBox triggers are 0 to 1
		// PS4 triggers are -1 to 1

		if ((Input.GetButton("Fire1") || Input.GetAxis("XBox R2") > 0.2) && lightVisual.transform.localScale.x > minRay) {
			lightFlicker.StopFlickering = true;
			lightVisual.transform.localScale -= spotlightChangeVector * Time.deltaTime * speed;
			lightCollider.transform.localScale -= aoeChangeVector * Time.deltaTime * speed / 3;
			lightAura.transform.localScale -= auraChangeVector * Time.deltaTime * speed / 2;
			lightSprite.Color.a = Mathf.Lerp(lightSprite.Color.a, 1, Time.deltaTime * speed/10);
			lightSprite.Color.r = Mathf.Lerp(lightSprite.Color.r, 1, Time.deltaTime * speed/10);
			lightSprite.Color.g = Mathf.Lerp(lightSprite.Color.g, 1, Time.deltaTime * speed/10);
			lightSprite.Color.b = Mathf.Lerp(lightSprite.Color.b, 1, Time.deltaTime * speed/10);
			if (!focusStarted){
				focusStarted = true;
				audioSource.clip = inAudio;
				audioSource.loop = true;
				exitFocus = false;
				StartCoroutine(PlayLanternSound());
			}
		} else if (!Input.GetButton("Fire1") && Input.GetAxis("XBox R2") < 0.2 && lightVisual.transform.localScale.x < maxRay) {
			lightFlicker.StopFlickering = false;
			lightVisual.transform.localScale += spotlightChangeVector * Time.deltaTime * speed * 3;
			lightCollider.transform.localScale += aoeChangeVector * Time.deltaTime * speed;
			lightAura.transform.localScale += auraChangeVector * Time.deltaTime * speed * 2;
			lightSprite.Color.a = Mathf.Lerp(lightSprite.Color.a, originalLightColor.a, Time.deltaTime * speed/10);
			lightSprite.Color.r = Mathf.Lerp(lightSprite.Color.r, originalLightColor.r, Time.deltaTime * speed/10);
			lightSprite.Color.g = Mathf.Lerp(lightSprite.Color.g, originalLightColor.g, Time.deltaTime * speed/10);
			lightSprite.Color.b = Mathf.Lerp(lightSprite.Color.b, originalLightColor.b, Time.deltaTime * speed/10);
			if (!exitFocus) {
				focusStarted = false;
				exitFocus = true;
				audioSource.clip = outAudio;
				audioSource.loop = false;
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
		} else if (lightCollider.transform.localScale.y < 1.8f) {
			Vector3 tmp = lightCollider.transform.localScale;
			tmp.y = 1.8f;
			lightCollider.transform.localScale = tmp;
		}

		lightAura.transform.localScale = new Vector3(
			Mathf.Clamp(lightAura.transform.localScale.x, 0, lightAuraOriginalScale.x),
			Mathf.Clamp(lightAura.transform.localScale.y, 0, lightAuraOriginalScale.y),
			0);

		float clamp_alpha = Mathf.Clamp(lightSprite.Color.a, 0.3f, 0.8f);
		lightSprite.Color.a = clamp_alpha;
		// convert light to normalized rayIntensity
		float lightX = Mathf.Clamp(lightVisual.transform.localScale.x, minRay, maxRay);
		rayIntensity = Mathf.Clamp(1 - (lightX-minRay)/(maxRay-minRay), 0.01f, 1.0f);
		currentDamage = rayIntensity * damagePerSecond;
	}

	IEnumerator PlayLanternSound(){
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length);
	}
}
