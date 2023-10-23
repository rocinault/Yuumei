using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Canvas canvas;
	private Vector3 initialPosition;

	private float duration = 0.0f;
	private float magnitude = 0.06f;
	private float dampingSpeed = 8.0f;


	private void OnEnable()
	{
		initialPosition = transform.localPosition;
	}

	// Simple camera shake.
	private void Update()
	{
		if (duration > 0)
		{
			canvas.renderMode = RenderMode.WorldSpace;
			transform.localPosition = initialPosition + Random.insideUnitSphere * magnitude;

			duration -= Time.deltaTime * dampingSpeed;
		}
		else
		{
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			duration = 0.0f;
			transform.localPosition = initialPosition;
		}
	}

	// set the camera to shake.
	public void Shake()
	{
		duration = 1.25f;
	}
}
