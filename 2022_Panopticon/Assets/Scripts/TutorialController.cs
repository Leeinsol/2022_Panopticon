using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;
    [SerializeField]
    private string nextSceneName = "";

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        currentTutorial.Enter();
    }

    void CompletedAllTutorials()
    {
        currentTutorial = null;

        Debug.Log("Complete All");

        if (!nextSceneName.Equals(""))
        {
            PlayerPrefs.SetInt("Stage", 1);
            if (PlayerPrefs.GetInt("isCinemaEnd") == 0)
            {
                StageSetting.Instance.setStageHard();
                SceneManager.LoadScene(nextSceneName);

            }
            else
                SceneManager.LoadScene(nextSceneName);
        }
    }
}