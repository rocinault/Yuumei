using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelController : MonoBehaviour
{
	public static VisualNovelController instance;

	public GameObject background;
	public GameObject foreground;

	public GameObject icon;

	[HideInInspector] public bool bWaitForInput = false;
	[HideInInspector] public bool bFadeAudio = false;

	private Dictionary<int, Data.Scene> scenes = new Dictionary<int, Data.Scene>();
	private Dictionary<int, GameObject> sceneObjects = new Dictionary<int, GameObject>();

	private TextController controller;
	private Dialogue dialog;

	private bool bSceneFinished = true;

	private int scene = 1;
	private int index = 1;

	private void Awake()
	{
		if (instance == null)
		{
			controller = gameObject.GetComponent<TextController>();
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this);
		}
	}

	// Pass in the details for the current chapter.
	public void Initialise(Dictionary<int, Data.Scene> scenes, Dictionary<int, GameObject> sceneObjects, string name)
	{
		this.sceneObjects = sceneObjects;
		this.scenes = scenes;

		PlayerName(name);
		FadeOutAllSceneObjects();
		Display();

		StopAllCoroutines();
		StartCoroutine(Delay(2));
	}

	// Read for user input.
	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !bWaitForInput)
		{
			StopAllCoroutines();
			StartCoroutine(Delay(0.15f));
		}
	}

	// Set a slight delay before text appears (used for delay between transitions).
	private IEnumerator Delay(float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);

		if (dialog != null)
		{
			Next();
		}
	}

	// Get the next item to be shown on screen.
	private void Next()
	{
		if (dialog.IsFinished())
		{
			index++;

			if (Display())
			{
				DisplayNewSceneWithDelay();
				return;
			}
		}

		if (dialog != null)
		{
			controller.Next(dialog);
		}
	}

	//Display the next item in the novel
	private bool Display()
	{
		bSceneFinished = true;

		foreach (GameObject prefab in sceneObjects.Values)
		{
			if ((prefab.tag == "Player" || prefab.tag == "Character") && ActiveCharacter(prefab, prefab.GetComponent<Character>()))
			{
				bSceneFinished = false;
			}
			else if (prefab.tag == "UI" && ActiveUI(prefab, prefab.GetComponent<UI>()))
			{
				dialog = null;
				bSceneFinished = false;
			}
		}

		return bSceneFinished;
	}

	// Check if the current index points to an active character.
	private bool ActiveCharacter(GameObject prefab, Character character)
	{
		if (character.scenes.TryGetValue(scene, out Character.Infomation info))
		{
			if (info.info.TryGetValue(index, out Data.Character data))
			{
				InitialiseCharacter(prefab);

				if (prefab.tag != "Player")
				{
					FadeOutMultiple(prefab.GetComponent<Image>(), prefab.GetComponentsInChildren<Image>(), 0.001f);
					FadeInMultiple(prefab.GetComponent<Image>(), prefab.GetComponentsInChildren<Image>(), 0.5f);
				}

				return true;
			}
		}

		return false;
	}

	// If true, initialise that character and display their current dialogue on screen.
	private void InitialiseCharacter(GameObject prefab)
	{
		prefab.SetActive(true);
		prefab.GetComponent<Character>().SetPosition(scene, index);
		dialog = new Dialogue(prefab.GetComponent<Character>(), scene, index);
	}

	// Check if the current index points to an active UI element.
	private bool ActiveUI(GameObject prefab, UI ui)
	{
		if (ui.scenes.TryGetValue(scene, out UI.Infomation info))
		{
			if (info.info.ContainsKey(index))
			{
				prefab.SetActive(true);
				bWaitForInput = true;
				return true;
			}
		}

		return false;
	}

	// Display the next scene with a delay.
	private void DisplayNewSceneWithDelay()
	{
		NewScene();
		Display();

		controller.Clear();

		scenes.TryGetValue(scene-1, out Data.Scene s);
		if (s.background != null)
		{
			StopAllCoroutines();
			StartCoroutine(Delay(4.0f));
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(Delay(0.15f));
		}
	}

	// Create a new scene.
	private void NewScene()
	{
		GetNextScene();
		FadeOutAllSceneObjects();

		bSceneFinished = false;
	}

	// Get the next scene, if the user isn't progressing linerly or presented with options.
	private void GetNextScene()
	{
		scenes.TryGetValue(scene, out Data.Scene s);
		scene = s.transition;

		if (scenes.Count < scene)
		{
			if (scene >= 100)
			{
				GameManager.instance.EnableLoadMainMenu();
			}

			foreground.GetComponent<PlayAnimation>().SetTransition(true);
			foreground.GetComponent<PlayAnimation>().Play();

			dialog = null;
		}
		else
		{
			index = 1;
			ChangeBackground(s);
			FadeOutAudio();
		}
	}

	// Fade out all the scene objects on screen.
	private void FadeOutAllSceneObjects()
	{
		foreach (GameObject prefab in sceneObjects.Values)
		{
			if (prefab.tag == "UI")
			{
				prefab.SetActive(false);
			}
			else if (prefab.tag != "Player" && prefab.activeSelf)
			{
				FadeOutMultiple(prefab.GetComponent<Image>(), prefab.GetComponentsInChildren<Image>(), 0.4f);
			}
		}
	}

	// Change the current background.
	private void ChangeBackground(Data.Scene s)
	{
		if (s.background != null)
		{
			foreground.GetComponent<PlayAnimation>().SetTransition(false);
			foreground.GetComponent<PlayAnimation>().Play();

			background.GetComponent<Animator>().SetTrigger("AnyStateFadeIn");
		}
	}

	// fade out the background audio.
	private void FadeOutAudio()
	{
		if (GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip != null && bFadeAudio)
		{
			GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>().Quieter();
			bFadeAudio = false;
		}
	}

	// Set the current scene manually (used when promoted by buttons).
	public void SetScene(int value)
	{
		scene = value;
		bWaitForInput = false;
		index = 1;

		FadeOutAllSceneObjects();
		Display();

		Next();
	}

	// set the current background manually.
	public void SetBackground()
	{
		scenes.TryGetValue(scene-1, out Data.Scene s);
		background.GetComponent<RawImage>().texture = Resources.Load<Texture>("Backgrounds/" + s.background);
	}

	// fade out the characters.
	private void FadeOutMultiple(Image image, Image[] images, float speed)
	{
		image.CrossFadeAlpha(0.0f, speed, false);

		foreach (Image i in images)
		{
			i.CrossFadeAlpha(0.0f, speed * 0.5f, false);
		}
	}

	// Fade in the characters.
	private void FadeInMultiple(Image image, Image[] images, float speed)
	{
		image.CrossFadeAlpha(1.0f, speed, false);

		foreach (Image i in images)
		{
			i.CrossFadeAlpha(1.0f, speed * 0.5f, false);
		}
	}

	// Set the player name in the novel itself.
	private void PlayerName(string name)
	{
		sceneObjects.TryGetValue(1, out GameObject value);
		value.GetComponent<Character>().title = name;
	}

	// Clear the current data.
	public void Clear()
	{
		sceneObjects.Clear();
		scenes.Clear();
	}

	// Get the current index in a scene.
	public int GetIndex()
	{
		return index;
	}

	// Get the current scene in a chapter.
	public int GetScene()
	{
		return scene;
	}

	// Display an icon on screen.
	public void Icon(string image)
	{
		icon.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/" + image);
		icon.GetComponent<Icon>().Display();
	}
}
