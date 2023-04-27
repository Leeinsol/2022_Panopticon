using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Stage
{
    easyMode,
    normalMode,
    hardMode
};

public class StageSetting : MonoBehaviour
{
    Stage stage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStageEasy()
    {
        stage = Stage.easyMode;
        loadMain();
    }

    public void setStageNormal()
    {
        stage = Stage.normalMode;
        loadMain();

    }

    public void setStageHard()
    {
        stage = Stage.hardMode;
        loadMain();

    }

    void loadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public string getStage()
    {
        return stage.ToString();
    }

}
