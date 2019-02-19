using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectPool
{
    public string namePool;
    public GameObject objectPrefab;
    public int amountToBuffer;
    public List<GameObject> pooledObjects;
}

public class ObjectPoolingManager : MonoBehaviour {

    private int defaultBufferAmount = 0;

    public static ObjectPoolingManager Instance;

    public ObjectPool[] arrayPooledObjects;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < arrayPooledObjects.Length; i++)
        {
            arrayPooledObjects[i].pooledObjects = new List<GameObject>();

            if (arrayPooledObjects[i].amountToBuffer <= 0)
                arrayPooledObjects[i].amountToBuffer = defaultBufferAmount;

            for (int n = 0; n < arrayPooledObjects[i].amountToBuffer; n++)
            {
                GameObject newObj = Instantiate(arrayPooledObjects[i].objectPrefab, transform);
                newObj.name = arrayPooledObjects[i].namePool;
                newObj.SetActive(false);
                arrayPooledObjects[i].pooledObjects.Add(newObj);
            }
        }
    }

    public ObjectPool GetTypePool(string namePool)
    {
        ObjectPool newPool = new ObjectPool();
        newPool.namePool = namePool;
        for (int i = 0; i < arrayPooledObjects.Length; i++)
        {
            if (arrayPooledObjects[i].namePool == namePool)
                return arrayPooledObjects[i];
        }
        return newPool;
    }

    private GameObject FindAvailableObject(List<GameObject> pooledObjects)
    {
        if (pooledObjects.Count <= 0)
            return null;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }

    public GameObject GetObjectForType(string objectType,Vector3 positioncr)
    {
        GameObject obj = FindAvailableObject(GetTypePool(objectType).pooledObjects);

        if (obj != null)
        {
            obj.transform.position = positioncr;
            return obj;
        }
        else
        {
            GameObject obj_P = Instantiate(GetTypePool(objectType).objectPrefab, transform);
            obj_P.name = GetTypePool(objectType).namePool;
            GetTypePool(objectType).pooledObjects.Add(obj_P);
            obj_P.SetActive(false);
            obj_P.transform.position = positioncr;
            return obj_P;
        }
    }

    public void ResetPoolForType(string namePool)
    {
        ObjectPool currentPool = GetTypePool(namePool);
        List<GameObject> pooledObjects = currentPool.pooledObjects;
        foreach(GameObject obj in pooledObjects)
        {
            Destroy(obj);
        }
        currentPool.pooledObjects.Clear();
    }

    public void ResetAllPool()
    {
        foreach(ObjectPool p in arrayPooledObjects)
        {
            ResetPoolForType(p.namePool);
        }
    }
}
