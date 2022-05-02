using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	[SerializeField] private Transform _camTransform;

	// How long the object should shake for.
	[SerializeField] private float _shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	[SerializeField] private float _shakeAmount = 0.25f;
	[SerializeField] private float _decreaseFactor = 1.0f;

	[SerializeField] private bool _shaketrue= false;

	Vector3 originalPos;

	void Awake()
	{
		if (_camTransform == null)
		{
			_camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = _camTransform.localPosition;
	}

	void Update()
	{
		if (_shaketrue) 
		{
			if (_shakeDuration > 0) {
				_camTransform.localPosition = originalPos + Random.insideUnitSphere * _shakeAmount;

				_shakeDuration -= Time.deltaTime * _decreaseFactor;
			} else {
				_shakeDuration = 0.25f;
				_camTransform.localPosition = originalPos;
				_shaketrue = false;
			}
		}
	}

	public void shakecamera()
	{
		_shaketrue = true;
	}
}
