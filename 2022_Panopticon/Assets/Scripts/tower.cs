using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

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

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent[] foundNavMeshAgents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent agent in foundNavMeshAgents)
        {
            navMeshAgents.Add(agent);
        }

        //enemyNum = navMeshAgents.Count;
        Debug.Log(navMeshAgents.Count);
        PlayerPrefs.SetInt("AllEnemy", navMeshAgents.Count);
        PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

        //Debug.Log("Number of NavMeshAgents: " + enemyNum);
        towerHpUI = towerHp.transform.GetChild(1).GetComponentInChildren<Image>();
        originalColor = towerHpUI.color;
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
            PlayerPrefs.GetInt("remainTower");
            SceneManager.LoadScene("GameClear");
            Debug.Log("Game Clear");
        }
        else if (hp < maXHP / 2f && count == 0)
        {
            count++;
            isHalfHP = true;
        }
        ShowWarningMessage();

        if (isFlachingUI)
        {
        }
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

        isFlachingUI = false;
    }
}
