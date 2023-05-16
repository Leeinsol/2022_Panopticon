using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class title : MonoBehaviour
{
    public GameObject[] StageButton;

    public GameObject StagePanel;
    public GameObject SettingPanel;
    public GameObject QuitPanel;

    public AudioClip titleMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (StageSetting.Instance.BGMSource.clip != titleMusic)
        {
            StageSetting.Instance.BGMSource.clip = titleMusic;
            StageSetting.Instance.BGMSource.Play();

        }


        StageButtonState();

        


        StagePanel.SetActive(false);

    }
    void StageButtonState()
    {
        if (PlayerPrefs.GetInt("Stage") == 0)
        {
            StageButton[0].GetComponent<Button>().interactable = false;
            StageButton[1].GetComponent<Button>().interactable = false;
            StageButton[2].GetComponent<Button>().interactable = false;
        }
        else if (PlayerPrefs.GetInt("Stage") == 1)
        {
            StageButton[1].GetComponent<Button>().interactable = false;
            StageButton[2].GetComponent<Button>().interactable = false;
        }
        else if (PlayerPrefs.GetInt("Stage") == 2)
        {
            StageButton[2].GetComponent<Button>().interactable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (QuitPanel.activeSelf)
            {
                setQuitPanel(false);

            }
            else
            {
                setQuitPanel(true);

            }
        }
    }
    

    public void gameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void closeStagePanel()
    {
        StagePanel.SetActive(false);
    }
    public void closeSettingPanel()
    {
        SettingPanel.SetActive(false);
    }


    public void setStage()
    {
        StagePanel.SetActive(true);
    } 
    public void setting()
    {
        SettingPanel.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Reset()
    {
        StageSetting.Instance.ResetStar();
    }

    public void setQuitPanel(bool state)
    {
        QuitPanel.SetActive(state);
    }
}
