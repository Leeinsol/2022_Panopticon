using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Controller : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    
    [SerializeField]
    private float sprintSpeed;


    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    public Transform camHandle;
    public Vector3 Amount = new Vector3(.15f, .05f, 0f);
    Animator animator;
    Vector3 Pos;
    float Timer;

    bool iswalking = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        myRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Pos = camHandle.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();             
        CameraRotation();   
        MoveFloor();
        heightFOV();
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;



        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")!=0)
        {
            Vector3 _velocity;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _velocity = (_moveHorizontal + _moveVertical).normalized * sprintSpeed;

            }
            else
            {
                _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

            }
            myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
            animator.SetBool("isRun", true);
            iswalking = true;

            //Timer += Time.deltaTime * 10f;
            //camHandle.localPosition = new Vector3(Pos.x + Mathf.Sin(Timer) * Amount.x,
            //    Pos.y + Mathf.Sin(Timer) * Amount.y,
            //    Pos.z + Mathf.Sin(Timer) * Amount.z);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

    }



    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _yRotation = Input.GetAxisRaw("Mouse X");

        float _cameraRotationX = _xRotation * lookSensitivity;
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_cameraRotationY)); // ÄõÅÍ´Ï¾ð * ÄõÅÍ´Ï¾ð

        // Debug.Log(myRigid.rotation);  // ÄõÅÍ´Ï¾ð
        // Debug.Log(myRigid.rotation.eulerAngles); // º¤ÅÍ
    }

    private void CharacterRotation()  // ÁÂ¿ì Ä³¸¯ÅÍ È¸Àü
    {
        
    }

    void MoveFloor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            SetCameraFOV(60f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = new Vector3(transform.position.x, 7.5f, transform.position.z);
            SetCameraFOV(65f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.position = new Vector3(transform.position.x, 15f, transform.position.z);
            SetCameraFOV(70f);
        }
    } 

    void heightFOV()
    {
        theCamera.fieldOfView = 60f + transform.position.y;
    }

    void SetCameraFOV(float fov)
    {
        //theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, fov, 5f * Time.deltaTime);
        theCamera.fieldOfView = fov;
    }

    //void walkshake()
    //{
    //    if (iswalking)
    //    {
    //        Timer += Time.deltaTime * 10f;
    //        camHandle.localPosition = new Vector3(Pos.x + Mathf.Sin(Timer) * Amount.x,
    //            Pos.y + Mathf.Sin(Timer) * Amount.y,
    //            Pos.z + Mathf.Sin(Timer) * Amount.z);
    //    }
    //    else
    //    {
    //        Timer = 0;
    //        camHandle.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, 
    //            jointOriginalPos.x, Time.deltaTime * bobSpeed), 
    //            Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, 
    //            Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, 
    //            jointOriginalPos.z, Time.deltaTime * bobSpeed));

    //    }
    //}
}

