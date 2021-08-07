using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFloorEffect : MonoBehaviour
{
   // public int GridSize;
    public int GridSizeX;
    public int GridSizeY;

 //   public float boxsize;
    public float GridElementSize;
    public float height;
    public float DefaultHeight;
    public float Distance1;
    public float Distance2;
    public float Damp;
    //public Vector3 EffectCenter;

    public ParticleSystem system;
    private ParticleSystem.TriggerModule module;
    private ParticleSystem.Particle[] Particles;
    ParticleSystem.Particle p;

    private GameObject Obj;
    private Vector3 PositionVector;
    private Vector2 delta;

    private void Awake()
    {
        delta = Vector2.zero;
        PositionVector = Vector3.zero;
        module = system.trigger;
        Particles = new ParticleSystem.Particle[GridSizeX * GridSizeY];
        system.Play();

    }


    private void Update()
    {
        system.GetParticles(Particles);
        Vector3 positionVector = Vector3.zero;
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                p = Particles[i * GridSizeY + j];
               // p = Particles[i * GridSize + j];

                positionVector = this.transform.position - this.transform.right* GridSizeX * GridElementSize / 2 - this.transform.forward * GridSizeY * GridElementSize / 2;
                positionVector+= this.transform.right * i * GridElementSize * 1.5f + this.transform.forward*( j* GridElementSize *Mathf.Sqrt(3) + GridElementSize * (i % 2) * Mathf.Sqrt(3) / 2.0f);
                //positionVector+= this.transform.right * i * GridElementSize * 1.5f + this.transform.forward*( j* GridElementSize *Mathf.Sqrt(3) + GridElementSize * (i % 2) * Mathf.Sqrt(3) / 2.0f);

                if (Obj != null)
                {
                    delta.x = positionVector.x - Obj.transform.position.x;
                    delta.y = positionVector.z - Obj.transform.position.z;
                }
                else
                    delta = Vector2.one * 55555f;
              
                //delta.y = 0;


                if (delta.magnitude > Distance1)
                {
                    //positionVector.y = p.position.y + (DefaultHeight - p.position.y) * Time.deltaTime;
                    positionVector.y = p.position.y + (DefaultHeight + Mathf.Sin(Time.realtimeSinceStartup * j * (i - j) / Damp) * 0.44f - Mathf.Cos(-Time.realtimeSinceStartup * i * (i - j) / Damp) * 0.82f + 0.5f * Mathf.Sin(-Time.realtimeSinceStartup * i * j / Damp) - p.position.y) * Time.deltaTime;
                }
                else
                {
                    if (delta.magnitude > Distance2)
                    {
                        positionVector.y =  p.position.y+ (height - delta.magnitude + Distance2 - p.position.y- 1.3f*Mathf.Abs( Mathf.Sin(Time.realtimeSinceStartup * j * (i - j) / 500.0f))) * Time.deltaTime*5.0f;
                        //PositionVector.y =  p.position.y+ (height - delta.magnitude + Distance2 - p.position.y- 1.3f*Mathf.Sin(Time.realtimeSinceStartup * j * (i - j) / 500.0f)) * Time.deltaTime*5.0f;
                       // positionVector.y = p.position.y + (height - p.position.y - 1.3f * Mathf.Sin(Time.realtimeSinceStartup * j * (i - j) / 500.0f)) * Time.deltaTime * 5.0f;
                    }
                    else
                    {
                        positionVector.y = (height - p.position.y) * Time.deltaTime * 10.0f + p.position.y;
                        //positionVector.y = height;
                    }

                }

                p.position = positionVector;
                Particles[i * GridSizeY + j] = p;

            }
        }
        system.SetParticles(Particles,Particles.Length);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Obj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            Obj = null;
    }
}
