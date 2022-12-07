using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CrossHairType
{
    cross, circle, dot
}

public class player_Controller : MonoBehaviour
{
    // Walk
    [SerializeField] private float walkSpeed;
    [SerializeField] Vector3 WalkShakeAmount = new Vector3(.15f, .05f, 0f);
    private bool iswalking = false;
    private float walkHeight;

    // Sprint
    [SerializeField] private bool useSprint = false;
    [SerializeField] private float sprintSpeed;
    private bool isSprinting = false;

    // Stamina
    [SerializeField] GameObject StaminaBar;
    private float stamina = 5;
    private float maxStamina = 5;

    // Crouch
    [SerializeField] private float crouchSpeed;
    private bool isCrouching = false;
    public float crouchHeight;

    // Jump
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance;
    [SerializeField] string LayerName;
    private bool isGround = false;
    private float bufferCheckDistance = 0.1f;

    // Camera
    [SerializeField] private Camera theCamera;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX;
    private Transform camHandle;
    private Vector3 camHandlePos;
    
    // Rigidbody
    private Rigidbody myRigid;
    private Animator animator;

    // Timer
    private float shakeTimer;
    private float zoomTimer;

    // RaycastHit
    private RaycastHit Hit;

    // current variable
    float currentSpeed;
    float currentHeight;

    // Gun
    [SerializeField] GameObject GunModel;
    GameObject GunHandle;

    // Zoom
    [SerializeField] float zoomSpeed = 2f;
    float defaultFOV = 60f;
    float ZoomMultipleNum = 2;
    bool isZooming = false;

    // Canvas
    [SerializeField] Text crossHair;
    public CrossHairType crosshairtype;

    // Start is called before the first frame update
    void Start()
    {
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked; 
        
        myRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        camHandle = transform.Find("CamHandle");
        GunHandle = theCamera.transform.Find("GunHandle").gameObject;

        // initialize
        camHandlePos = camHandle.localPosition;
        walkHeight = theCamera.transform.localPosition.y;
        currentSpeed = walkSpeed;
        currentHeight = walkHeight;
        zoomTimer = zoomSpeed;

        // gun instantiate
        GameObject Gun = Instantiate(GunModel, GunHandle.transform) as GameObject;
        Gun.transform.parent = GunHandle.transform;
        //Gun.transform.SetParent(theCamera.transform, false);

        if (useSprint)
        {
            StaminaBar.SetActive(true);
        }
        else
        {
            StaminaBar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //StaminaBar.enabled = false;
        Move();

        CameraRotationVerticality();
        CameraRotationHorizontality();
        
        heightFOV();
        walkshake();

        if(Input.GetKeyDown(KeyCode.LeftShift) && iswalking)
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            zoomTimer = zoomSpeed;
            isSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
            isCrouching = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            currentSpeed = walkSpeed;
            isCrouching = false;
        }

        if (useSprint)
        {
            Sprint();
            StaminaUI();
        }
        Crouch();
        Jump();
        ZoomCamera();
        SetCrossHair();
    }

