using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMap : MonoBehaviour
{

    public GameObject[] PrisonRoom;
    public GameObject[] PrisonStair;
    public GameObject[] Tower;

    public SharedData sharedData;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(sharedData.stage);
        if (sharedData.stage == "easy")
        {
            PrisonRoom[0].SetActive(false);
            PrisonRoom[1].SetActive(false);

            PrisonStair[0].SetActive(false);
            PrisonStair[1].SetActive(false);

            Tower[0].SetActive(false);
            Tower[1].SetActive(false);
        }
        else if (sharedData.stage == "normal")
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
