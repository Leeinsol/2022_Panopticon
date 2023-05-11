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
    private static StageSetting instance = null;

    [SerializeField]
    Stage stage = Stage.easyMode;

    public AudioSource BGMSource;

    private void Awake()
    {
        //PlayerPrefs.SetInt("Stage", 1);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("easyStar", 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static StageSetting Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public string getStage()
    {
        return stage.ToString();
    }

    public void setStageEasy()
    {
        stage = Stage.easyMode;
    }

    public void setStageNormal()
    {
        stage = Stage.normalMode;

    }

    public void setStageHard()
    {
        stage = Stage.hardMode;
    }

    public void SetStar(string mode, int currentStar)
    {

        if (mode == "easyMode")
        {
            SetEasyStar(currentStar);
            Debug.Log(PlayerPrefs.GetInt("easyStar"));

        }
        else if (mode == "normalMode")
        {
            SetNormalStar(currentStar);
            Debug.Log(PlayerPrefs.GetInt("normalStar"));
        }
        else if (mode == "hardMode")
        {
            SetHardStar(currentStar);
            Debug.Log(PlayerPrefs.GetInt("hardStar"));
        }
    }
    void SetEasyStar(int currentStar)
    {
        if (PlayerPrefs.GetInt("easyStar") < currentStar)
            PlayerPrefs.SetInt("easyStar", currentStar);
    }

    void SetNormalStar(int currentStar)
    {
        if (PlayerPrefs.GetInt("normalStar") < currentStar)
            PlayerPrefs.SetInt("normalStar", currentStar);
    }

    void SetHardStar(int currentStar)
    {
        if (PlayerPrefs.GetInt("hardStar") < currentStar) 
            PlayerPrefs.SetInt("hardStar", currentStar);
    }
}
