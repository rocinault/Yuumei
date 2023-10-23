using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Camera cameraComponent;

	private Dictionary<int, Character> affinity = new Dictionary<int, Character>();

	private float speed = 5.0f;
	private float sensitivity = 125.0f;
	private float xAxisClamp = 0.0f;

	private int karma = 0;
	private bool canMove = true;

	private void Awake()
	{
		cameraComponent = gameObject.GetComponentInChildren<Camera>();
	}

	// Character inputs and movement controls.
	private void FixedUpdate()
	{
		if (canMove)
		{
			PlayerMovement();
			CameraRotation();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Karma for the player.
	public void ChangeKarma(int _value)
	{
		karma += _value;
	}

	// Read for movement inputs.
	private void PlayerMovement()
	{
		float moveForward = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
		float moveRight = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

		transform.Translate(moveRight, 0, moveForward);
	}

	// Control the camera rotation.
	private void CameraRotation()
	{
		float yRot = Input.GetAxis("Turn") * sensitivity * Time.deltaTime;
		float xRot = Input.GetAxis("LookUp") * sensitivity * Time.deltaTime;

		xAxisClamp += xRot;

		if (xAxisClamp > 90.0f)
		{
			xAxisClamp = 90.0f;
			xRot = 0.0f;
			ClampRotationAroundXAxis(270.0f);
		}
		else if (xAxisClamp < -90.0f)
		{
			xAxisClamp = -90.0f;
			xRot = 0.0f;
			ClampRotationAroundXAxis(90.0f);
		}

		cameraComponent.transform.Rotate(Vector3.left * xRot);
		transform.Rotate(Vector3.up * yRot);
	}

	// Clamp the rotation around the x axis.
	private void ClampRotationAroundXAxis(float value)
	{
		Vector3 eurlorRotation = cameraComponent.transform.eulerAngles;
		eurlorRotation.x = value;
		cameraComponent.transform.eulerAngles = eurlorRotation;
	}

	// Disable movement and camera.
	public void SetCanMove(bool _canMove)
	{
		canMove = _canMove;
	}

	// Get if the character can move.
	public bool GetCanMove()
	{
		return canMove;
	}

	// Get the current camera attahed to player.
	public Camera GetCameraComponent()
	{
		return cameraComponent;
	}
}
