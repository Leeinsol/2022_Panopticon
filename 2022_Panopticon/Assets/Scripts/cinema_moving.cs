using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class cinema_moving : MonoBehaviour
{

    public float goalRotation = -30f;
    public float Speed = 1f;

    public float doorGoalRotation = -75f;
    public float doorRotationSpeed = 1f;

    public int[] cinemaNums;
    float timer15;
    float timer30;

    float Maxtimer15 = 1.5f;
    float Maxtimer30 = 3f;

    //float maximum = 60.0F;
    private float startTime;

    int cinemaNum = 0;
    int index = 0;

    public bool isDoorOpen = false;
    public GameObject canvas;
    public GameObject Sprintcanvas;

    // Start is called before the first frame update
    void Start()
    {
        SetCanvas(false);
        isDoorOpen = false;
        timer15 = Maxtimer15;
        timer30 = Maxtimer30;
        startTime = Time.time;
        cinemaNum = cinemaNums[0];
        if (PlayerPrefs.GetInt("isCinemaEnd") == 1)
        {
            SetCanvas(true);

            Array.Resize(ref cinemaNums, 0);
            //isDoorOpen = false;
        }
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

        if (PlayerPrefs.GetInt("isCinemaEnd") == 0)
        {
            if (cinemaNum == 0)
                moveRotateLeft();

            if (cinemaNum == 1)
                moveRotateRight();

            if (cinemaNum == 2)
                moveRotateMiddle();

            if (cinemaNum == 3)
                zoomCamera();

            if (cinemaNum == 4)
                setCameraPosition();

            if (cinemaNum == 5)
                doorOpen();
        }



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

    void SetCanvas(bool set)
    {
        canvas.SetActive(set);
        Sprintcanvas.SetActive(set);
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

        timer15 = timer15 - Time.deltaTime;
        transform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(10, 60, timer15 * 0.33f);


        Vector3 destination = new Vector3(transform.position.x, 19.9f, transform.position.z);
        //Debug.Log(destination);
        transform.localPosition = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 0.4f);
        //transform.position= Vector3.MoveTowards(transform.position, new Vector3(0,19.7f,0), 0.4f);
        //transform.localPosition =destination;

        //Debug.Log(transform.localPosition.y);
        //Debug.Log(transform.GetComponent<Camera>().fieldOfView);
        if (transform.GetComponent<Camera>().fieldOfView == 10)
        {
            timer30 = timer30 - Time.deltaTime;

            if (timer30 < 0)
            {
                timer15 = Maxtimer15;
                timer30 = Maxtimer30;
                indexCheck();
            }
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
        StageSetting.Instance.setStageEasy();
        SceneManager.LoadScene("Main");
    }

    void setCameraPosition()
    {
        transform.position = new Vector3(transform.position.x, 24f, -23f);
        //transform.GetComponent<Camera>().fieldOfView = 60;
        timer30 = timer30 - Time.deltaTime;
        //Debug.Log(timer30);
        if (timer30 < 0)
        {
            timer30 = Maxtimer30;
            indexCheck();
        }
    }
    void doorOpen()
    {
        transform.position = new Vector3(transform.position.x, 19.9f, 0f);

        isDoorOpen = true;
        
        timer30 = timer30 - Time.deltaTime;
        if(timer30 < 0)
        {
            timer30 = Maxtimer30;
            indexCheck();
        }
    }
}
