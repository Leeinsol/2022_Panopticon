using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    bool isInstantiate = false;
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

            isInstantiate = true;
        }
    }
}
