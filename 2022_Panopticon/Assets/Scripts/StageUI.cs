using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class StageUI : MonoBehaviour
{
    public AudioSource BGMSource;
    public SharedData sharedData;

    // Start is called before the first frame update
    void Start()
    {
        //BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectStage(string stage)
    {
        Debug.Log(stage);
        sharedData.stage = stage;
        SceneManager.LoadScene("Main");
    }

    
    public void loadTutorial()
    {
        StageSetting.Instance.BGMSource = BGMSource;
        SceneManager.LoadScene("Tutorial");
    }
}
