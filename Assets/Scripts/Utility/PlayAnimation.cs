using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour
{
	private Animator animator;
	private RawImage image;

	private bool transition = true;

	private void Awake()
	{
		animator = gameObject.GetComponent<Animator>();
		image = gameObject.GetComponent<RawImage>();
	}

	// Trigger the fadin to play (automatically goes to fade out by anim event).
	public void Play()
	{
		animator.SetTrigger("FadeIn");
	}

	// Trigger swap animation.
	public void Swap()
	{
		animator.SetTrigger("Swap");
	}

	// Function for when fadin finished playing, check if scene needs to be changed.
	public void OnAnimationFinished()
	{
		animator.SetTrigger("FadeOut");

		if (transition)
		{
			GameManager.instance.ChangeScene();
		}	
	}

	// Set whether game manager loads another scene.
	public void SetTransition(bool value)
	{
		transition = value;
	}

	// Change the current background.
	public void ChangeBackground()
	{
		VisualNovelController.instance.SetBackground();
	}

	// Disable input during the scene fades.
	public void DisableInput()
	{
		VisualNovelController.instance.bWaitForInput = true;
	}

	// Enable input once the scene fading has finished.
	public void EnableInput()
	{
		VisualNovelController.instance.bWaitForInput = false;
	}
}
