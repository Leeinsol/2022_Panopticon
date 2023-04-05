using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject Bomb;

    bool isInstantiate = false;
    float circleRadius = 5f;
    float gravity = 9.8f;
    float minLaunchAngle = 30f;
    float maxLaunchAngle = 60f;
    float launchSpeed = 10f;

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
            //Debug.Log("instantiateItem");

            //Gizmos.color = Color.white;
            //Gizmos.DrawWireSphere(position, 2f);

            //float randomAngle = Random.Range(0f, 1f * Mathf.PI);
            //float randomDistance = Random.Range(0f, circleRadius/2f);

            ////Vector2 randomPoint = Random.insideUnitCircle * circleRadius;
            //Vector3 randomPosition = position + new Vector3(randomDistance * Mathf.Cos(randomAngle), Random.Range(1f, 2f), randomDistance * Mathf.Sin(randomAngle));

            //GameObject Item =Instantiate(Bomb, randomPosition, Quaternion.identity);

            //Rigidbody rb = Item.AddComponent<Rigidbody>();

            //float launchAngle = Random.Range(minLaunchAngle, maxLaunchAngle);
            //float launchVelocity = launchSpeed / Mathf.Cos(launchAngle * Mathf.Deg2Rad);
            //Vector3 launchDirection = new Vector3(Mathf.Sin(launchAngle * Mathf.Deg2Rad), Mathf.Cos(launchAngle * Mathf.Deg2Rad), 0f);
            //Vector3 launchVelocityVector = launchDirection * launchVelocity;

            //rb.velocity = launchVelocityVector;

            //rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

            //isInstantiate = true;
            //Vector3 randomPoint = Random.insideUnitCircle * circleRadius;
            //Vector3 launchDirection = (randomPoint - position).normalized;
            //float launchAngle = Random.Range(minLaunchAngle, maxLaunchAngle);
            //float launchVelocity = launchSpeed / Mathf.Cos(launchAngle * Mathf.Deg2Rad);
            //Vector3 launchVelocityVector = launchDirection * launchVelocity;

            //GameObject item = Instantiate(Bomb, position, Quaternion.identity);
            //Rigidbody rb = item.AddComponent<Rigidbody>();

            //Vector3 gravityVector = new Vector3(0f, -gravity, 0f);
            //rb.velocity = launchVelocityVector + gravityVector * launchVelocity / gravity;
            //rb.useGravity = false;
            //rb.AddForce(gravityVector, ForceMode.Acceleration);

            //isInstantiate = true;


            //Debug.Log("instantiateItem");

            Vector2 randomPoint = Random.insideUnitCircle * circleRadius;

            Instantiate(Bomb, position + new Vector3(randomPoint.x, 0f, randomPoint.y), Quaternion.identity);


            isInstantiate = true;
        }
    }
}
