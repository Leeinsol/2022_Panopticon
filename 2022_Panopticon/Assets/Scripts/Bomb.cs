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
    public AudioClip BombSound,explosionSound;
    AudioSource audioSource;

    public RaycastHit[] rayHits = new RaycastHit[0];
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

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

        float scaleTime = 0.5f;
        float scaleStartTime = Time.time;
        while (Time.time < scaleStartTime + scaleTime)
        {
            float t = (Time.time - scaleStartTime) / scaleTime;
            Vector3.Lerp(Vector3.zero, new Vector3(blastRadius * 2f, blastRadius * 2f, blastRadius * 2f), t);
            yield return null;
        }
        rayHits = Physics.SphereCastAll(transform.position, blastRadius, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        var effect = PollingManager.GetObject(EffectType.Explosion);
        effect.transform.position = transform.position;
        effect.Play();
        //effectObj.gameObject.transform.Find("ExplosionEffect").gameObject.SetActive(true);
        PlaySoundEffects(explosionSound);

        foreach (RaycastHit hitObj in rayHits)
        {
            Enemy enemy = hitObj.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitByBomb(transform.position);
            }
        }

        Destroy(gameObject, 1f);
    }
    void PlaySoundEffects(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
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
            PlaySoundEffects(BombSound);
        }
    }
}
