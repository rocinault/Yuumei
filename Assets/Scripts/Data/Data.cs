using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
	public int chapter;
	public List<Scene> scenes = new List<Scene>();

	// Data that is stored in each scene.
	[System.Serializable]
	public struct Scene
	{
		public int index;
		public int transition;

		public string background;

		public List<Character> characters;
		public List<UI> ui;

		public Scene(int index, int transition, string background, List<Character> characters, List<UI> ui)
		{ this.index = index; this.transition = transition; this.background = background; this.characters = characters; this.ui = ui; }
	}

	// Character data.
	[System.Serializable]
	public struct Character
	{
		public int id;

		public string tag;
		public string name;

		public int index;
		public string[] text;

		public string pos;

		public Character(int id, string tag, string name, int index, string[] text, string pos)
		{ this.id = id; this.tag = tag; this.name = name; this.index = index; this.text = text; this.pos = pos; }
	}

	// UI data.
	[System.Serializable]
	public struct UI
	{
		public int id;

		public string tag;
		public string name;

		public int index;
		public int transition;

		public string[] text;

		public UI(int id, string tag, string name, int index, int transition, string[] text)
		{ this.id = id; this.tag = tag; this.name = name; this.index = index; this.transition = transition; this.text = text; }
	}
}
