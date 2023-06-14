using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public GameObject[] Button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StageButtonState();

    }
    void StageButtonState()
    {
        if (PlayerPrefs.GetInt("Stage") == 0)
        {
            Button[0].GetComponent<Button>().interactable = false;
            Button[1].GetComponent<Button>().interactable = false;
            Button[2].GetComponent<Button>().interactable = false;
        }
        else if (PlayerPrefs.GetInt("Stage") == 1)
        {
            Button[1].GetComponent<Button>().interactable = false;
            Button[2].GetComponent<Button>().interactable = false;
        }
        else if (PlayerPrefs.GetInt("Stage") == 2)
        {
            Button[2].GetComponent<Button>().interactable = false;
        }
    }
}
