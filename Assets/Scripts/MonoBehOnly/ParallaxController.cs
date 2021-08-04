using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{ 
    public static ParallaxController Parallax { set; get; }

    protected private Vector2 ParallaxVelocity;
    public List<ParallaxLayer> ParallaxLayers;
    Vector2 tempVel = Vector2.zero;
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


    
    void FixedUpdate()
    {
        foreach(ParallaxLayer layer in ParallaxLayers)
        {
            tempVel.x=ParallaxVelocity.x * layer.ParallaxSpeedFactor * layer.ParallaxDirection; 
            tempVel.y=ParallaxVelocity.y * layer.ParallaxSpeedFactor;
            layer.Layer.velocity = tempVel;
        }
    }

    public void SetVelocity(Vector2 Vel)
    {
        ParallaxVelocity.x = Vel.x;
        ParallaxVelocity.y = -Vel.y;
    }

    public void ResetParallax()
    {
        foreach (ParallaxLayer layer in ParallaxLayers)
        {
            layer.Layer.transform.position = Vector3.zero;
        }
    }
}


[System.Serializable]
public struct ParallaxLayer
{
    public Rigidbody2D Layer;
    [Range(0f,6f)]
    public float ParallaxSpeedFactor;
    [Range(-1,1)]
    public int ParallaxDirection;
}