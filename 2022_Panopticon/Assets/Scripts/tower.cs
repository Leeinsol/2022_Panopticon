using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tower : MonoBehaviour
{
    public int hp = 100;
    public GameObject towerHp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        towerHp.GetComponent<Slider>().value = hp;

        if (hp == 0)
        {
            Debug.Log("Game Over");
        }
    }
}
