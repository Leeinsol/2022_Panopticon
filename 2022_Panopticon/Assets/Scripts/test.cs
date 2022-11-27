using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public bool isGround = false;

    public float groundCheckDistance;
    float bufferCheckDistance = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
