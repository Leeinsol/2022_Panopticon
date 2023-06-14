using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollingManager : MonoBehaviour
{
    public static PollingManager Instance;
    [SerializeField]
    private GameObject fireEffectPrefab;

    [SerializeField]
    private GameObject getItemEffectPrefab;

    [SerializeField]
    private GameObject BloodEffectPrefab;

    [SerializeField]
    private GameObject DeathEffectPrefab;

    [SerializeField]
    private GameObject ExplosionEffectPrefab;

    Queue<EffectSetting> poolingObjectQueue = new Queue<EffectSetting>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Initialize(5);
    }

   void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            EffectType effectType = (EffectType)(i % initCount);
            poolingObjectQueue.Enqueue(CreateNewObject(effectType));
        }
    }
    private GameObject GetEffectPrefab(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Fire:
                return fireEffectPrefab;
            case EffectType.GetItem:
                return getItemEffectPrefab;
            case EffectType.Blood:
                return BloodEffectPrefab;
            case EffectType.Death:
                return DeathEffectPrefab;
            case EffectType.Explosion:
                return ExplosionEffectPrefab;
            default:
                return fireEffectPrefab;
        }
    }

    private EffectSetting CreateNewObject(EffectType effectType)
    {
        GameObject effectPrefab = GetEffectPrefab(effectType);
        var newObj = Instantiate(effectPrefab).GetComponent<EffectSetting>();
        newObj.effectType = effectType;
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static EffectSetting GetObject(EffectType effectType)
    {
        foreach(var obj in Instance.poolingObjectQueue)
        {
            if (obj.effectType == effectType)
            {
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        var newObj = Instance.CreateNewObject(effectType);
        newObj.gameObject.SetActive(true);
        newObj.transform.SetParent(null);
        return newObj;
    }

    public static void ReturnObject(EffectSetting obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}