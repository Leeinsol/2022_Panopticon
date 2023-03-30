using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject Bomb;

    bool isInstantiate = false;
    float circleRadius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateItem(Vector3 position)
    {
        if (!isInstantiate)
        {
            Debug.Log("instantiateItem");

            //Gizmos.color = Color.white;
            //Gizmos.DrawWireSphere(position, 2f);

            Vector2 randomPoint = Random.insideUnitCircle * circleRadius;

            Instantiate(Bomb, position + new Vector3(randomPoint.x, 0f, randomPoint.y), Quaternion.identity);


            isInstantiate = true;
        }
    }
}
