//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CanvasPoolerScript : PoolClass
//{
//    public static CanvasPoolerScript Pooler = null;

//    public PoolData PopUpData;
//    public PoolData BubbleData;
//    public List<PoolData> HpBarData;
//    public List<PoolData> DebuffEffData;


//    private int[] Indices = new int[6];
//    public Vector3 DefaultPopUpPos;
//    public float PopUpDelta;

//    private void Awake()
//    {
//        if (Pooler == null)
//            Pooler = this;
//        else
//            Destroy(this.gameObject);
//    }

//    private void Start()
//    {
//        InitiatePool(PopUpData.Pool, PopUpData.ObjToPool, PopUpData.count,this.transform.parent);
//        InitiatePool(BubbleData.Pool, BubbleData.ObjToPool, BubbleData.count, this.transform.parent);

//        foreach(PoolData pool in HpBarData)
//            InitiatePool(pool.Pool, pool.ObjToPool, pool.count, this.transform.parent);

//        foreach (PoolData pool in DebuffEffData)
//            InitiatePool(pool.Pool, pool.ObjToPool, pool.count, this.transform.parent);

//    }

   

      
//    public GameObject FetchFromBubblePool()
//    {
//        return GetFromPool(BubbleData.Pool, BubbleData.ObjToPool, this.transform.parent);
      
//    }
    
//    public GameObject FetchFromPopUpPool()
//    {
//        return GetFromPool(PopUpData.Pool, PopUpData.ObjToPool, this.transform.parent);
//    }
    
//    public GameObject FetchFromDebuffPool(int TypeOfDebuff)
//    {
//        return GetFromPool(DebuffEffData[TypeOfDebuff].Pool, DebuffEffData[TypeOfDebuff].ObjToPool,this.transform.parent);
       
//    }

//    public GameObject FetchFromHpBarPool(int TypeOfHpBar)
//    {
//        return GetFromPool(HpBarData[TypeOfHpBar].Pool, HpBarData[TypeOfHpBar].ObjToPool, this.transform.parent);

//    }

//    public void SetupPopup(string S,float _Time)
//    {
        
//        int Index = -1;
//        for (int i = 0; i < Indices.Length; i++)
//            if (Indices[i] == 0)
//            {
//                Indices[i] = 1;
//                Index = i;
//                break;
//            }

//        if (Index == -1)
//            return;

//        //fetch
//        GameObject obj = FetchFromPopUpPool();
//        obj.SetActive(true);

//        //text i pos
//        obj.transform.GetChild(0).GetComponent<Text>().text = S;
//        obj.transform.position = DefaultPopUpPos + Vector3.up * Index * PopUpDelta;
        

//        //trigger
//        obj.GetComponent<PopUpScript>().Trigger(_Time, Index);
//    }

//    //clear da se oslobodi pozicija
//    public void ClearIndex(int ind) { Indices[ind] = 0; }
//}
