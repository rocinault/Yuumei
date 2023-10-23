using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
	public struct Infomation
	{
		public Dictionary<int, Data.UI> info;

		public Infomation(Dictionary<int, Data.UI> info)
		{ this.info = info; }
	}

	[HideInInspector] public Dictionary<int, Infomation> scenes = new Dictionary<int, Infomation>();

	private Dictionary<int, Data.UI> data = new Dictionary<int, Data.UI>();

	// Store the data for the UI object.
	public void Initialise(int index, Data.UI ui)
	{
		data.Add(index, ui);

		if (ui.text.Length > 0)
		{
			gameObject.GetComponentInChildren<TextMeshProUGUI>().text = ui.text[0];
		}
	}

	// Add in which scene they appear in.
	public void AddScene(int scene)
	{
		if (!scenes.ContainsKey(scene))
		{
			Infomation information = new Infomation(data);
			scenes.Add(scene, information);

			data = new Dictionary<int, Data.UI>();
		}
	}

	// When prompted by selection, change the current scene and index in the visual novel.
	public void TaskOnClick()
	{
		scenes.TryGetValue(VisualNovelController.instance.GetScene(), out Infomation info);
		info.info.TryGetValue(VisualNovelController.instance.GetIndex(), out Data.UI ui);

		VisualNovelController.instance.SetScene(ui.transition);
	}
}
