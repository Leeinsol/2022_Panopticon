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
        yield return new WaitForSecondsRealtime(1f); // ����Ʈ�� ����Ǵ� ���� ����� �ð� (���ϴ� �ð����� ���� ����)

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
