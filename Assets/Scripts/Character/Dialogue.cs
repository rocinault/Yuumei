using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue
{
	private Character character;
	private Queue<string> sentences = new Queue<string>();

	private int index;
	private int scene;

	private bool finished = true;

	// create a new dialogue.
	public Dialogue(Character character, int scene, int index)
	{
		sentences.Clear();
		finished = false;

		this.index = index;
		this.scene = scene;

		this.character = character;

		foreach (string sentence in character.GetText(scene, index))
		{
			sentences.Enqueue(sentence);
		}
	}

	// Set the name, replace the narrator with nothing.
	public string Title()
	{
		if (character.title == "Narrator")
		{
			return "";
		}
		else if (character.title == "Guard")
		{
			return "???";
		}

		return character.title;
	}

	// Set the text.
	public string Text()
	{
		if (sentences.Count > 0)
		{
			if (sentences.Count == 1)
			{
				finished = true;
			}

			string output = sentences.Dequeue();

			output = CheckForAccessory(output);
			output = CheckForIcon(output);
			output = CheckForAudio(output);
			output = CheckForAction(output);
			output = CheckForExpression(output);

			return output;
		}
		else
		{
			return null;
		}
	}

	// check if an accessory needs to be changed.
	private string CheckForAccessory(string output)
	{
		if (output.Contains("^"))
		{
			string[] split = output.Split('^');

			string[] s = split[1].Split('_');

			Image accessory = character.transform.Find("Accessories").GetComponent<Image>();

			accessory.sprite = Resources.Load<Sprite>("Accessories/" + character.title + "_" + s[0]);

			Expression(s[1]);

			return split[0];

		}
		return output;
	}

	// Check if an icon needs to appear.
	private string CheckForIcon(string output)
	{
		if (output.Contains("+"))
		{
			string[] split = output.Split('+');
			VisualNovelController.instance.Icon(split[1]);
			return split[0];
		}

		return output;
	}

	// Check for sfx.
	private string CheckForAudio(string output)
	{
		if (output.Contains("@"))
		{
			string[] split = output.Split('@');

			AudioClip clip = Resources.Load<AudioClip>("Audio/" + split[1]);

			GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioSource>().clip = clip;
			GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioSource>().Play();

			return split[0];
		}
		else if (output.Contains("&"))
		{
			string[] split = output.Split('&');

			AudioClip clip = Resources.Load<AudioClip>("Audio/" + split[1]);

			GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip = clip;
			GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>().Louder();

			return split[0];
		}
		else if (output.Contains("|"))
		{
			string[] split = output.Split('|');

			VisualNovelController.instance.bFadeAudio = true;

			return split[0];
		}

		return output;
	}

	// Check if an action needs to be performed.
	private string CheckForAction(string output)
	{
		if (output.Contains("*"))
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().Shake();

			string[] split = output.Split('*');

			return split[0];
		}

		return output;
	}

	// Check if an expression needs to be changed.
	private string CheckForExpression(string output)
	{
		if (output.Contains("_"))
		{
			string[] split = output.Split('_');
			Expression(split[1]);
			return split[0];
		}

		return output;
	}

	// Set the expression.
	private void Expression(string expression)
	{
		Image face = character.transform.Find("FacialExpressions").GetComponent<Image>();
		face.sprite = Resources.Load<Sprite>("Expressions/" + character.title + "/" + character.title + "_" + expression);
	}

	// See if the character has no more dialogue.
	public bool IsFinished()
	{
		return finished;
	}
}
