using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    Rigidbody rigidbody;
    public float bombSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = transform.position * bombSpeed;
        GetComponent<Rigidbody>().AddForce(transform.forward * bombSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Destroy(gameObject, 1f);
        }
    }
}
