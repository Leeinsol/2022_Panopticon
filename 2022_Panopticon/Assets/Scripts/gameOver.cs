using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOver : MonoBehaviour
{
    public GameObject remainEnemyText;

    // Start is called before the first frame update
    void Start()
    {
        remainEnemyText.GetComponent<Text>().text = PlayerPrefs.GetInt("remainEnemy").ToString();
        Debug.Log(PlayerPrefs.GetInt("remainEnemy") / (float)PlayerPrefs.GetInt("AllEnemy"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
