using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_pacnopticon : MonoBehaviour
{
    [SerializeField] private Camera theCamera;
    float defaultFOV = 60f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveFloor();

        //if(!transform.GetComponent<player_Controller>().isZooming)
        //    heightFOV();
    }

    void MoveFloor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            //SetCameraFOV(60f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = new Vector3(transform.position.x, 9f, transform.position.z);
            //SetCameraFOV(65f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.position = new Vector3(transform.position.x, 16.5f, transform.position.z);
            //SetCameraFOV(70f);
        }
    }
    void heightFOV()
    {
        theCamera.fieldOfView = defaultFOV + transform.position.y;
    }
}
