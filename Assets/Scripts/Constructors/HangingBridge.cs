using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode] 
public class HangingBridge : AbstractPhysicsBased
{
    public Transform ObjectEnd;
    public Transform ObjectStart;

    public GameObject SingleSegmentInstance;
    
    override protected private void CreateSegments()
    {

        KillAll();



        Vector3 SpawnDirection = (ObjectEnd.position - ObjectStart.position).normalized;
        float Distance = (ObjectEnd.position - ObjectStart.position).magnitude;
        float DistancePerSegment = Distance / (float)(NumberOfSegments - 1);

        bool once = false;
        //generiranje
        for (int i = 0; i < NumberOfSegments; i++)
        {
            float breakDist = 9999f;
            if (!once && i !=0 && i!=NumberOfSegments-1)
            {
                once = (Random.Range(0, 1f) > 0.5f);
                breakDist = BreakDistance;
            }
                
            //triba mu poziciju setat al k
            Vector3 SegmentPosition = ObjectStart.position + SpawnDirection * DistancePerSegment * i;
            GameObject BridgePart = Instantiate(SingleSegmentInstance, ObjectMain) as GameObject;
            Rigidbody Rig = BridgePart.GetComponent<Rigidbody>();
            List<Transform> CornerPosition = new List<Transform>();

            for (int j = 0; j < 4; j++)
            {
                GameObject Corner = Instantiate(CornerObj, BridgePart.transform) as GameObject;
                CornerPosition.Add(Corner.transform);
            }


            BridgeSegment _Segment;
            if (i == 0 || i == NumberOfSegments - 1)
            {
                _Segment = new CornerSegment(Rig, CornerPosition, Force, EquilibriumDist, CornerPointScale, 9999f);
                _Segment.Rigid.gameObject.layer = 0;
            }
            else
                _Segment = new CenterSegment(Rig, CornerPosition, Force, EquilibriumDist, CornerPointScale, breakDist);

            _Segment.Rigid.transform.position = SegmentPosition;
            _Segment.Rigid.transform.rotation = InitialRotationTransform.rotation;
            _Segment.Update(Force, EquilibriumDist, Scale, CornerPointScale);
            PhysicsBasedSegments.Add(_Segment);

        }


        //susidi
        for (int j = 0; j < NumberOfSegments; j++)
        {
            PhysicsBasedSegment _Segment = PhysicsBasedSegments[j];
            if (j == 0 || j == NumberOfSegments - 1)
                _Segment.GiveNeigh(null);
            else
            {
                List<Transform> Neigh = new List<Transform>();
                Neigh.Add(PhysicsBasedSegments[j - 1]._SegmentCorners[2]);
                Neigh.Add(PhysicsBasedSegments[j - 1]._SegmentCorners[3]);
                Neigh.Add(PhysicsBasedSegments[j + 1]._SegmentCorners[0]);
                Neigh.Add(PhysicsBasedSegments[j + 1]._SegmentCorners[1]);

                _Segment.GiveNeigh(Neigh);
            }
            _Segment.Update(Force, EquilibriumDist, Scale, CornerPointScale);
        }

    }



}


