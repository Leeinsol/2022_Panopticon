using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class tower : MonoBehaviour
{
    private int maXHP = 10000;
    public int hp = 10000;
    public GameObject towerHp;
    int count = 0;
    public bool isHalfHP = false;

    public GameObject WarningText;

    List<NavMeshAgent> navMeshAgents = new List<NavMeshAgent>();

    int enemyNum;

    public bool isFlachingUI = false;
    Image towerHpUI;
    Color originalColor;
    Color FlashColor;

    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    //Camera Shake
    public float shakeTime = 1.0f;
    public float shakeSpeed = 2.0f;
    [SerializeField]
    Vector3 shakeAmount = new Vector3(0f, 0f, 0f);
    Transform cameraTransform;
    GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {

        NavMeshAgent[] foundNavMeshAgents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent agent in foundNavMeshAgents)
        {
            navMeshAgents.Add(agent);
        }

        //enemyNum = navMeshAgents.Count;
        //Debug.Log(navMeshAgents.Count);
        PlayerPrefs.SetInt("AllEnemy", navMeshAgents.Count);
        PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

        //Debug.Log("Number of NavMeshAgents: " + enemyNum);
        towerHpUI = towerHp.transform.GetChild(1).GetComponentInChildren<Image>();
        originalColor = towerHpUI.color;

        postProcessVolume.profile.TryGetSettings(out colorGrading);

        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTransform = Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        towerHp.GetComponent<Slider>().value = hp;

        for (int i = navMeshAgents.Count - 1; i >= 0; i--)
        {
            if (navMeshAgents[i] == null)
            {
                navMeshAgents.RemoveAt(i);
            }
        }
        //Debug.Log(navMeshAgents.Count);
        if (hp <= 0)
        {
            //PlayerPrefs.GetInt("remainEnemy");
            
            PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

            SceneManager.LoadScene("GameOver");
            Debug.Log("Game Over");
        }
        if (navMeshAgents.Count == 0)
        {
            PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

            Debug.Log(hp);
            PlayerPrefs.SetInt("remainTower", hp);
            //PlayerPrefs.GetInt("remainTower");


            if (StageSetting.Instance.getStage() == "easyMode")
            {
                PlayerPrefs.SetInt("Stage", 2);
            }
            else if (StageSetting.Instance.getStage() == "normalMode")
            {
                PlayerPrefs.SetInt("Stage", 3);
            }

            SceneManager.LoadScene("GameClear");


            Debug.Log("Game Clear");
        }
        else if (hp < maXHP / 2f && count == 0)
        {
            count++;
            isHalfHP = true;
        }
        ShowWarningMessage();

        if (hp < maXHP)
        {
            float weight = 1f - (float)hp / maXHP;
            SetPostProcessingWeight(weight);
        }
        if (hp >1950 && hp < 2000)
        {
            StartCoroutine(ShakeCamera());
        }
    }

    void SetPostProcessingWeight(float weight)
    {
        colorGrading.postExposure.value = weight;
        postProcessVolume.weight = weight;
    }

    void ShowWarningMessage()
    {
        if (isHalfHP && WarningText!=null)
        {
            WarningText.SetActive(true);
        }
    }


    public void DecreaseHp(int power)
    {
        hp -= power;
        //if (!isFlachingUI) isFlachingUI = true;
        //else return;
        StartCoroutine("FlashUI");

    }

    IEnumerator FlashUI()
    {
       
        towerHpUI.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        towerHpUI.color = originalColor;
        yield return new WaitForSeconds(0.2f);

    }

    IEnumerator ShakeCamera()
    {
        Vector3 originPos = cameraTransform.localPosition;
        float shakeTimer = 0.0f;

        while (shakeTimer < shakeTime)
        {
            //Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;
            Vector3 randomPoint = originPos + new Vector3(Random.Range(-shakeAmount.x, shakeAmount.x), Random.Range(-shakeAmount.y, shakeAmount.y), shakeAmount.z);


            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            shakeTimer += Time.deltaTime;
        }

        cameraTransform.localPosition = originPos;

    }
}
