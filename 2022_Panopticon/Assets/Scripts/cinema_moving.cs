using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cinema_moving : MonoBehaviour
{

    public float goalRotation = -30f;
    public float Speed = 1f;

    public int[] cinemaNums;
    float time = 3f;

    float maximum = 60.0F;
    private float startTime;

    int cinemaNum = 0;
    int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        cinemaNum = cinemaNums[0];
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isRotateleft)
        //    moveRotateLeft();

        //if (isRotateleft)
        //{
        //    moveRotateRight();

        //}

        //if(isRotateleft && isRotateRight)
        //{
        //    moveRotateMiddle();

        //}
        //if (isRotateleft && isRotateRight && isRotateMiddle)
        //{
        //    zoomCamera();

        //}


        if (cinemaNum == 0)
            moveRotateLeft();

        if (cinemaNum == 1)
        {
            moveRotateRight();

        }

        if (cinemaNum == 2)
        {
            moveRotateMiddle();

        }
        if (cinemaNum == 3)
        {
            zoomCamera();

        }


        //if (cinemaNum == 1)
        //    moveRotateLeft();

        //if (cinemaNum == 2)
        //{
        //    moveRotateRight();

        //}

        //if (cinemaNum == 3)
        //{
        //    moveRotateMiddle();

        //}
        //if (cinemaNum == 4)
        //{
        //    zoomCamera();

        //}


        //else if (isRotateleft)
        //{
        //    moveRotateRight();
        //    if (isRotateRight)
        //    {
        //        moveRotateMiddle();
        //        if (isRotateMiddle)
        //        {
        //            zoomCamera();

        //        }
        //    }
        //}



        //float FOVvalue = Mathf.SmoothStep(0, 10, Time.deltaTime);
        //Debug.Log(FOVvalue);

        //zoomCamera();
    }

    void moveRotateLeft()
    {
        //Debug.Log("moveRotateLeft");
        Quaternion rot = Quaternion.Euler(0, goalRotation, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);

        if (transform.localRotation == rot)
        {
            indexCheck();
        }

    }
    void moveRotateRight()
    {
        //Debug.Log("moveRotateRight");
        Quaternion rot = Quaternion.Euler(0, -goalRotation, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);
        if (transform.localRotation == rot)
        {
            indexCheck();
        }
    }

    void moveRotateMiddle()
    {
        //Debug.Log("moveRotateMiddle");
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);

        if (transform.localRotation == rot)
        {
            indexCheck();
        }
    }

    void zoomCamera()
    {
        //Debug.Log("zoomCamera");
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        transform.localRotation = rot;
        //float FOVvalue = Mathf.Lerp(60, 10, Time.deltaTime * 50);
        //float FOVvalue = Mathf.SmoothStep(0, 10, Time.deltaTime * 100.0f);
        //Debug.Log(FOVvalue);
        //transform.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60, 10, Time.deltaTime * 100.0f);
        //float FOVtime = (Time.time - startTime) / time;
        
        time = time - Time.deltaTime;
        Debug.Log(time);
        transform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(10, 60, time*0.33f);


        Vector3 destination = new Vector3(transform.position.x, 19.4f, transform.position.z);
        transform.localPosition = Vector3.MoveTowards(transform.position, destination, 0.001f);

        //Debug.Log(transform.localPosition.y);
        //Debug.Log(transform.GetComponent<Camera>().fieldOfView);
        if (transform.GetComponent<Camera>().fieldOfView == 10)
        {
            indexCheck();
        }
    }
    void indexCheck()
    {
        if (index >= cinemaNums.Length - 1)
        {
            Invoke("loadTitle", 1.5f);
            //ShowMainCamera();
            return;
        }
        index++;
        cinemaNum = cinemaNums[index];

    }

    void loadTitle()
    {
        PlayerPrefs.SetInt("isCinemaEnd", 1);
        SceneManager.LoadScene("Title");
    }
}
