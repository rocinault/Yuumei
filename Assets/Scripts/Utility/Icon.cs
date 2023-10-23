using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
	private Animator animator;
	private RawImage icon;


	private void Awake()
	{
		icon = GetComponent<RawImage>();
		animator = GetComponent<Animator>();
	}

	// Display the icon.
	public void Display()
	{
		animator.SetTrigger("Display");
	}

	// Hide the icon.
	public void Hide()
	{
		animator.SetTrigger("Hide");
	}
}
