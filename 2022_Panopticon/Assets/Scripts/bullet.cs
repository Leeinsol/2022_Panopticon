using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public static float moveSpeed = 7f;
    public float lifeTime = 1000f;
    Transform firePos;
    Vector3 dir;
    Ray ray;

    public GameObject shootPos;
    // Start is called before the first frame update
    void Start()
    {
        firePos = shootPos.transform;
        dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        ray = new Ray(firePos.position, dir);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //lifeTime = lifeTime - Time.deltaTime * 1000;

        //if (lifeTime <= 0)
        //{
        //    Destroy(gameObject);
        //}
        transform.position += ray.direction * 50 * Time.deltaTime;

    }

}
