using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTarget : AbstractPlatform
{
 
    private Vector3 SpeedDir = Vector3.zero;
    public float ThreshHold = 1f;
    public float SpeedGravity = 1f;

    public Transform Target1;
    public Transform Target2;
    private bool Switch = false;

    public override void Move()
    {
        if (Switch)
        {
            SpeedDir += (Target2.transform.position - this.transform.position - SpeedDir) * Time.fixedDeltaTime * SpeedGravity;
            if ((Target2.transform.position - this.transform.position).magnitude < ThreshHold)
                Switch = false;
        }
        else
        {
            SpeedDir += (Target1.transform.position - this.transform.position - SpeedDir) * Time.fixedDeltaTime * SpeedGravity;

            if ((Target1.transform.position - this.transform.position).magnitude < ThreshHold)
                Switch = true;
        }


        Rigid.velocity = SpeedDir * MaxSpeed;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Reset()
    {
        base.Reset();

        SpeedDir = Vector3.zero;
        Switch = false;
    }

}

