using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinema_controller : MonoBehaviour
{
    public Camera MainCamera;
    public Camera CinemaCamera;
    public GameObject playerCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("isCinemaEnd")==1)
        {
            ShowMainCamera();
        }
        else
        {
            playerCanvas.SetActive(false);
            ShowCinemaCamera();
        }
    }

    public void ShowMainCamera()
    {
        MainCamera.enabled = true;
        CinemaCamera.enabled = false;
        //CinemaCamera.GetComponent<cinema_moving>().enabled = false;
    }

    public void ShowCinemaCamera()
    {
        CinemaCamera.enabled = true;
        MainCamera.enabled = false;
    }

}
