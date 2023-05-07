using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[] dialogs;
	[SerializeField]
	private Image imgaeDialog;
	[SerializeField]
	private	TextMeshProUGUI textDialogues; 
	[SerializeField]
	private	GameObject objectArrows; 
	[SerializeField]
	private	float typingSpeed; 
	[SerializeField]
	private	KeyCode keyCodeSkip = KeyCode.Return; 

	private	int currentIndex = -1;
	private	bool isTypingEffect = false;

	public void Setup()
	{
		InActiveObjects();

		SetNextDialog();
	}

	public bool UpdateDialog()
	{
		if ( Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0) )
		{ 
			if ( isTypingEffect == true )
			{ 
				StopCoroutine("TypingText");
				isTypingEffect = false;
				textDialogues.text = dialogs[currentIndex].dialogue;
				objectArrows.SetActive(true);

				return false;
			}
			 
			if ( dialogs.Length > currentIndex + 1 )
			{
				SetNextDialog();
			} 
			else
			{
				InActiveObjects();

				return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{ 
		InActiveObjects();

		currentIndex ++;

		imgaeDialog.gameObject.SetActive(true);

		textDialogues.gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));
	}

	private void InActiveObjects()
	{
		imgaeDialog.gameObject.SetActive(false);
		textDialogues.gameObject.SetActive(false);
		objectArrows.SetActive(false);
	}

	private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;
		 
		while ( index < dialogs[currentIndex].dialogue.Length )
		{
			textDialogues.text = dialogs[currentIndex].dialogue.Substring(0, index);

			index ++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;
		 
		objectArrows.SetActive(true);
	}
}

[System.Serializable]
public struct Dialog
{
	[TextArea(3, 5)]
	public	string dialogue; 
}

