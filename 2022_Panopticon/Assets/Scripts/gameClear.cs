using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameClear : MonoBehaviour
{
    public GameObject oneStar;
    public GameObject twoStar;
    public GameObject threeStar;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("remainTower"));
        

        if (PlayerPrefs.GetInt("remainTower")/10000f >= 0.8f)
        {
            StageSetting.Instance.SetStar(StageSetting.Instance.getStage(), 3);
            return;
        }
        else if (PlayerPrefs.GetInt("remainTower") / 10000f >= 0.4f)
        {
            StageSetting.Instance.SetStar(StageSetting.Instance.getStage(), 2);
            threeStar.SetActive(false);
        }
        else
        {
            StageSetting.Instance.SetStar(StageSetting.Instance.getStage(), 1);
            twoStar.SetActive(false);
            threeStar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
