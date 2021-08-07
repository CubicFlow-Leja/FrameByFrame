using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{ 
    public static ParallaxController Parallax { set; get; }

    protected private Vector2 ParallaxVelocity;
    public List<ParallaxLayer> ParallaxLayers;
    private List<Vector3> ParallaxLayerPositions=new List<Vector3>();

    public Transform SpawnPosition;

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

        SetupInitialPosition();
        InitialOffset();
    }


    public void SetupInitialPosition()
    {
        foreach(ParallaxLayer lay in ParallaxLayers)
            ParallaxLayerPositions.Add(lay.Layer.transform.position);
    }

    public void InitialOffset()
    {
        foreach (ParallaxLayer lay in ParallaxLayers)
            lay.Layer.position-= lay.ParallaxSpeedFactor * lay.ParallaxDirection*(lay.Layer.position- (Vector2)SpawnPosition.position);
        
    }


    void FixedUpdate()
    {
        foreach(ParallaxLayer layer in ParallaxLayers)
            layer.Layer.velocity = ParallaxVelocity * layer.ParallaxSpeedFactor * layer.ParallaxDirection;

    }

    public void SetVelocity(Vector2 Vel)
    {
        ParallaxVelocity = Vel;
    }

    public void ResetParallax()
    {
        for (int i = 0; i < ParallaxLayers.Count; i++)
        {
            ParallaxLayers[i].Layer.position = ParallaxLayerPositions[i];
        }
        InitialOffset();
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