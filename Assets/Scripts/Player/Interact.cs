using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Interact : MonoBehaviour
{
	public GameObject dialogue;
	public GameObject foreground;

	public TextMeshProUGUI interact;
	public TextMeshProUGUI title;
	public TextMeshProUGUI text;

	private int rayLength = 2;
	private GameObject raycastedObj;

	private Player player;

	private void Awake()
	{
		player = gameObject.GetComponent<Player>();
	}

	// Check for raycast against interactable objects.
	private void Update()
	{
		LayerMask mask = LayerMask.GetMask("Interact");
		RaycastHit hit;

		Ray ray = player.GetCameraComponent().ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, rayLength, mask))
		{
			interact.text = "Press left mouse to interact with: " + hit.collider.gameObject.name;

			if (hit.collider.CompareTag("Object"))
			{
				ObjectInteraction(hit);
			}
			else if (hit.collider.CompareTag("Exit"))
			{
				ExitLevel(hit);
			}
		}
		else
		{
			interact.text = "";
		}
	}

	// Check which object collided with.
	private void ObjectInteraction(RaycastHit hit)
	{
		raycastedObj = hit.collider.gameObject;

		if (player.GetCanMove() && Input.GetMouseButtonDown(0))
		{
			player.SetCanMove(false);
			title.text = hit.collider.gameObject.name;
			Display(raycastedObj.GetComponent<Interactable>().itemInformation);
		}
		else if (!player.GetCanMove() && Input.GetMouseButtonDown(0))
		{
			player.SetCanMove(true);
			StopDisplay();
		}
	}

	// Exit the current level and load the visual novel.
	private void ExitLevel(RaycastHit hit)
	{
		raycastedObj = hit.collider.gameObject;

		if (player.GetCanMove() && Input.GetMouseButtonDown(0))
		{
			player.SetCanMove(false);
			title.text = GameManager.instance.GetPlayerName();
			Display(raycastedObj.GetComponent<Interactable>().itemInformation);
		}
		else if (!player.GetCanMove() && Input.GetMouseButtonDown(0))
		{
			StopDisplay();

			GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>().Quieter();

			foreground.GetComponent<PlayAnimation>().Play();
		}
	}

	// Display the item information.
	public void Display(string itemInfo)
	{
		dialogue.SetActive(true);

		StopAllCoroutines();
		StartCoroutine(EffectTypeText(itemInfo));
	}

	// Stop displaying the item information.
	public void StopDisplay()
	{
		title.text = "";
		text.text = "";
		StopAllCoroutines();
		dialogue.SetActive(false);
	}

	// Effect for the text appearing.
	private IEnumerator EffectTypeText(string sentence)
	{
		foreach (char character in sentence.ToCharArray())
		{
			text.text += character;
			yield return new WaitForSeconds(0.01f);
		}
	}
}
