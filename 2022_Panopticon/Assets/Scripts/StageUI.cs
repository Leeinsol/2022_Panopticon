using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    public AudioSource BGMSource;
    // Start is called before the first frame update
    void Start()
    {
        //BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageEasy()
    {
        StageSetting.Instance.setStageEasy();
    }

    public void StageNormal()
    {
        StageSetting.Instance.setStageNormal();

    }

    public void StageHard()
    {
        StageSetting.Instance.setStageHard();
    }

    public void loadTutorial()
    {
        StageSetting.Instance.BGMSource = BGMSource;
        SceneManager.LoadScene("Tutorial");
    }
}
