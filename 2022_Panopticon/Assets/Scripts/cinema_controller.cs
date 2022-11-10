using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinema_controller : MonoBehaviour
{
    public Camera MainCamera;
    public Camera CinemaCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            ShowMainCamera();
        }    
        if (Input.GetKey("2"))
        {
            ShowCinemaCamera();
        }    
    }

    public void ShowMainCamera()
    {
        MainCamera.enabled = true;
        CinemaCamera.enabled = false;
    }

    public void ShowCinemaCamera()
    {
        CinemaCamera.enabled = true;
        MainCamera.enabled = false;
    }

}
