using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject HUD;

	// Swap between the player name screen and main menu (one way only).
	public void Swap()
	{
		mainMenu.gameObject.SetActive(false);
		HUD.gameObject.SetActive(true);
	}
}
