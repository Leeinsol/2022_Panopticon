using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tower : MonoBehaviour
{
    private int maXHP = 10000;
    public int hp = 10000;
    public GameObject towerHp;

    public bool isHalfHP = false;

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
        else if (hp < maXHP / 2)
        {
            isHalfHP = true;
        }
    }
}
