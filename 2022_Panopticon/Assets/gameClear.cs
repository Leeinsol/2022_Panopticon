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
            Debug.Log("3");
            return;
        }
        else if (PlayerPrefs.GetInt("remainTower") / 10000f >= 0.5f)
        {
            Debug.Log("2");
            threeStar.SetActive(false);
        }
        else
        {
            Debug.Log("1");
            twoStar.SetActive(false);
            threeStar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
