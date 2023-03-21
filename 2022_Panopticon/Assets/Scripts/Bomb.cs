using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float bombSpeed;

    public GameObject meshObj;
    public GameObject effectObj;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = transform.position * bombSpeed;
        rigidbody.AddForce(transform.forward * bombSpeed);
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Destroy(gameObject, 1f);
        }
    }
}
