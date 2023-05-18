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
    public AudioClip tutorialMusic;

    public GameObject SettingPanel;
    // Start is called before the first frame update
    void Start()
    {
        StageSetting.Instance.getAudioSource();
        if (StageSetting.Instance.BGMSource.clip != tutorialMusic)
        {
            StageSetting.Instance.BGMSource.clip = tutorialMusic;
            StageSetting.Instance.BGMSource.Play();

        }

        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingPanel.activeSelf)
            {
                Time.timeScale = 1;
                SettingPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;

            }
            else
            {
                StageSetting.Instance.SfxSource.Stop();

                Time.timeScale = 0;
                SettingPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void setSettingPanel()
    {
        Time.timeScale = 1;
        SettingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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