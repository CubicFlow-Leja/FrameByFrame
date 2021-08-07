using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractPhysicsBased : MonoBehaviour
{
    public List<PhysicsBasedSegment> PhysicsBasedSegments = new List<PhysicsBasedSegment>();

    public Transform ObjectMain;

    public int NumberOfSegments;
    public Vector3 Scale;
    public float Force;
    public float EquilibriumDist = 2f;

    [Range(0, 1)]
    public float CornerPointScale = 0.8f;

    public float BreakDistance = 1.5f;
    public Transform InitialRotationTransform;


    public GameObject CornerObj;//promini ime
    protected void Start()
    {
        CreateSegments();
    } 

    virtual protected void FixedUpdate()
    {
        foreach (PhysicsBasedSegment Segment in PhysicsBasedSegments)
            Segment.Adjust();

        //if (NumberOfSegments != PhysicsBasedSegments.Count || PhysicsBasedSegments == null)
        //    CreateSegments();


        //UpdateSegments();

    }

    protected private abstract void CreateSegments();
    protected private void KillAll()
    {
        for (int a = PhysicsBasedSegments.Count - 1; a >= 0; a--)
        {
            PhysicsBasedSegment Segment = PhysicsBasedSegments[a];
            Segment.Rigid.gameObject.SetActive(false);
            DestroyImmediate(Segment.Rigid.gameObject);
            // BridgeSegments.RemoveAt(a);
        }
        PhysicsBasedSegments = new List<PhysicsBasedSegment>();
    }
    protected private void UpdateSegments()
    {
        foreach (PhysicsBasedSegment Segment in PhysicsBasedSegments)
            Segment.Update(Force, EquilibriumDist, Scale, CornerPointScale);
    }


}



public abstract class PhysicsBasedSegment
{
    public Rigidbody Rigid;
    public List<Transform> _SegmentCorners;
    public List<Transform> _NeightCorners;
    public float Force;
    public float EquilibriumDistance;
    public float BreakDistance;
    public abstract void Adjust();
    public abstract void Update(float _Force, float _Equi, Vector3 Scale, float CornerPointFactor);
  
    public void GiveNeigh(List<Transform> NeightCorn)
    {
        this._NeightCorners = NeightCorn;
    }


}

public class RopeSegment:PhysicsBasedSegment
{
    LineRenderer Renderer;
    int Index;
    public RopeSegment(Rigidbody _Rigid, List<Transform> SegmentCorn, float _Force, float _Equi, float CornerPointFactor, float _BreakDistance,int Ind,LineRenderer _rend)
    {
        this.Rigid = _Rigid;
        this._SegmentCorners = SegmentCorn;
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.BreakDistance = _BreakDistance;
        this.Renderer = _rend;
        this.Index = Ind;

        SegmentCorn[0].localPosition = new Vector3(0, 0.5f * CornerPointFactor, 0);
        SegmentCorn[1].localPosition = new Vector3(0, -0.5f * CornerPointFactor, 0);
    }

    //nulti neigh je za gornjeg
    //svi ostali za donjeg
    public override void Adjust()
    {
        Vector3 ForcePoint = Vector3.zero;
        Vector3 F = Vector3.zero;

        for (int i = 0; i < _SegmentCorners.Count; i++)
        {
            ForcePoint = _SegmentCorners[i].position;

            float D = (_NeightCorners[i].position - _SegmentCorners[i].position).magnitude - EquilibriumDistance;
            if (D > this.BreakDistance)
            {
                _NeightCorners.RemoveAt(i);
                _SegmentCorners.RemoveAt(i);
                i--;
                continue;
            }

            F = (_NeightCorners[i].position - _SegmentCorners[i].position).normalized * Force * D;
            this.Rigid.AddForceAtPosition(F, ForcePoint);
        }

        Renderer.SetPosition(this.Index, _SegmentCorners[1].position);
    }

    override public void Update(float _Force, float _Equi, Vector3 Scale, float CornerPointFactor)
    {
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.Rigid.transform.localScale = Scale;
        //_SegmentCorners[0].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[1].localPosition = new Vector3(0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[2].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
        //_SegmentCorners[3].localPosition = new Vector3(0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
    }

}

public class CentralConnectionSegment : PhysicsBasedSegment
{
    LineRenderer Renderer;
    int Index;
    public CentralConnectionSegment(Rigidbody _Rigid, List<Transform> SegmentCorn, float _Force, float _Equi, float CornerPointFactor, float _BreakDistance, int Ind, LineRenderer _rend)
    {
        this.Rigid = _Rigid;
        this._SegmentCorners = SegmentCorn;
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.BreakDistance = _BreakDistance;
        this.Renderer = _rend;
        this.Index = Ind;

        SegmentCorn[0].localPosition = new Vector3(0, 0.3f, 0);
      
    }



