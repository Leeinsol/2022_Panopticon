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
        PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

        //Debug.Log("Number of NavMeshAgents: " + enemyNum);
    }

    // Update is called once per frame
    void Update()
    {
        towerHp.GetComponent<Slider>().value = hp;

        if (hp <= 0)
        {
            //PlayerPrefs.GetInt("remainEnemy");
            for (int i = navMeshAgents.Count - 1; i >= 0; i--)
            {
                if (navMeshAgents[i] == null)
                {
                    navMeshAgents.RemoveAt(i);
                }
            }
            PlayerPrefs.SetInt("remainEnemy", navMeshAgents.Count);

            SceneManager.LoadScene("GameOver");
            Debug.Log("Game Over");
        }
        else if (hp < maXHP / 1.1f && count == 0)
        {
            count++;
            isHalfHP = true;
        }
        ShowWarningMessage();
    }

    void ShowWarningMessage()
    {
        if (isHalfHP && WarningText!=null)
        {
            WarningText.SetActive(true);
        }
    }
}
