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

    public SharedData sharedData;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("isCinemaEnd", 1);
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
        }
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void SetCanvas(bool set)
    {
        canvas.SetActive(set);
        Sprintcanvas.SetActive(set);
    }

void moveRotateLeft()
    {
        Quaternion rot = Quaternion.Euler(0, goalRotation, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);

        if (transform.localRotation == rot)
        {
            indexCheck();
        }

    }
    void moveRotateRight()
    {
        Quaternion rot = Quaternion.Euler(0, -goalRotation, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);
        if (transform.localRotation == rot)
        {
            indexCheck();
        }
    }

    void moveRotateMiddle()
    {
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * Speed);

        if (transform.localRotation == rot)
        {
            indexCheck();
        }
    }

    void zoomCamera()
    {
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        transform.localRotation = rot;

        timer15 = timer15 - Time.deltaTime;
        transform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(10, 60, timer15 * 0.33f);

        Vector3 destination = new Vector3(transform.position.x, 19.9f, transform.position.z);
        transform.localPosition = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 0.4f);

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
        sharedData.stage = "easy";
        SceneManager.LoadScene("Main");
    }

    void setCameraPosition()
    {
        transform.position = new Vector3(transform.position.x, 24f, -23f);
        timer30 = timer30 - Time.deltaTime;

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