    void SetCrossHair()
    {
        if (crosshairtype == CrossHairType.cross)
        {
            crossHair.text = "+";
        }
        else if (crosshairtype == CrossHairType.circle)
        {
            crossHair.text = "��";
        }
        else if (crosshairtype == CrossHairType.dot)
        {
            crossHair.text = ".";
        }
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
            _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;
            transform.localScale = new Vector3(transform.localScale.x, walkHeight, transform.localScale.z);

            //if (isSprinting)
            //{
            //    _velocity = (_moveHorizontal + _moveVertical).normalized * sprintSpeed;

            //}
            //else
            //{
            //    _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

            //}
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
            iswalking = false;
            animator.SetBool("isRun", false);
        }

    }


    void Sprint()
    {
        if (isSprinting)
        {
            SetFOVSmooth(theCamera.fieldOfView + 5);
            currentSpeed = sprintSpeed;
            stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                isSprinting = false;

            }
        }
        else if(stamina < maxStamina){
            stamina += Time.deltaTime;
        }

    }


    void Crouch()
    {
        if (isCrouching)
        {
            //nowSpeed = crouchSpeed;
            currentHeight = crouchHeight;
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            //float center = crouchHeight / 2;
            //GetComponent<CapsuleCollider>().height = Mathf.Lerp(GetComponent<CapsuleCollider>().height, crouchHeight, 0.1f);
            //GetComponent<CapsuleCollider>().center = Vector3.Lerp(GetComponent<CapsuleCollider>().center, new Vector3(0, center, 0), 0.3f);

        }
        else
        {
            //nowSpeed = walkSpeed;
            currentHeight = walkHeight;
            //Debug.Log("move: " + moveHeight);
            //Debug.Log("now: " + nowHeight);
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            //float center = moveHeight;
            //GetComponent<CapsuleCollider>().height = Mathf.Lerp(GetComponent<CapsuleCollider>().height, nowHeight, 0.1f);
            //GetComponent<CapsuleCollider>().center = Vector3.Lerp(GetComponent<CapsuleCollider>().center, new Vector3(0, 0, 0), 0.3f);

        }

    }

    void StaminaUI()
    {
        float ratio = stamina / maxStamina;

        StaminaBar.GetComponent<Slider>().value = ratio;
    }



    private void CameraRotationVerticality()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;
        
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        // Debug.Log(myRigid.rotation);  // ���ʹϾ�
        // Debug.Log(myRigid.rotation.eulerAngles); // ����
    }

    void CameraRotationHorizontality()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_cameraRotationY)); // ���ʹϾ� * ���ʹϾ�
    }

   

    void heightFOV()
    {
        theCamera.fieldOfView = defaultFOV + transform.position.y;
    }

    void walkshake()
    {
        if (iswalking)
        {
            shakeTimer += Time.deltaTime * 7f;
            camHandle.localPosition = new Vector3(camHandlePos.x + Mathf.Sin(shakeTimer) * WalkShakeAmount.x,
                camHandlePos.y + Mathf.Sin(shakeTimer) * WalkShakeAmount.y,
                camHandlePos.z + Mathf.Sin(shakeTimer) * WalkShakeAmount.z);
        }
        else
        {
            shakeTimer = 0;

            camHandle.localPosition = new Vector3(Mathf.Lerp(camHandle.localPosition.x, camHandlePos.x, Time.deltaTime * 7f),
                Mathf.Lerp(camHandle.localPosition.y, camHandlePos.y, Time.deltaTime * 10f),
                Mathf.Lerp(camHandle.localPosition.z, camHandlePos.z, Time.deltaTime * 10f));


        }
    }

    void Jump()
    {

        //CapsuleCollider capsuleCollider = transform.GetComponent<CapsuleCollider>();
        ////Debug.Log(capsuleCollider.bounds.extents.y);
        ////isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+ 1.5f); ;
        //isGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f);
        //Debug.Log(isGround);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red) ;


        //Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        //Vector3 direction = transform.TransformDirection(Vector3.down);
        //float distance = .75f;

        //if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        //{
        //    Debug.DrawRay(origin, direction * distance, Color.red);
        //    isGround = true;
        //}
        //else
        //{
        //    isGround = false;
        //}

        //RaycastHit hit;
        //RaycastHit hitInfo = new RaycastHit();

        //Ray landingRay = new Ray(transform.position, Vector3.down);
        //Debug.DrawRay(landingRay.origin, landingRay.direction * 10f, Color.red);
        //if (Physics.Raycast(landingRay, out hitInfo))
        //{
        //    Debug.Log(hitInfo.collider.name);
        //    if (hitInfo.collider == null)
        //    {
        //        isGround = true;
        //    }
        //    else
        //    {
        //        isGround = false;
        //    }
        //}


        //Debug.Log("groundcheck");
        groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance, 1 << LayerMask.NameToLayer(LayerName)))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

    }

    void ZoomCamera()
    {
        if (Input.GetMouseButton(1))
        {
            SetFOVSmooth(defaultFOV / ZoomMultipleNum);
        }
        if (Input.GetMouseButtonUp(1))
        {
            zoomTimer = zoomSpeed;
            //SetFOVSmooth(defaultFOV);
        }
    }

    void SetFOVSmooth(float target)
    {
        Debug.Log(zoomTimer);
        //float time = zoomSpeed;
        zoomTimer -= Time.deltaTime;
        //float angle = Mathf.Abs((defaultFOV / ZoomMultipleNum) - defaultFOV);
        //theCamera.fieldOfView = Mathf.MoveTowards(target, theCamera.fieldOfView, Time.deltaTime * (angle/0.15f));
        //theCamera.fieldOfView = Mathf.MoveTowards(target, theCamera.fieldOfView, Time.deltaTime * 0.15f);
        //Debug.Log(theCamera.fieldOfView);
        theCamera.fieldOfView = Mathf.Lerp(target, theCamera.fieldOfView, zoomTimer * 0.5f);
    }
}

