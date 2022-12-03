using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonDoor : MonoBehaviour
{
    public float goalRotation = -75f;
    public float rotationSpeed= 1f;

    //bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        //isOpen=gameObject.Find("Camera").GetComponent<cinema_moving>().
        //isOpen = transform.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen;
        //isOpen = GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen)
        {
            Quaternion rot = Quaternion.Euler(0, goalRotation, 0);
            //Debug.Log("Open door");
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * rotationSpeed);

        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    isOpen = true;
        //    //transform.Rotate(transform.rotation.x, -goalRotation, transform.rotation.z);
        //    //transform.Rotate(0, -goalRotation, 0);
        //}

        //if (isOpen)
        //{


        //}
    }
}
