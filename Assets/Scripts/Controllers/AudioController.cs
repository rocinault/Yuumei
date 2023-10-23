using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StopAllCoroutines();
		StartCoroutine(FadeIn());
	}

	// Make the volume slowly move to 0.
	public void Quieter()
	{
		StopAllCoroutines();
		StartCoroutine(FadeOut());
	}

	// Make the volume slowly move to 0.7.
	public void Louder()
	{
		StopAllCoroutines();
		StartCoroutine(FadeIn());
	}

	// Fade out the audio.
	private IEnumerator FadeOut()
	{
		float vol = source.volume;

		while (source.volume > 0)
		{
			source.volume -= vol * Time.deltaTime / 3.0f;
			yield return null;
		}

		source.Stop();
	}

	// Fade in the audio.
	private IEnumerator FadeIn()
	{
		source.Play();

		source.volume = 0;

		while (source.volume < 0.7)
		{
			source.volume += Time.deltaTime / 3.0f;
			yield return null;
		}
	}
}
