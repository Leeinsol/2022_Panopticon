using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
    public float rotationSpeed = 200;

    float mouse_X;
    float mouse_Y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mouse_X += h + rotationSpeed + Time.deltaTime;
        mouse_Y += v + rotationSpeed + Time.deltaTime;

        mouse_Y = Mathf.Clamp(mouse_Y, -90, 90);
        transform.eulerAngles = new Vector3(-mouse_Y, mouse_X, 0);
    }


}
