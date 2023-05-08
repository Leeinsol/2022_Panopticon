using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMap : MonoBehaviour
{

    public GameObject[] PrisonRoom;
    public GameObject[] PrisonStair;
    public GameObject[] Tower;

    // Start is called before the first frame update
    void Start()
    {

        if (StageSetting.Instance.getStage() == "easyMode")
        {
            PrisonRoom[0].SetActive(false);
            PrisonRoom[1].SetActive(false);

            PrisonStair[0].SetActive(false);
            PrisonStair[1].SetActive(false);

            Tower[0].SetActive(false);
            Tower[1].SetActive(false);
        }
        else if (StageSetting.Instance.getStage() == "normalMode")
        {
            PrisonRoom[1].SetActive(false);
            PrisonStair[1].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
