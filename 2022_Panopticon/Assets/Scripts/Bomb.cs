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
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = transform.position * bombSpeed;
        //rigidbody.AddForce(transform.forward * bombSpeed);
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

        GameObject blastRadiusObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        blastRadiusObj.transform.position = transform.position;
        blastRadiusObj.transform.localScale = Vector3.zero;
        Destroy(blastRadiusObj.GetComponent<Collider>());
        Material blastRadiusMat = new Material(Shader.Find("Standard"));
        blastRadiusMat.color = new Color(1f, 0.5f, 0f, 0.5f);
        blastRadiusObj.GetComponent<MeshRenderer>().material = blastRadiusMat;

        float scaleTime = 0.5f;
        float scaleStartTime = Time.time;
        while (Time.time < scaleStartTime + scaleTime)
        {
            float t = (Time.time - scaleStartTime) / scaleTime;
            float blastRadius = Mathf.Lerp(0f, 5f, t);
            blastRadiusObj.transform.localScale = new Vector3(blastRadius, blastRadius, blastRadius);
            yield return null;
        }
        blastRadiusObj.transform.localScale = new Vector3(5f, 5f, 5f);
        Destroy(blastRadiusObj, 1f);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByBomb(transform.position);

        }
        Destroy(gameObject,1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            //Destroy(gameObject, 1f);
        }
    }
}
