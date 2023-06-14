using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectType
{
    Fire,
    GetItem,
    Blood,
    Death,
    Explosion
}

public class EffectSetting : MonoBehaviour
{
    public EffectType effectType;
    public float timer;

    private void OnEnable()
    {

    }


    public void Play()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(ReturnToPool());
    }
    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSecondsRealtime(1f); // 이펙트가 재생되는 동안 대기할 시간 (원하는 시간으로 변경 가능)

        PollingManager.ReturnObject(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
