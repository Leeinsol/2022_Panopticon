using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    // Walk
    public float walkSpeed;
    public Vector3 HeadBobAmount = new Vector3(0f, .05f, 0f);
    public bool iswalking = false;
    private float walkHeight;

    // Sprint
    public bool useSprint = true;
    public float sprintSpeed = 50f;
    public bool isSprinting = false;

    // Stamina
    public bool useStaminaLimit = true;
    public GameObject StaminaBar;
    public float maxStamina = 5f;
    private float stamina = 5f;

    // Crouch
    public bool useCrouch = true;
    public float crouchSpeed = 15f;
    public float crouchHeight = 0.5f;
    public bool isCrouching = false;

    // Head Bob
    public bool useHeadBob = true;
    public float walkHeadBobSpeed = 5f;
    public float sprintHeadBobSpeed = 10f;

    // Jump
    public bool useJump = true;
    public float jumpForce = 5f;
    float groundCheckDistance = .1f;
    private float bufferCheckDistance = .1f;
    public bool isGround = false;

    // Camera
    public bool useCameraRotationVerticality = true;
    public bool useCameraRotationHorizontality = true;
    public Camera theCamera;
    public float lookSensitivity = 2f;
    public float cameraRotationLimit = 60f;
    private float currentCameraRotationX;
    Transform camHandle;
    Vector3 camHandlePos;

    // Zoom
    public bool useCameraZoom = true;
    public float zoomSpeed = 2f;
    public bool isZooming = false;
    float defaultFOV = 60f;
    float ZoomMultipleNum = 2;

    // Gun
    public bool useGun = true;
    public GameObject GunModel;
    public GameObject BombModel;
    public GameObject EnergeDrinkModel;

    public float GunRotationSpeed = 3f;
    GameObject GunHandle;

    public GameObject[] Weapon = new GameObject[4];

    public bool useItem = true;

    //GameObject Gun;
    //GameObject Bomb;
    //GameObject EnergeDrink;

    Vector3 GunOriginPos = new Vector3(0, 0, 0);
    Quaternion GunOriginRot = Quaternion.Euler(0, 180, 0);
    Vector3 GunSprintPos = new Vector3(-0.271f, 0.001f, 0.135f);
    Quaternion GunSprintRot = Quaternion.Euler(-0.133f, 123.826f, -0.249f);
    Vector3 GetItemPos = new Vector3(0, 0.2f, 0);
    Quaternion GetItemRot = Quaternion.Euler(-15f, 180, 0);
    public Transform BombPostion;
    public float BombRadius = 10.0f;
    public float BombForce = 500f;
    public float throwForce = 1000f;

    int BombDamage;
    new Rigidbody rigidbody;

    // Fire
    public GameObject bulletEffect;
    ParticleSystem psBullet;
    public int maxBulletNum = 10;
    public int bulletNum;
    public float fireRate = 0.5f;
    public float fireTimer;

    // Canvas
    public Text crossHairText;
    public CrossHairType crosshairtype;
    public GameObject BulletNumUI;
    public GameObject ReloadTimerUI;
    public GameObject ItemNumUI;

    //Reload
    public bool useReload = true;
    public reloadBulletType reloadType;
    public float maxReloadTime = 5f;
    public float allReloadTime = 1f;
    public float reloadActionForce = 0.5f;
    float ReloadTimer;
    float OneBulletReloadTime;
    float currentReloadTime;
    public bool isReload = false;

    // Sound
    public bool useFireSound = true;
    public bool useReloadSound = true;
    public AudioClip FireSound, oneByOneReloadSound, allReloadSound, getItemSound, changeWaeponSound, energySound;
    AudioSource audioSource;

    // Timer
    private float shakeTimer;
    private float zoomTimer;
    float energyTimer;
    float ultimateTimer;
    public float ultimateTime = 7f;

    // current variable
    float currentSpeed;

    public float flightLengthFactor = 0f;
    public float IncreaseAmount = 2f;

    public Image BombGauge;

    public int bulletPower = 1;
    public int currentBulletPower = 1;
    bool isPowerUp = false;

    public int Item2Num = 0;
    public int[] WeaponNum;
    private int currentIndex = 0;

    public GameObject RemainItemNumUI;
    public GameObject PowerTimeUI;
    public GameObject getItemModel;

    public GameObject BombInstantiate;

    bool isPulling = false;
    float distanceThreshold = 1f;

    public int ultimateGauge = 0;
    public float radius = 1f;
    public Transform middleAimPoint;
    List<Collider> colliders = new List<Collider>();

    public GameObject ultimateCrossHair;
    public Canvas playerCanvas;
    Collider closestCollider = null;

    float RayCastDis = 21f;
    public int ultimateNum = 100;

    public GameObject getItemEffect;
    
    private void Start()
    {
        ultimateNum = 100;
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        // initialize
        rb = GetComponent<Rigidbody>();
        audioSource = StageSetting.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        camHandle = transform.Find("CamHandle");
        GunHandle = theCamera.transform.Find("GunHandle").gameObject;

        camHandlePos = camHandle.localPosition;
        walkHeight = GetComponent<CapsuleCollider>().height;
        currentSpeed = walkSpeed;
        zoomTimer = zoomSpeed;

        WeaponNum = new int[Weapon.Length];

        // gun instantiate
        if (useGun)
        {
            // Gun instantiate
            Weapon[0] = Instantiate(GunModel, GunHandle.transform) as GameObject;
            Weapon[0].transform.parent = GunHandle.transform;
            WeaponNum[0] = 1;

            // bomb instantiate
            Weapon[2] = Instantiate(BombModel, GunHandle.transform) as GameObject;
            Weapon[2].transform.parent = GunHandle.transform;
            WeaponNum[2] = 0;

            Destroy(Weapon[2].GetComponent<Bomb>());
            Destroy(Weapon[2].GetComponent<Rigidbody>());

            //Weapon[1].SetActive(false);
        }
        else
        {
            crossHairText.enabled = false;
        }

        if (useItem)
        {
            // energydrink instantiate
            Weapon[3] = Instantiate(EnergeDrinkModel, GunHandle.transform) as GameObject;
            Weapon[3].transform.parent = GunHandle.transform;
            //Weapon[2].SetActive(false);
            WeaponNum[3] = 0;
        }

        // bullet instantiate
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        psBullet = bulletEffect.GetComponent<ParticleSystem>();

        // reload instantiate
        SetReloadTimer();
        ReloadTimerUI.SetActive(false);
        currentReloadTime = ReloadTimer;
        ReloadTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;

        if (!useReload || !useGun) BulletNumUI.SetActive(false);

        // set SprintBar
        if (useSprint && useStaminaLimit) StaminaBar.SetActive(true);
        else StaminaBar.SetActive(false);

        // cross hair instantiate
        SetCrossHair();
        BombGauge.transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < Weapon.Length; i++)
        {
            if (WeaponNum[i] > 0)
            {
                currentIndex = i;
                break;
            }
        }
        // getItem instantiate
        Weapon[1] = Instantiate(getItemModel, GunHandle.transform) as GameObject;
        Weapon[1].transform.parent = GunHandle.transform;

        WeaponNum[1] = 1;


        offWeapon();
        Weapon[currentIndex].SetActive(true);

        setRemainItemUI(false);
        ultimateTimer = ultimateTime;


        if (StageSetting.Instance.getStage() == "easy")
        {
            ultimateNum = 30;
            ultimateTime = 12;
        }
        else if (StageSetting.Instance.getStage() == "normal")
        {
            ultimateNum = 50;
            ultimateTime = 10;
        }
        else if (StageSetting.Instance.getStage() == "hard")
        {
            ultimateNum = 100;
            ultimateTime = 7;
        }
    }

    private void Update()
    {
        if (useSprint)
        {
            Sprint();
            StaminaUI();
        }
        if (!isZooming && !isSprinting) theCamera.fieldOfView = defaultFOV;
        Crouch();
        checkGroundDistance();

        if (ultimateGauge >= ultimateNum)
        {
            setUltimateTimer();
        }

        if (!Weapon[0].activeSelf)
        {
            setUltimateCrossHair(0.2f);
        }

        if (useGun && Weapon[2].activeSelf && Time.timeScale > 0)
        {
            if (useReload)
            {
                reloadBomb();
                setRemainItemUI(true);
                RemainBombNum();
            }
        }

        if (useReload)
        {
            bulletUI();
        }

        powerUp();
        changeWeapon();
    }

    public void endSprint()
    {
        currentSpeed = walkSpeed;
        zoomTimer = zoomSpeed;
        isSprinting = false;
        setGunOrigin();
    }

    public void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 _velocity;
            _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            iswalking = true;

        }

    }

    public void Sprint()
    {
        if (isSprinting)
        {
            SetFOVSmooth(65);

            currentSpeed = sprintSpeed;

            if (useGun)
            {
                if (currentIndex == 0 || currentIndex == 1)
                {

                    Weapon[currentIndex].transform.localPosition = Vector3.Slerp(Weapon[currentIndex].transform.localPosition, GunSprintPos, GunRotationSpeed * Time.deltaTime);
                    Weapon[currentIndex].transform.localRotation = Quaternion.Slerp(Weapon[currentIndex].transform.localRotation, GunSprintRot, GunRotationSpeed * Time.deltaTime);
                }
            }

            if (useStaminaLimit) stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                isSprinting = false;
                setGunOrigin();
            }
        }
        else if (stamina < maxStamina) stamina += Time.deltaTime;
    }

    public void setCrouch()
    {
        if (!useCrouch) return;
        currentSpeed = crouchSpeed;
        isCrouching = true;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z);
    }

    public void EndCrouch()
    {
        currentSpeed = walkSpeed;
        isCrouching = false;
    }
    public void Crouch()
    {
        if (!useCrouch) return;
        if (isCrouching) GetComponent<CapsuleCollider>().height = crouchHeight;

        else GetComponent<CapsuleCollider>().height = walkHeight;

    }

    public void CameraRotationVerticality()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    public void CameraRotationHorizontality()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_cameraRotationY));
    }

    public void checkGroundDistance()
    {
        if (!useJump) return;
        if (!isCrouching) groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        else groundCheckDistance = (GetComponent<CapsuleCollider>().height * 2) + bufferCheckDistance;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance)) isGround = true;
        else isGround = false;
    }
    public void Jump()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void HeadBob()
    {
        if (!useHeadBob) return;
        if (iswalking && isSprinting)
        {
            shakeTimer += Time.deltaTime * sprintHeadBobSpeed;

            camHandle.localPosition = new Vector3(camHandlePos.x + Mathf.Sin(shakeTimer) * HeadBobAmount.x,
                camHandlePos.y + Mathf.Sin(shakeTimer) * HeadBobAmount.y,
                camHandlePos.z + Mathf.Sin(shakeTimer) * HeadBobAmount.z);

        }
        else if (iswalking && !isSprinting)
        {
            shakeTimer += Time.deltaTime * walkHeadBobSpeed;

            camHandle.localPosition = new Vector3(camHandlePos.x + Mathf.Sin(shakeTimer) * HeadBobAmount.x,
                    camHandlePos.y + Mathf.Sin(shakeTimer) * HeadBobAmount.y,
                    camHandlePos.z + Mathf.Sin(shakeTimer) * HeadBobAmount.z);
        }

        else
        {
            shakeTimer = 0;
            camHandle.localPosition = new Vector3(Mathf.Lerp(camHandle.localPosition.x, camHandlePos.x, Time.deltaTime * 7f),
              Mathf.Lerp(camHandle.localPosition.y, camHandlePos.y, Time.deltaTime * 10f),
              Mathf.Lerp(camHandle.localPosition.z, camHandlePos.z, Time.deltaTime * 10f));
        }
    }

    public void ZoomCamera()
    {
        isZooming = true;
        SetFOVSmooth(defaultFOV / ZoomMultipleNum);
    }

    public void EndZoomCamera()
    {
        isZooming = false;
        zoomTimer = zoomSpeed;
        SetFOVSmooth(defaultFOV);
    }

    public void Fire()
    {
        if (!useGun) return;
        if (!useReload && ultimateGauge < ultimateNum) ShootBullet();

        if (bulletNum > 0 && useReload)
        {
            if (isReload)
            {
                setReloadBulletUI(false);
                return;
            }
            ShootBullet();
        }
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;
    }

    public void bombFire()
    {
        if (!useGun) return;
        if (ReloadTimerUI.activeSelf) return;

        BombUI();
        flightLengthFactor += IncreaseAmount * Time.deltaTime;
        if (flightLengthFactor >= 1f)
        {
            flightLengthFactor = 1f;
            IncreaseAmount = -IncreaseAmount;
            //Debug.Log("check");
        }
        else if (flightLengthFactor <= 0f)
        {
            flightLengthFactor = 0f;
            IncreaseAmount = -IncreaseAmount;
        }

    }

    public void endBombFire()
    {
        if (!useGun) return;
        if (ReloadTimerUI.activeSelf) return;


        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();


        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Ignore Raycast")))
        {
            Vector3 forwardPosition = theCamera.transform.position + theCamera.transform.forward * 2f;
            Vector3 nextVector = hitInfo.point - transform.position;
            nextVector.y = 5;

            GameObject bomb = Instantiate(BombInstantiate, forwardPosition, transform.rotation);
            Rigidbody rigidBomb = bomb.GetComponent<Rigidbody>();


            rigidBomb.AddForce(nextVector * flightLengthFactor, ForceMode.Impulse);
            rigidBomb.AddTorque(Vector3.back * 10, ForceMode.Impulse);

            //Collider playerCollider = GetComponent<Collider>();
            //Collider bombCollider = bomb.transform.GetChild(0).GetComponent<Collider>();
            //Physics.IgnoreCollision(playerCollider, bombCollider);
            flightLengthFactor = 0f;
            BombGauge.transform.parent.gameObject.SetActive(false);
            //reloadBomb();

            //reloadBomb();

            WeaponNum[2]--;

            if (WeaponNum[2] <= 0)
            {
                Debug.Log("다 사용했어요" + currentIndex);
                currentIndex++;
                changeWeaponNext(currentIndex);
                setRemainItemUI(false);
            }
            else
            {
                SetReload();
            }
        }
    }
    void BombUI()
    {
        //Debug.Log("set UI");
        BombGauge.transform.parent.gameObject.SetActive(true);

        BombGauge.fillAmount = flightLengthFactor;
    }
    void changeWeapon()
    {
        int oldIndex = currentIndex;

        Vector2 scrollDelta = Input.mouseScrollDelta;

        //changeWeaponPrevious(oldIndex, scrollDelta);
        //changeWeaponNext(oldIndex, scrollDelta);

        if (scrollDelta.y != 0 && Time.timeScale > 0)
        {
            if (scrollDelta.y > 0)
            {
                // Scroll up
                changeWeaponPrevious(oldIndex);
            }
            else if (scrollDelta.y < 0)
            {
                // Scroll down
                changeWeaponNext(oldIndex);
            }
            setRemainItemUI(false);
            PlaySoundEffects(changeWaeponSound);

            //Debug.Log(currentIndex);

            if (isReload)
            {
                //isReload = false;
                setReloadBulletUI(false);
            }

            if (ultimateGauge < ultimateNum)
            {
                setUltimateCrossHair(0.2f);
            }
        }


        offWeapon();
        Weapon[currentIndex].SetActive(true);
        //Debug.Log(currentIndex);
        if (currentIndex == 0 && ultimateGauge < ultimateNum) BulletNumUI.SetActive(true);
        else if (currentIndex == 2 || currentIndex == 3) ItemNumUI.SetActive(true);
    }

    void changeWeaponPrevious(int oldIndex)
    {
        do
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = WeaponNum.Length - 1;
            }
        } while (WeaponNum[currentIndex] == 0 && currentIndex != oldIndex);
    }

    void changeWeaponNext(int oldIndex)
    {
        do
        {
            currentIndex++;
            if (currentIndex >= WeaponNum.Length)
            {
                currentIndex = 0;
            }
        } while (WeaponNum[currentIndex] == 0 && currentIndex != oldIndex);
    }
    void ShootBullet()
    {
        if (fireTimer < fireRate)
        {
            return;
        }
        //Debug.Log("shootBullet");

        if (isSprinting)
        {
            isSprinting = false;
            setGunOrigin();
        }
        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);
        bulletNum--;

        if (useFireSound)
        {
            audioSource.volume = 0.7f;
            PlaySoundEffects(FireSound);
        }

        StopAllCoroutines();
        StartCoroutine(reloadActionCoroutine());


        if (Physics.Raycast(ray, out hitInfo, RayCastDis))
        {
            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;

            // Particle: Looping -> False / Stop Action -> Destroy
            //Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            //psBullet.Play();
            var effect = PollingManager.GetObject(EffectType.Fire);
            effect.transform.position = hitInfo.point;
            effect.transform.forward = hitInfo.normal;

            effect.Play();

            Collider collider = hitInfo.collider;

            // 총 맞았을 때
            if (collider.gameObject.GetComponent<Enemy>())
            {

                if (collider is CapsuleCollider)
                {
                    //Debug.Log("캡슐");
                    //Debug.Log(currentBulletPower);
                    collider.gameObject.GetComponent<Enemy>().hp -= currentBulletPower;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    collider.gameObject.GetComponent<Enemy>().playHurtAnim();
                    collider.gameObject.GetComponent<Enemy>().playBloodEffect(hitInfo);
                    ultimateGauge++;
                    //Debug.Log(ultimateGauge);
                }
                if (collider is SphereCollider)
                {
                    Debug.Log("머리");
                    //collider.gameObject.GetComponent<Enemy>().hp -= 2;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    //collider.gameObject.GetComponent<Enemy>().playHurtAnim();

                    collider.gameObject.GetComponent<Enemy>().hp -= (currentBulletPower * 2);

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    collider.gameObject.GetComponent<Enemy>().playHurtAnim();

                    ultimateGauge += 2;
                    Debug.Log(ultimateGauge);
                }
            }
        }
        fireTimer = 0f;
    }

    public void SetReload()
    {
        isReload = true;
        if (useReload) ReloadTimerUI.SetActive(true);

        SetReloadTimer();
        currentReloadTime = ReloadTimer;
        //Debug.Log(ReloadTimer);
        ReloadTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }

    void PlaySoundEffects(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }


    IEnumerator reloadActionCoroutine()
    {
        Vector3 reloadAction = new Vector3(GunOriginPos.x, GunOriginPos.y, reloadActionForce);
        Weapon[0].transform.localPosition = GunOriginPos;

        while (Weapon[0].transform.localPosition.z <= reloadActionForce - 0.02f)
        {
            Weapon[0].transform.localPosition = Vector3.Lerp(Weapon[0].transform.localPosition, reloadAction, 0.4f);
            yield return null;
        }

        while (Weapon[0].transform.localPosition != GunOriginPos)
        {
            Weapon[0].transform.localPosition = Vector3.Lerp(Weapon[0].transform.localPosition, GunOriginPos, 0.1f);
            yield return null;
        }
    }

    public void setUltimateState()
    {
        crossHairText.enabled = false;
        BulletNumUI.SetActive(false);
        showClosestCEnemy();

    }
    public void setFireTimer()
    {
        fireTimer = fireRate;
    }
    public void ultimateFire()
    {
        shootUltimateBullet();
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;

    }

    void showClosestCEnemy()
    {
        ultimateCrossHair.SetActive(true);
        //crossHairText.enabled = false;
        setUltimateCrossHair(0.2f);

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, RayCastDis))
        {
            Collider[] collidersInRange = Physics.OverlapSphere(hitInfo.point, radius);

            closestCollider = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider collider in collidersInRange)
            {
                if (collider.name == "ZombiePrefab")
                {

                    float distance = Vector3.Distance(collider.transform.position, hitInfo.point);
                    if (distance < closestDistance)
                    {
                        closestCollider = collider;
                        closestDistance = distance;
                    }
                }
            }
            //OnDrawGizmos();


            if (closestCollider != null)
            {
                //Debug.Log("! null");

                setUltimateCrossHair(1f);

                Vector3 colliderCenter = new Vector3(closestCollider.gameObject.transform.position.x, 1.7f, closestCollider.gameObject.transform.position.z);

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(colliderCenter);

                Vector2 localPosition;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(playerCanvas.transform as RectTransform, screenPosition, playerCanvas.worldCamera, out localPosition);
                ultimateCrossHair.GetComponent<RectTransform>().anchoredPosition = localPosition;
            }
        }
    }

    void shootUltimateBullet()
    {
        if (fireTimer < fireRate)
        {
            return;
        }

        //Debug.Log("shootUltimateBullet");

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();

        if (useFireSound) PlaySoundEffects(FireSound);

        StopAllCoroutines();
        StartCoroutine(reloadActionCoroutine());
        if (Physics.Raycast(ray, out hitInfo, RayCastDis))
        {
            //Debug.Log("RayCast");
            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;

            Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            psBullet.Play();

            if (closestCollider != null)
            {
                //Debug.Log("closestCollider: " + closestCollider);
                closestCollider.GetComponent<Enemy>().hp -= currentBulletPower;
                //closestCollider.GetComponent<Enemy>().decreaseHP();
                //closestCollider.GetComponent<Enemy>().decreaseHP(currentBulletPower);

                closestCollider.gameObject.GetComponent<Enemy>().playHurtAnim();
                closestCollider.gameObject.GetComponent<Enemy>().playBloodEffect(hitInfo);
            }
        }

        fireTimer = 0f;
    }

    void setUltimateCrossHair(float alpha)
    {
        Color color = ultimateCrossHair.transform.GetChild(0).GetComponent<Image>().color;
        color.a = alpha;

        if (alpha < 0.8f)
        {
            ultimateCrossHair.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        ultimateCrossHair.transform.GetChild(0).GetComponent<Image>().color = color;
    }

    void setUltimateTimer()
    {

        ultimateTimer -= Time.deltaTime;
        if (ultimateTimer < 0)
        {
            ultimateTimer = ultimateTime;
            currentBulletPower = bulletPower;
            ultimateGauge = 0;
            ultimateCrossHair.SetActive(false);
            crossHairText.enabled = true;
            BulletNumUI.SetActive(true);
        }
    }


    public void getItem()
    {

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);


        //RaycastHit hitInfo = new RaycastHit();
        RaycastHit[] hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.parent != null && hit.transform.parent.gameObject.tag == "EnergyDrink")
            {
                //Debug.Log("에너지 드링크");
                isPulling = true;
                StartCoroutine(PullItem(hit.transform.parent.gameObject));
                break;
            }
            if (hit.transform.gameObject.tag == "Bomb")
            {
                isPulling = true;
                StartCoroutine(PullItem(hit.transform.gameObject));
                break;
            }
        }
    }

    IEnumerator PullItem(GameObject Object)
    {
        float t = 0;
        Vector3 originalPosition = Object.transform.position;

        while (isPulling)
        {
            t += Time.deltaTime * 1.2f;
            float shakeAmount = 0.1f;
            Vector3 randomOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));

            Object.transform.position = Vector3.Lerp(originalPosition, transform.position + randomOffset, t);

            float distance = Vector3.Distance(Object.transform.position, transform.position);
            if (distance < distanceThreshold)
            {
                if (Object.gameObject.tag == "Bomb")
                {
                    WeaponNum[2]++;
                }

                if (Object.gameObject.tag == "EnergyDrink")
                {
                    WeaponNum[3]++;
                }
                PlaySoundEffects(getItemSound);


                var effect = PollingManager.GetObject(EffectType.GetItem);
                effect.transform.position = Object.transform.position;
     

                effect.Play();
                
                //Instantiate(getItemEffect.GetComponent<ParticleSystem>(), getItemEffect.transform.position, Quaternion.Euler(getItemEffect.transform.forward));
                //getItemEffect.GetComponent<ParticleSystem>().Play();

                Destroy(Object);
                yield break;
            }

            yield return null;
        }
    }

    public void pullItemMotion()
    {
        Weapon[1].transform.localPosition = Vector3.Slerp(Weapon[1].transform.localPosition, GetItemPos, GunRotationSpeed * Time.deltaTime);
        Weapon[1].transform.localRotation = Quaternion.Slerp(Weapon[1].transform.localRotation, GetItemRot, GunRotationSpeed * Time.deltaTime);
    }

    void reloadBomb()
    {
        if (!isReload) return;
        if (WeaponNum[1] <= 0) return;

        ReloadTimer -= Time.deltaTime;
        ReloadTimerUI.GetComponent<Slider>().value = ReloadTimer;

        //Debug.Log("reload Time: " + ReloadTimer);

        if (ReloadTimer < 0)
        {
            //Debug.Log("reload Time: " + ReloadTimer);

            PlaySoundEffects(allReloadSound);
            setReloadBulletUI(false);
        }
    }

    void RemainBombNum()
    {
        ItemNumUI.transform.GetChild(0).GetComponent<Text>().text = WeaponNum[2].ToString();
    }

    public void eatEnergyDrink()
    {
        PlaySoundEffects(energySound);
        currentBulletPower = Weapon[3].GetComponent<Item_energyDrink>().energyDrink.getPower();

        isPowerUp = true;
        energyTimer += Weapon[3].GetComponent<Item_energyDrink>().energyDrink.getTime();


        WeaponNum[3]--;
        PowerTimeUI.GetComponent<Slider>().maxValue = energyTimer;


        if (WeaponNum[3] <= 0)
        {
            currentIndex++;
            changeWeaponNext(currentIndex);

        }
    }

    public void RemainEnergyDrinkNum()
    {
        ItemNumUI.transform.GetChild(0).GetComponent<Text>().text = WeaponNum[3].ToString();
    }

    void powerUp()
    {
        if (!isPowerUp) return;
        //Debug.Log("powerUP");

        //PowerTimeUI.transform.GetChild(0).GetComponent<Text>().text = energyTimer.ToString();
        //ReloadTimerUI.SetActive(true);
        //ReloadTimerUI.GetComponent<Slider>().value = energyTimer;

        PowerTimeUI.SetActive(true);
        PowerTimeUI.GetComponent<Slider>().value = energyTimer;
        energyTimer -= Time.deltaTime;
        //Debug.Log(energyTimer);
        //Debug.Log(currentBulletPower);


        if (energyTimer < 0)
        {
            //Debug.Log("끝");

            energyTimer = Weapon[3].GetComponent<Item_energyDrink>().energyDrink.getTime();
            //setReloadBulletUI(false);
            PowerTimeUI.SetActive(false);
            energyTimer = 0;
            PowerTimeUI.GetComponent<Slider>().value = energyTimer;

            currentBulletPower = bulletPower;
            Debug.Log(currentBulletPower);
            setRemainItemUI(false);
            isPowerUp = false;
        }
    }


    void SetFOVSmooth(float target)
    {
        zoomTimer -= Time.deltaTime;
        theCamera.fieldOfView = Mathf.Lerp(target, theCamera.fieldOfView, zoomTimer * 0.5f);
    }

    public void setGunOrigin()
    {
        isPulling = false;

        Weapon[currentIndex].transform.localPosition = GunOriginPos;
        Weapon[currentIndex].transform.localRotation = GunOriginRot;
    }

    public void bulletUI()
    {
        BulletNumUI.transform.Find("bulletText").gameObject.GetComponent<Text>().text = bulletNum.ToString();
        BulletNumUI.transform.Find("MaxBulletText").gameObject.GetComponent<Text>().text = maxBulletNum.ToString();
    }
    public void PressReloadKey()
    {
        if (!isReload && bulletNum != maxBulletNum) SetReload();
    }

    public void reloadBullet()
    {
        if (!isReload) return;

        ReloadTimer -= Time.deltaTime;
        ReloadTimerUI.GetComponent<Slider>().value = ReloadTimer;

        if (reloadType == reloadBulletType.oneByOneReload) increaseBullet();

        if (ReloadTimer < 0)
        {
            bulletNum = maxBulletNum;

            if (reloadType == reloadBulletType.allReload && useReloadSound) PlaySoundEffects(allReloadSound);

            setReloadBulletUI(false);
        }
    }

    void increaseBullet()
    {
        for (int i = 1; i < maxBulletNum - bulletNum + 1; i++)
        {
            if (currentReloadTime - OneBulletReloadTime > ReloadTimer)
            {
                bulletNum++;
                if (reloadType == reloadBulletType.oneByOneReload && useReloadSound) PlaySoundEffects(oneByOneReloadSound);

                currentReloadTime -= OneBulletReloadTime;
            }
        }
    }
    void SetReloadTimer()
    {
        if (reloadType == reloadBulletType.oneByOneReload) ReloadTimer = OneBulletReloadTime * (maxBulletNum - bulletNum);

        else if (reloadType == reloadBulletType.allReload) ReloadTimer = allReloadTime;

        //Bomb
        if (Weapon[2].activeSelf) ReloadTimer = 2f;
        //else ReloadTimer = 2f;
    }

    void offWeapon()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive(false);
        }
        BulletNumUI.SetActive(false);
        ItemNumUI.SetActive(false);
    }

    void SetCrossHair()
    {
        if (crosshairtype == CrossHairType.cross) crossHairText.text = "+";

        else if (crosshairtype == CrossHairType.circle) crossHairText.text = "○";

        else if (crosshairtype == CrossHairType.dot) crossHairText.text = ".";
    }

    void setReloadBulletUI(bool state)
    {
        ReloadTimerUI.SetActive(state);
        ReloadTimer = maxReloadTime;
        isReload = state;
        ReloadTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }
    public void setRemainItemUI(bool isShow)
    {
        ItemNumUI.SetActive(isShow);
        //Debug.Log("실행 " + RemainItemNumUI.activeSelf);

    }

    void StaminaUI()
    {
        float ratio = stamina / maxStamina;

        StaminaBar.GetComponent<Slider>().value = ratio;
    }

}
