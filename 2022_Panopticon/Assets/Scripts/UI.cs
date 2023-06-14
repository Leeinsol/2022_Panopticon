using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public AudioClip ClickSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameStart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene("Main");

    }
    public void LoadTitle()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }


    public void clickSound()
    {
        StageSetting.Instance.SfxSource.clip = ClickSound;
        StageSetting.Instance.SfxSource.Play();
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

}

