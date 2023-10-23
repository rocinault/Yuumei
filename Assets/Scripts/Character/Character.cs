using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	// All the characters information for a particular scene.
	public struct Infomation
	{
		public Dictionary<int, Data.Character> info;

		public Infomation(Dictionary<int, Data.Character> info)
		{ this.info = info; }
	}

	[HideInInspector] public Dictionary<int, Infomation> scenes = new Dictionary<int, Infomation>();
	[HideInInspector] public string title;

	private RectTransform root;
	private Dictionary<int, Data.Character> data = new Dictionary<int, Data.Character>();

	// Store reference to the root for movement.
	private void Awake()
	{
		root = gameObject.GetComponent<RectTransform>();
	}

	// Set the current data for the 
	public void Initialise(int index, Data.Character character)
	{
		title = character.name;
		data.Add(index, character);
	}

	// Add a new scene for the character to appear in.
	public void AddScene(int scene)
	{
		if (!scenes.ContainsKey(scene))
		{
			Infomation information = new Infomation(data);
			scenes.Add(scene, information);

			data = new Dictionary<int, Data.Character>();
		}
	}

	// Get the current dialogue for a scene, based off the index in the VN controller.
	public string[] GetText(int scene, int index)
	{
		scenes.TryGetValue(scene, out Infomation info);
		info.info.TryGetValue(index, out Data.Character character);

		return character.text;
	}

	// Get the characters name.
	public string GetTitle(int scene, int index)
	{
		scenes.TryGetValue(scene, out Infomation info);
		info.info.TryGetValue(index, out Data.Character data);

		return data.name;
	} 

	// Set their position on screen.
	public void SetPosition(int scene, int index)
	{
		scenes.TryGetValue(scene, out Infomation info);
		info.info.TryGetValue(index, out Data.Character character);

		switch (character.pos)
		{
			case "middle":
				{
					StopAllCoroutines();
					StartCoroutine(Move(Vector2.zero, 10));
					break;
				}
			case "left":
				{
					StopAllCoroutines();
					StartCoroutine(Move(Vector2.left, 10));
					break;
				}
			case "right":
				{
					StopAllCoroutines();
					StartCoroutine(Move(Vector2.right, 10));
					break;
				}
		}
	}

	// Move the character to that position.
	IEnumerator Move(Vector2 target, float speed)
	{
		float offset = 420;

		while (root.anchoredPosition != target)
		{
			root.anchoredPosition = Vector2.Lerp(root.anchoredPosition, target * offset, Time.deltaTime * speed);
			yield return new WaitForEndOfFrame();
		}
	}

	// Clear all character info.
	public void Clear()
	{
		scenes.Clear();
		scenes = new Dictionary<int, Infomation>();
	}
}
