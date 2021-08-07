using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPool : AbstractPoolClass
{
    public bool MainMenu = false;
    public static MainPool _MPool = null;

    //public PoolData FootStepDustEffect;

    public List<PoolData> OneTakeEffects;
    public List<PoolData> Projectiles;
    public List<PoolData> Explosions;

 
    private protected void Awake()
    {
        if (_MPool == null && !MainMenu)
            _MPool = this;
        else
            Destroy(this.gameObject);
    }

    private protected void Start()
    {

      //  InitiatePool(FootStepDustEffect.Pool, FootStepDustEffect.ObjToPool, FootStepDustEffect.count, null);

        foreach (PoolData pool in Projectiles)
            InitiatePool(pool.Pool, pool.ObjToPool, pool.count, null);

        foreach (PoolData pool in Explosions)
            InitiatePool(pool.Pool, pool.ObjToPool, pool.count, null);

        foreach (PoolData pool in OneTakeEffects)
            InitiatePool(pool.Pool, pool.ObjToPool, pool.count, null);
    }


    public GameObject RequestProjectile(int TypeOfProjectile)
    {
        return GetFromPool(Projectiles[TypeOfProjectile].Pool, Projectiles[TypeOfProjectile].ObjToPool, null);
    }

    public GameObject RequestExplosion(int TypeOfEffect)
    {
        return GetFromPool(Explosions[TypeOfEffect].Pool, Explosions[TypeOfEffect].ObjToPool, null);
    }


    public GameObject RequestOneTakeEffect(int Type)
    {
        return GetFromPool(OneTakeEffects[Type].Pool, OneTakeEffects[Type].ObjToPool, null);
    }
    //public GameObject RequestFootStepEffect()
    //{
    //    return GetFromPool(FootStepDustEffect.Pool, FootStepDustEffect.ObjToPool, null);
    //}

}


[System.Serializable]
public class PoolData
{
    public int count;
    public GameObject ObjToPool;
    public List<GameObject> Pool;

    public PoolData(int _Count, GameObject obj)
    {
        count = _Count;
        ObjToPool = obj;
        Pool = new List<GameObject>();
    }
}
