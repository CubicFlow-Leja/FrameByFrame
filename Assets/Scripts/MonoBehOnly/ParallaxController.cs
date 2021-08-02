using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{ 
    public static ParallaxController Parallax { set; get; }

    public Rigidbody2D ForeGround;
    public Rigidbody2D Background;

    protected private Vector2 ParallaxVelocity;
    public float ForeGroundSpeedFactor = 0.25f;
    public float BackgroundSpeedFactor = 2.0f;

    void Start()
    {
        ParaInit();
    }

    private void ParaInit()
    {
        if (!Parallax)
            Parallax = this;
        else
            Destroy(this.gameObject);

        ParallaxVelocity = Vector2.zero;
    }


    void Update()
    {
        ForeGround.velocity = -ParallaxVelocity * ForeGroundSpeedFactor;
        Background.velocity = ParallaxVelocity * BackgroundSpeedFactor;
    }

    public void SetVelocity(Vector2 Vel)
    {
        ParallaxVelocity = Vel;
    }
}
