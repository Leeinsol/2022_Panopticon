using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeEffect : MonoBehaviour
{
	[System.Serializable]
	private class FadeEvent : UnityEvent { }
	private FadeEvent onFadeEvent = new FadeEvent();

	[SerializeField]
	[Range(0.01f, 10f)]
	private float fadeTime;
	[SerializeField]
	private AnimationCurve fadeCurve;
	private Image fadeImage;

	private void Awake()
	{
		fadeImage = GetComponent<Image>();
	}

	public void FadeIn(UnityAction action)
	{
		StartCoroutine(Fade(action, 1, 0));
	}

	public void FadeOut(UnityAction action)
	{
		StartCoroutine(Fade(action, 0, 1));
	}

	private IEnumerator Fade(UnityAction action, float start, float end)
	{
		onFadeEvent.AddListener(action);

		float current = 0.0f;
		float percent = 0.0f;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / fadeTime;

			Color color = fadeImage.color;
			color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
			fadeImage.color = color;

			yield return null;
		}

		onFadeEvent.Invoke();

		onFadeEvent.RemoveListener(action);
	}
}
