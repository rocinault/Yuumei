using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class FileData : MonoBehaviour
{
	private string filename = "Chapter 1.json";
	private string path;

	private Data data;

	private Dictionary<int, Data.Scene> scenes;
	private Dictionary<int, GameObject> sceneObjects;

	private void Awake()
	{
		data = new Data();
	}

	// Load in a current chapter based off the number passed in.
	public void Load(int chapter)
	{
		filename = "Chapter " + chapter + ".json";
		path = Application.streamingAssetsPath + "/" + filename;

		scenes = new Dictionary<int, Data.Scene>();
		sceneObjects = new Dictionary<int, GameObject>();

		Parse(chapter);
	}

	// Parse all the information in the chapter, storing them as references in sceneObjects dictionary.
	public void Parse(int chapter)
	{
		try
		{
			if (File.Exists(path))
			{
				string contents = File.ReadAllText(path);
				data = JsonUtility.FromJson<Data>(contents);

				GameManager.instance.data.chapter = data.chapter;
				sceneObjects.Add(1, GameObject.FindGameObjectWithTag("Player"));

				foreach (Data.Scene scene in data.scenes)
				{
					foreach (Data.Character character in scene.characters)
					{
						LoadCharacter(character);
					}

					foreach (Data.UI ui in scene.ui)
					{
						LoadUI(ui);
					}

					AddScene(scene);
				}
			}
			else
			{
				Debug.Log("Unable to read the data, file does not exist");
				data = new Data();
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	// Load a character and set its information in the character struct.
	private void LoadCharacter(Data.Character character)
	{
		GameObject instance = null;

		if (character.tag == "Character" && !sceneObjects.TryGetValue(character.id, out instance))
		{
			instance = Instantiate(Resources.Load<GameObject>(character.tag + "/" + character.name), GameObject.FindGameObjectWithTag(character.tag).transform);
			instance.SetActive(false);

			Initialise(instance, character);

			sceneObjects.Add(character.id, instance);
		}
		else if (character.tag == "Player" && sceneObjects.TryGetValue(character.id, out instance))
		{
			Initialise(instance, character);
		}
		else
		{
			Initialise(instance, character);
		}
	}

	// Set a characters information in the character class struct.
	private void Initialise(GameObject prefab, Data.Character character)
	{
		prefab.GetComponent<Character>().Initialise(character.index, character);
	}

	// Load in UI element.
	private void LoadUI(Data.UI ui)
	{
		GameObject instance = null;

		if (!sceneObjects.TryGetValue(ui.id, out instance))
		{
			instance = Instantiate(Resources.Load<GameObject>(ui.tag + "/" + ui.name), GameObject.FindGameObjectWithTag(ui.tag).transform);
			instance.GetComponent<UI>().Initialise(ui.index, ui);

			instance.SetActive(false);

			sceneObjects.Add(ui.id, instance);
		}
	}

	// Set the data in the UI class.
	private void InitialiseUI(GameObject prefab, Data.UI ui)
	{
		prefab.GetComponent<UI>().Initialise(ui.index, ui);
	}

	// Add a new scene from the file.
	private void AddScene(Data.Scene scene)
	{
		foreach (Data.Character character in scene.characters)
		{
			sceneObjects.TryGetValue(character.id, out GameObject instance);
			instance.GetComponent<Character>().AddScene(scene.index);
		}

		foreach (Data.UI ui in scene.ui)
		{
			sceneObjects.TryGetValue(ui.id, out GameObject instance);
			instance.GetComponent<UI>().AddScene(scene.index);
		}

		scenes.Add(scene.index, scene);
	}

	// return all the scene objects loaded.
	public Dictionary<int, GameObject> GetSceneObjects()
	{
		return sceneObjects;
	}

	// return the total number of scenes for a chapter.
	public Dictionary<int, Data.Scene> GetAllScenesForChapter()
	{
		return scenes;
	}

	// Get the current data.
	public Data GetData()
	{
		return data;
	}

	// Save helper function.
	public void Save()
	{
		Debug.Log("Save data");

		string contents = JsonUtility.ToJson(data, true);
		File.WriteAllText(path, contents);
	}
}
