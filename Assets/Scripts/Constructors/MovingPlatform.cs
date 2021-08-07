using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : AbstractPlatform
{
    public float SinOffset;
    public float SinSpeed;
    public Vector2 SpeedDirection;
    private Vector2 SpeedDir;
    private float time = 0;

   
    public override void Move()
    {
        time += Time.fixedDeltaTime;
        SpeedDir = (SpeedDirection.x * this.transform.right + SpeedDirection.y * this.transform.up).normalized;
        Rigid.velocity = SpeedDir* MaxSpeed * Mathf.Sin(SinSpeed * time + SinOffset);
    }


    public override void Init()
    {
        base.Init();
    }

    public override void Reset()
    {
        base.Reset();
        SpeedDir = Vector3.zero;
        time = 0;
    }

}
