using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : AbstractPlatform
{
    public float SinOffset;
    public float SinSpeed;
    private float time = 0;
    public bool Alternating = false;
    public override void Move()
    {
        time += Time.fixedDeltaTime;
        if (Alternating)
            Rigid.angularVelocity = MaxSpeed * Mathf.Sin(SinSpeed * time + SinOffset);
        else
            Rigid.angularVelocity = MaxSpeed;
    }

    public override void Init()
    {
        base.Init();
    }
    public override void Reset()
    {
        base.Reset();
    }

}
