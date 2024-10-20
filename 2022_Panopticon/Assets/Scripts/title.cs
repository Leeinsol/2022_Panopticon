using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class title : MonoBehaviour
{

    public GameObject StagePanel;
    public GameObject SettingPanel;
    public GameObject QuitPanel;
    public GameObject ResetPanel;
    public GameObject AllresetButton;
    public GameObject ResetButton;
    public GameObject InputButton;

    public AudioClip titleMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        if (StageSetting.Instance.BGMSource.clip != titleMusic)
        {
            StageSetting.Instance.BGMSource.clip = titleMusic;
            StageSetting.Instance.BGMSource.Play();

        }
        if(StagePanel!=null)        closeStagePanel();
        if(SettingPanel!=null)      closeSettingPanel();
        if(QuitPanel!=null)         setQuitPanel(false);
        if (ResetPanel != null)     setResetPanel(false);
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
        SettingPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        SettingPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        SettingPanel.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);
    }

    public void nextPage()
    {
        SettingPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        SettingPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
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


    public void setQuitPanel(bool state)
    {
        QuitPanel.SetActive(state);
    }
    public void setResetPanel(bool state)
    {
        ResetPanel.SetActive(state);
    }

    public void setResetButton(GameObject button)
    {
        offAllResetButton();
        button.SetActive(true);        
    }
    public void setResetButton()
    {
        offAllResetButton();

        ResetButton.SetActive(true);
    }

    void offAllResetButton()
    {
        AllresetButton.SetActive(false);
        ResetButton.SetActive(false);
        InputButton.SetActive(false);
    }

    public void resetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
