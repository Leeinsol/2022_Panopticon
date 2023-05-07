using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowBlink : MonoBehaviour
{
	[SerializeField]
	private	float fadeTime; 
	private	TextMeshProUGUI fadeImage; 

	private void Awake()
	{
		fadeImage = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{ 
		StartCoroutine(FadeInOut());
	}

	private IEnumerator FadeInOut()
	{
		while ( true )
		{
			yield return StartCoroutine(Fade(1, 0)); 

			yield return StartCoroutine(Fade(0, 1)); 
		}
	}

	private IEnumerator Fade(float start, float end)
	{
		float current = 0;
		float percent = 0;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / fadeTime;

			Color color = fadeImage.color;
			color.a	= Mathf.Lerp(start, end, percent);
			fadeImage.color	= color;

			yield return null;
		}
	}
}

