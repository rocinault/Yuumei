using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	private FileData file;
	[HideInInspector] public Data data;

	private bool bIsVisualNovel = false;
	private bool bIsMainMenu = true;
	private bool bEnd = false;

	private string playerName;

	private void Awake()
	{
		if (instance == null)
		{
			file = gameObject.GetComponent<FileData>();
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this);
		}
	}

	// Track when a scene is loaded.
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	// Track when a scene is no longer needed.
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	// Functions to occur immediately after a scene is loaded.
	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (bIsVisualNovel && !bEnd)
		{
			VisualNovelController.instance.Clear();
			LoadVisualNovel(data.chapter + 1);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			bIsMainMenu = false;
		}
		else if (!bIsMainMenu && !bEnd)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	// Change between the various scenes.
	public void ChangeScene()
	{
		if (!bIsVisualNovel)
		{
			bIsVisualNovel = true;
			SceneManager.LoadScene(1);
		}
		else if (!bEnd)
		{
			bIsVisualNovel = false;
			SceneManager.LoadScene(2);
		}
		else
		{
			bEnd = true;
			SceneManager.LoadScene(3);
		}
	}

	// Load the data from the script json file.
	private void LoadVisualNovel(int chapter)
	{
		GetComponentInChildren<Character>().Clear();

		file.Load(chapter);
		data = file.GetData();

		VisualNovelController.instance.Initialise(file.GetAllScenesForChapter(), file.GetSceneObjects(), playerName);
	}

	// Set the player name from the main menu input.
	public void SetPlayerName(string name)
	{
		playerName = name;
	}

	// Get the current player name.
	public string GetPlayerName()
	{
		return playerName;
	}

	// Allow the main menu to be loaded in.
	public void EnableLoadMainMenu()
	{
		bEnd = true;
	}
}

