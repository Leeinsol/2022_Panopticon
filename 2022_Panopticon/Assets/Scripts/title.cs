using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{

    public GameObject StagePanel;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        StagePanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void gameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void closeStagePanel()
    {
        StagePanel.SetActive(false);
    }


    public void setStage()
    {
        StagePanel.SetActive(true);
    } 
}