    public override void Adjust()
    {
        Vector3 ForcePoint = Vector3.zero;
        Vector3 F = Vector3.zero;

        for (int i = 0; i < _SegmentCorners.Count; i++)
        {
            ForcePoint = _SegmentCorners[i].position;
            float D = (_NeightCorners[i].position - _SegmentCorners[i].position).magnitude - EquilibriumDistance;
            if (D > this.BreakDistance)
            {
                _NeightCorners.RemoveAt(i);
                _SegmentCorners.RemoveAt(i);
                i--;
                continue;
            }

            F = (_NeightCorners[i].position - _SegmentCorners[i].position).normalized * Force * D;
            this.Rigid.AddForceAtPosition(F, ForcePoint);
        }
        Renderer.SetPosition(this.Index, Rigid.transform.position);
    }


    override public void Update(float _Force, float _Equi, Vector3 Scale, float CornerPointFactor)
    {
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.Rigid.transform.localScale = Scale;
        //_SegmentCorners[0].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[1].localPosition = new Vector3(0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[2].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
        //_SegmentCorners[3].localPosition = new Vector3(0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
    }

}

public abstract class BridgeSegment: PhysicsBasedSegment
{
    override public void Update(float _Force, float _Equi, Vector3 Scale, float CornerPointFactor)
    {
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.Rigid.transform.localScale = Scale;
        //_SegmentCorners[0].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[1].localPosition = new Vector3(0.5f * CornerPointFactor, 0, -0.5f * CornerPointFactor);
        //_SegmentCorners[2].localPosition = new Vector3(-0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
        //_SegmentCorners[3].localPosition = new Vector3(0.5f * CornerPointFactor, 0, 0.5f * CornerPointFactor);
    }


}

public class CornerSegment : BridgeSegment
{

    public CornerSegment(Rigidbody _Rigid, List<Transform> SegmentCorn, float _Force, float _Equi,float CornerPointFactor,float _BreakDistance)
    {
        this.Rigid = _Rigid;
        this._SegmentCorners = SegmentCorn;
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.BreakDistance = _BreakDistance;

        this.Rigid.constraints = RigidbodyConstraints.FreezeAll;
        SegmentCorn[0].localPosition = new Vector3(-0.5f* CornerPointFactor, 0.4f, -0.5f* CornerPointFactor);
        SegmentCorn[1].localPosition = new Vector3(0.5f* CornerPointFactor, 0.4f, -0.5f* CornerPointFactor);
        SegmentCorn[2].localPosition = new Vector3(-0.5f* CornerPointFactor, 0.4f, 0.5f* CornerPointFactor);
        SegmentCorn[3].localPosition = new Vector3(0.5f* CornerPointFactor, 0.4f, 0.5f* CornerPointFactor);
    }

    public override void Adjust()
    {
        //miruje zasad i tjt
    }

}

public class CenterSegment : BridgeSegment
{
    public CenterSegment(Rigidbody _Rigid, List<Transform> SegmentCorn, float _Force, float _Equi, float CornerPointFactor, float _BreakDistance)
    {
        this.Rigid = _Rigid;
        this._SegmentCorners = SegmentCorn;
        this.Force = _Force;
        this.EquilibriumDistance = _Equi;
        this.BreakDistance = _BreakDistance;

        SegmentCorn[0].localPosition = new Vector3(-0.5f * CornerPointFactor, 0.4f, -0.5f * CornerPointFactor);
        SegmentCorn[1].localPosition = new Vector3(0.5f * CornerPointFactor, 0.4f, -0.5f * CornerPointFactor);
        SegmentCorn[2].localPosition = new Vector3(-0.5f * CornerPointFactor, 0.4f, 0.5f * CornerPointFactor);
        SegmentCorn[3].localPosition = new Vector3(0.5f * CornerPointFactor, 0.4f, 0.5f * CornerPointFactor);
    }



    public override void Adjust()
    {
        Vector3 ForcePoint = Vector3.zero;
        Vector3 F = Vector3.zero;

        for (int i = 0; i < _SegmentCorners.Count; i++)
        {
            ForcePoint = _SegmentCorners[i].position;
            float D = (_NeightCorners[i].position - _SegmentCorners[i].position).magnitude - EquilibriumDistance;
            if (D > this.BreakDistance)
            {
                _NeightCorners.RemoveAt(i);
                _SegmentCorners.RemoveAt(i);
                i--;
                continue;
            }

            F = (_NeightCorners[i].position - _SegmentCorners[i].position).normalized * Force * D;
            this.Rigid.AddForceAtPosition(F, ForcePoint);
        }
    }
}

