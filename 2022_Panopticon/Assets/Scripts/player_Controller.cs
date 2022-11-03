using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Controller : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();             
        CameraRotation();    
        CharacterRotation();
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;


        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")!=0)
        {
            myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()  // ÁÂ¿ì Ä³¸¯ÅÍ È¸Àü
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // ÄõÅÍ´Ï¾ð * ÄõÅÍ´Ï¾ð
        // Debug.Log(myRigid.rotation);  // ÄõÅÍ´Ï¾ð
        // Debug.Log(myRigid.rotation.eulerAngles); // º¤ÅÍ
    }
}

