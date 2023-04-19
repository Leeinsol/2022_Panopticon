using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    Rigidbody rigidbody;
    //public float bombSpeed;

    public GameObject meshObj;
    public GameObject effectObj;

    public RaycastHit[] rayHits = new RaycastHit[0];
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        float blastRadius = 3f;
        //GameObject blastRadiusObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //blastRadiusObj.transform.position = transform.position;
        //blastRadiusObj.transform.localScale = new Vector3(blastRadius * 2f, blastRadius * 2f, blastRadius * 2f);
        //Destroy(blastRadiusObj.GetComponent<Collider>());
        //Material blastRadiusMat = new Material(Shader.Find("UI/Unlit/Transparent"));
        //blastRadiusMat.color = new Color(.35f, .1f, .1f, 0.3f);
        //blastRadiusObj.GetComponent<MeshRenderer>().material = blastRadiusMat;

        float scaleTime = 0.5f;
        float scaleStartTime = Time.time;
        while (Time.time < scaleStartTime + scaleTime)
        {
            float t = (Time.time - scaleStartTime) / scaleTime;
            //blastRadius = Mathf.Lerp(0f, 5f, t);
            //blastRadiusObj.transform.localScale = new Vector3(blastRadius, blastRadius, blastRadius);
            Vector3.Lerp(Vector3.zero, new Vector3(blastRadius * 2f, blastRadius * 2f, blastRadius * 2f), t);
            yield return null;
        }
        //blastRadiusObj.transform.localScale = new Vector3(5f, 5f, 5f);
        //blastRadiusObj.transform.localScale = new Vector3(blastRadius * 2f, blastRadius * 2f, blastRadius * 2f);

        //Destroy(blastRadiusObj, 1f);

        rayHits = Physics.SphereCastAll(transform.position, blastRadius, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        //Debug.Log("rayHits: " + rayHits.Length);

        effectObj.gameObject.transform.Find("ExplosionEffect").gameObject.SetActive(true);
        foreach (RaycastHit hitObj in rayHits)
        {
            Enemy enemy = hitObj.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitByBomb(transform.position);
            }
            //enemy.GetComponent<Enemy>().isDamaged = false;

            //hitObj.transform.GetComponent<Enemy>().HitByBomb(transform.position);

        }

        Destroy(gameObject, 1f);

        //Invoke("DestroyBomb", 1f);
    }

    void DestroyBomb()
    {
        effectObj.gameObject.transform.Find("ExplosionEffect").gameObject.SetActive(true);
        Debug.Log(effectObj.gameObject.transform.Find("ExplosionEffect").gameObject.activeSelf);
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
