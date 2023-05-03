using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Speaker { Rico = 0, DoctorKO }

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[] dialogs;
	[SerializeField]
	private	Image[]	 imageDialogs;
	[SerializeField]
	private	TextMeshProUGUI[] textNames; 
	[SerializeField]
	private	TextMeshProUGUI[] textDialogues; 
	[SerializeField]
	private	GameObject[] objectArrows; 
	[SerializeField]
	private	float typingSpeed; 
	[SerializeField]
	private	KeyCode keyCodeSkip = KeyCode.Space; 

	private	int currentIndex = -1;
	private	bool isTypingEffect = false; 
	private	Speaker	currentSpeaker = Speaker.Rico;

	public void Setup()
	{
		for ( int i = 0; i < 2; ++ i )
		{ 
			InActiveObjects(i);
		}

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
				textDialogues[(int)currentSpeaker].text = dialogs[currentIndex].dialogue;
  
				objectArrows[(int)currentSpeaker].SetActive(true);

				return false;
			}
			 
			if ( dialogs.Length > currentIndex + 1 )
			{
				SetNextDialog();
			} 
			else
			{ 
				for ( int i = 0; i < 2; ++ i )
				{ 
					InActiveObjects(i);
				}

				return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{ 
		InActiveObjects((int)currentSpeaker);

		currentIndex ++;
		 
		currentSpeaker = dialogs[currentIndex].speaker;
 
		imageDialogs[(int)currentSpeaker].gameObject.SetActive(true);

		 
		textNames[(int)currentSpeaker].gameObject.SetActive(true);
		textNames[(int)currentSpeaker].text = dialogs[currentIndex].speaker.ToString();

		 
		textDialogues[(int)currentSpeaker].gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));
	}

	private void InActiveObjects(int index)
	{
		imageDialogs[index].gameObject.SetActive(false);
		textNames[index].gameObject.SetActive(false);
		textDialogues[index].gameObject.SetActive(false);
		objectArrows[index].SetActive(false);
	}

	private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;
		 
		while ( index < dialogs[currentIndex].dialogue.Length )
		{
			textDialogues[(int)currentSpeaker].text = dialogs[currentIndex].dialogue.Substring(0, index);

			index ++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;
		 
		objectArrows[(int)currentSpeaker].SetActive(true);
	}
}

[System.Serializable]
public struct Dialog
{
	public	Speaker speaker; 
	[TextArea(3, 5)]
	public	string dialogue; 
}

