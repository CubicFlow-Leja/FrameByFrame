using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : AbstractPhysicsBased
{
   
    public GameObject RopePartInstance;

    public Transform ObjectEnd;
    public Transform ObjectStart;

    public LineRenderer Rend;
    override protected private void CreateSegments()
    {
        KillAll();


        Vector3 SpawnDirection = (ObjectEnd.position - ObjectStart.position).normalized;
        float Distance = (ObjectEnd.position - ObjectStart.position).magnitude;
        float DistancePerSegment = Distance / (float)(NumberOfSegments - 1);

        //konop
        for (int i = 0; i < NumberOfSegments - 1; i++)
        {
            Vector3 SegmentPosition = ObjectStart.position + SpawnDirection * DistancePerSegment * i;


            GameObject RopePart = Instantiate(RopePartInstance, ObjectMain) as GameObject;
            Rigidbody _Rig = RopePart.GetComponent<Rigidbody>();
            List<Transform> RopeEdgeTransforms = new List<Transform>();

            for (int a = 0; a < 2; a++)
            {
                GameObject Edge = Instantiate(CornerObj, RopePart.transform) as GameObject;
                RopeEdgeTransforms.Add(Edge.transform);
            }
            RopeSegment _RopeSegment;

            _RopeSegment = new RopeSegment(_Rig, RopeEdgeTransforms, Force, EquilibriumDist, CornerPointScale, 9999f, i, Rend);

            _RopeSegment.Rigid.transform.position = SegmentPosition;
            _RopeSegment.Rigid.transform.rotation = InitialRotationTransform.rotation;
            _RopeSegment.Update(Force, EquilibriumDist, Scale, CornerPointScale);
            PhysicsBasedSegments.Add(_RopeSegment);

            //if (i == NumberOfSegments - 1)
            //    PlatformNeighbours.Add(_RopeSegment._SegmentCorners[1]);
        }


        //susidi
        for (int b = 0; b < NumberOfSegments - 1; b++)
        {
            PhysicsBasedSegment _PhysSegment = PhysicsBasedSegments[b];

            List<Transform> Neigh = new List<Transform>();
            if (b == 0)
            {
                Neigh.Add(ObjectStart.transform);
                Neigh.Add(PhysicsBasedSegments[b + 1]._SegmentCorners[0]);
            }
            else
            {
                if (b == NumberOfSegments - 2)
                {
                    Neigh.Add(PhysicsBasedSegments[b - 1]._SegmentCorners[1]);
                    Neigh.Add(ObjectEnd.transform);
                }
                else
                {
                    Neigh.Add(PhysicsBasedSegments[b - 1]._SegmentCorners[1]);
                    Neigh.Add(PhysicsBasedSegments[b + 1]._SegmentCorners[0]);
                }
            }

            _PhysSegment.GiveNeigh(Neigh);
            _PhysSegment.Update(Force, EquilibriumDist, Scale, CornerPointScale);
        }


        //kraj


        Vector3 PlatformPosition = ObjectEnd.position;


        GameObject PlatformObj = Instantiate(RopePartInstance, ObjectMain) as GameObject;
        Rigidbody Rigid = PlatformObj.GetComponent<Rigidbody>();
        List<Transform> Platformedges = new List<Transform>();


        GameObject _Edge = Instantiate(CornerObj, PlatformObj.transform) as GameObject;
        Platformedges.Add(_Edge.transform);

        CentralConnectionSegment Platform;

        Platform = new CentralConnectionSegment(Rigid, Platformedges, Force, EquilibriumDist, CornerPointScale, 9999f, NumberOfSegments-1, Rend);

        Platform.Rigid.transform.position = PlatformPosition;
        Platform.Rigid.transform.rotation = InitialRotationTransform.rotation;
        Platform.Update(Force, EquilibriumDist, Scale, CornerPointScale);
        PhysicsBasedSegments.Add(Platform);

        //if (i == NumberOfSegments - 1)
        //    PlatformNeighbours.Add(_RopeSegment._SegmentCorners[1]);


        List<Transform> Neighb = new List<Transform>();
        Neighb.Add(PhysicsBasedSegments[NumberOfSegments - 2]._SegmentCorners[1]);
        Platform.GiveNeigh(Neighb);
        Platform.Update(Force, EquilibriumDist, Scale, CornerPointScale);


        PhysicsBasedSegments[NumberOfSegments - 2]._NeightCorners[1] = Platform._SegmentCorners[0];



        Rend.positionCount = NumberOfSegments;
        Rend.startWidth = Scale.x;
        Rend.endWidth = Scale.x;
    }
}
