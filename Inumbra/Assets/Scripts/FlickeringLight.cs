using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class FlickeringLight : MonoBehaviour {
	public float MaxReduction = 0.2f;
	public float MaxIncrease = 0.2f;
	public float RateDamping = 0.1f;
	public float Strength = 300;
	public bool StopFlickering;

	private LightSprite _lightSource;
    private float _baseIntensity;
    private bool _flickering;

	public void Start() {
		_lightSource = GetComponent<LightSprite>();
		if (_lightSource == null)
		{
			Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
			return;
		}
		_baseIntensity = _lightSource.Color.a;
	}

	void Update() {
		// bool sprintEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("XBox L2") > 0.2 || Input.GetButton("Fire2");
		// float in_x = Input.GetAxis("Horizontal");
        // float in_y = Input.GetAxis("Vertical");
		// bool isMoving = (in_x != 0 || in_y != 0);
		// bool isSprinting = isMoving && sprintEnabled;

		// if (isSprinting) {
		// 	StopAllCoroutines();
		// }

		// if (!StopFlickering && !_flickering)
		// {
		// 	StartCoroutine(DoFlicker());
		// }
	}

	private IEnumerator DoFlicker() {
		_flickering = true;
		while (!StopFlickering)
		{
			_lightSource.Color.a = Mathf.Lerp(_lightSource.Color.a, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
			yield return new WaitForSeconds(RateDamping);
		}
		_flickering = false;
	}
}
