using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	private static GameObject instance;

	// Have the object persist throughout the game.
	private void Awake()
	{
		if (instance == null)
		{
			instance = this.gameObject;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
}
