using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPoolClass : MonoBehaviour
{

    public virtual void InitiatePool(List<GameObject> poolToinit, GameObject ObjectToPool, int Count,Transform parent)
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject p = Instantiate(ObjectToPool, Vector3.zero, Quaternion.identity, parent) as GameObject;
            p.SetActive(false);
            AddToPool(poolToinit, p);
        }
    }

    public virtual GameObject GetFromPool(List<GameObject> PoolToFetchFrom, GameObject ObjectToPool, Transform parent)
    {
        for (int i = 0; i < PoolToFetchFrom.Count; i++)
            if (!PoolToFetchFrom[i].activeSelf)
                return PoolToFetchFrom[i];

        GameObject p = Instantiate(ObjectToPool, Vector3.zero, Quaternion.identity, parent) as GameObject;
        p.SetActive(false);
        AddToPool(PoolToFetchFrom, p);
        return p;
    }


    public virtual void AddToPool(List<GameObject> poolToinit, GameObject P)
    {
        poolToinit.Add(P);
    }

}


