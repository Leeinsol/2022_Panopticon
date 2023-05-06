using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject Bomb;
    public GameObject energyDrink;

    public bool useRandomEnergyDrink = true;
    public int enertDrinkNum=10;

    float circleRadius = 5f;
    //float gravity = 9.8f;
    //float minLaunchAngle = 30f;
    //float maxLaunchAngle = 60f;
    //float launchSpeed = 10f;

    float maxRadius = 23f;
    float minRadius = 10f;

    public bool isDistanceCorrection = true;

    // Start is called before the first frame update
    void Start()
    {
        if (useRandomEnergyDrink)
        {
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log(i);
                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
                //Debug.Log(randomPoint);
                //Vector2 towerPoint = Random.insideUnitCircle * 7f;

                while (randomPoint.magnitude <= minRadius)
                {
                    randomPoint = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
                }

                Instantiate(energyDrink, new Vector3(randomPoint.x, 0, randomPoint.y), Quaternion.identity);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateItem(Vector3 position)
    {

        //Debug.Log("instantiateItem");

        //Vector2 randomPoint = Random.insideUnitCircle * circleRadius;
        //Debug.Log(randomPoint.magnitude);

        ////while(randomPoint.magnitude <= 10f)
        ////{
        ////    randomPoint = Random.insideUnitCircle * circleRadius;
        ////}

        //Instantiate(Bomb, position + new Vector3(randomPoint.x, 0f, randomPoint.y), Quaternion.identity);

        Vector2 randomPoint;
        Vector3 vector3;
        if (isDistanceCorrection)
        {
            do
            {
                randomPoint = Random.insideUnitCircle * circleRadius;
                vector3 = position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            } while (vector3.magnitude < 10f);
            Instantiate(Bomb, position + new Vector3(randomPoint.x, 0f, randomPoint.y), Quaternion.identity);

        }
        else
        {
            randomPoint = Random.insideUnitCircle * circleRadius;
            Instantiate(Bomb, position + new Vector3(randomPoint.x, 0f, randomPoint.y), Quaternion.identity);
        }

    }
}
