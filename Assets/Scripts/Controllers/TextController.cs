using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
	public TextMeshProUGUI title;
	public TextMeshProUGUI text;

	private float speed = 0.01f;

	// set the next element for the current text.
	public void Next(Dialogue dialogue)
	{
		SetTitle(dialogue.Title());

		SetFont(title.text);
		SetText(dialogue.Text());

		VisualNovelController.instance.bWaitForInput = true;
	}

	// Set the name of who is speaking.
	private void SetTitle(string value)
	{
		title.text = value;
	}

	// set the text.
	private void SetText(string value)
	{
		text.text = "";

		string output = value.Replace("(username)", GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().title);

		StopAllCoroutines();
		StartCoroutine(EffectTypeText(output));
	}

	// Effect for displaying the text.
	private IEnumerator EffectTypeText(string sentence)
	{
		foreach (char character in sentence.ToCharArray())
		{
			text.text += character;
			
			text.CrossFadeAlpha(1.0f, 0.1f, false);

			yield return new WaitForSeconds(speed);
		}

		if (text.text == sentence)
		{
			VisualNovelController.instance.bWaitForInput = false;
		}
	}

	// Swap the font for when the narrator is speaking.
	private void SetFont(string title)
	{
		if (title == "")
		{
			text.fontStyle = FontStyles.Italic;
		}
		else
		{
			text.fontStyle = FontStyles.Normal;
		}
	}

	// Clear the current name and text for when a new character speaks.
	public void Clear()
	{
		text.text = "";
		title.text = "";
	}
}
