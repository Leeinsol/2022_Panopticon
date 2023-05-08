using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageEasy()
    {
        StageSetting.Instance.setStageEasy();
        loadMain();
    }

    public void StageNormal()
    {
        StageSetting.Instance.setStageNormal();
        loadMain();

    }

    public void StageHard()
    {
        StageSetting.Instance.setStageHard();

        loadMain();
    }

    public void loadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
