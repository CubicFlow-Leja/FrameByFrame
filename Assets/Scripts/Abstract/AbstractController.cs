using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AbstractController : MonoBehaviour
{
    
    protected private float yFactor = 0;
    protected private float JumpCharged=0;
    protected private Vector2 ForceVector=Vector2.zero;
    protected private Vector2 ForceVectorJmp=Vector2.zero;
    protected private RaycastHit2D hit;
    protected private Vector2 HitNormal=Vector2.up;
    protected private Vector2 HitTangent=Vector2.right;
   // protected private float DirectionParameter=1;
    protected private bool Grounded=false;
    protected private InputStruct Inputs;
   // protected private Collider2D PawnCollider;
    protected private Rigidbody2D PawnRigidBody;
   // protected private int AnimationState=0;
    public AbstractInputController ParentController;
    protected private Vector3 scale = Vector3.one;

    [Header("CharacterParams")]
    public Animator Ani;
    public GameObject Character;
    public GameObject Model;
    public LayerMask CollissionMask;
    public Stats CharacterStats;

    [Header("Grounded")]
    [Range(0, 2)]
    public float RaycastMaxLength = 1f;
    [Range(0, 15)]
    public int RayCastResolution=4;
    public GameObject DropShadow;
    public float DropShadowMax = 25f;
    public float DropShadowMaxX = 7.5f;
    public float DropShadowMaxY = 1.5f;
    private float DropShadowAlpha = 1.0f;
    public Vector3 DropShadowScale = Vector3.one;
    public Vector3 DirectionLight = Vector3.up;
    public virtual void AsignInput(InputStruct InputMain)
    {
        Inputs = InputMain;
    }

    protected private abstract void CalculateAnimationData();
   

    protected private void HandleInputs(InputStruct _Input) 
    { 
        Inputs.Dir = _Input.Dir;
        Inputs.JumpPressed = _Input.JumpPressed;
       
    }


    public virtual void InitPawn(AbstractInputController Parent)
    {
        PawnRigidBody = GetComponent<Rigidbody2D>();
        ParentController = Parent;
        DropShadow.transform.parent = null;
    }

    //public virtual void SetAnimationStates()
    //{
    //    Ani.SetInteger("State", AnimationState);
    //}

  
    public void MovementLoop()
    {
        HandleInputs(Inputs);
        CheckGrounded();
        CalculateMovement();
        CalculateAnimationData();
   //     SetAnimationStates();
        MoveCharacter();
    }


   

    protected private void CheckGrounded()
    {
       
        Vector3 StartVec;
        Grounded = false;
       
        for (int i = 0; i <= RayCastResolution; i++)
        {
            StartVec = this.transform.position + this.transform.right * i / RayCastResolution * CharacterStats.PawnWidth / 2;
            hit = Physics2D.Raycast(StartVec, -this.transform.up, RaycastMaxLength * scale.y, CollissionMask);

            if (hit.collider!= null)
            {
               // Debug.DrawLine(StartVec, hit.point, Color.red);

                Grounded = true;
               // break;
            }

            if(i!=0)
            {
                StartVec = this.transform.position  - this.transform.right * i / RayCastResolution * CharacterStats.PawnWidth / 2;
                hit = Physics2D.Raycast(StartVec, -this.transform.up, RaycastMaxLength * scale.y, CollissionMask);

                if (hit.collider != null)
                {
                   // Debug.DrawLine(StartVec, hit.point, Color.red);

                    Grounded = true;
                    //break;
                }

            }

        }

        if(Grounded)
        {
            //SlopeAngle = Vector3.Angle(HitNormal, Vector3.up);
            //if (SlopeAngle >= 90.0f)
            //    SlopeAngle = 0.0f;

            float x = Vector3.Dot(HitNormal, Vector3.right);
            float y = Vector3.Dot(HitNormal, Vector3.up);
            HitTangent.x = y;
            HitTangent.y = -x;
           // Debug.DrawLine(Character.transform.position, this.transform.position + (Vector3)HitTangent * 1, Color.yellow);

           // DirectionParameter = Mathf.Sign(Vector3.Dot(PawnRigidBody.velocity,Vector3.right));
        }


        hit = Physics2D.Raycast(this.transform.position, -DirectionLight, DropShadowMax, CollissionMask);
        if(hit.collider!=null)
        {
            float DistFactor = 1f-(hit.point - (Vector2)this.transform.position).magnitude/DropShadowMax;
            DropShadow.transform.position = hit.point;

            DropShadowScale.x = DropShadowMaxX * DistFactor;
            DropShadowScale.y = DropShadowMaxY * DistFactor;

            DropShadow.transform.localScale = DropShadowScale;
            DropShadowAlpha = DistFactor;
            DropShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, DropShadowAlpha*0.5f);
            DropShadow.transform.up = hit.normal;

        }
        else
        {
            DropShadowScale.x += (0f - DropShadowScale.x)*Time.fixedDeltaTime*5.0f ;
            DropShadowScale.y += (0f - DropShadowScale.y)*Time.fixedDeltaTime*5.0f ;

            DropShadow.transform.localScale = DropShadowScale;

            DropShadowAlpha += (0 - DropShadowAlpha) * Time.fixedDeltaTime * 5f;

            DropShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, DropShadowAlpha);
        }

       


    }

    protected private bool Jmp=false;

    protected private void Jump()
    {
        JumpCharged = Mathf.Clamp01(JumpCharged);
        float Param = Mathf.Clamp01(1f - scale.y) / CharacterStats.MaxStretchFactor;
        PawnRigidBody.AddForce(Character.transform.up * Param * JumpCharged  * CharacterStats.MaxJumpForce);
        JumpCharged = 0;
        RelativeVector = Vector3.zero;
        Jmp = false;
    }
    protected private void CalculateMovement()
    {
        ForceVectorJmp = Vector2.zero;
        ForceVector = Vector2.zero;

        if (Grounded)
        {
            if (Inputs.JumpPressed)
            {
               
                JumpCharged += (2f - JumpCharged) * Time.fixedDeltaTime * CharacterStats.JumpChargeGravity;
                Jmp = true;

                if (JumpCharged >= 1f)
                    Debug.Log("JUMPMAX");

                if (JumpCharged >= 1.9f)
                    Jump();
            }
            else
            {
                if (Jmp)
                    Jump();
                else
                    JumpCharged += (0f - JumpCharged) * Time.fixedDeltaTime * CharacterStats.JumpChargeGravity;
            }
               
            yFactor += (0 - yFactor) * Time.fixedDeltaTime * CharacterStats.AirStretchFactor;
            this.transform.up = Vector3.RotateTowards(Character.transform.up, HitNormal, CharacterStats.NormalAlignSpeed * Time.fixedDeltaTime, 1.0f);
            Vector2 vecInput = Inputs.Dir.x * HitTangent;
            ForceVector = vecInput*CharacterStats.MaxForce;
        }
        else
        {
            yFactor += (Mathf.Clamp01(Mathf.Clamp( Mathf.Abs(PawnRigidBody.velocity.y),0,CharacterStats.AirIncrementFactor) / CharacterStats.AirIncrementFactor) - yFactor) * Time.fixedDeltaTime * CharacterStats.AirStretchFactor;
            JumpCharged += (0f - JumpCharged) * Time.fixedDeltaTime * CharacterStats.JumpChargeGravity * 10f;
            this.transform.up = Vector3.RotateTowards(Character.transform.up, ((PawnRigidBody.velocity).normalized * Mathf.Sign(PawnRigidBody.velocity.y)+HitNormal * 1.5f ).normalized , CharacterStats.AirAlignSpeed * Time.fixedDeltaTime, 1.0f);
            // Character.transform.up = Vector3.RotateTowards(Character.transform.up, (PawnRigidBody.velocity.normalized*Mathf.Sign(PawnRigidBody.velocity.y)).normalized,CharacterStats.NormalAlignSpeed * Time.fixedDeltaTime, 1.0f); 
            //  Character.transform.up = Vector3.RotateTowards(Character.transform.up, Vector2.up,CharacterStats.NormalAlignSpeed* Time.fixedDeltaTime, 1.0f); 
            HitNormal = Vector3.RotateTowards(HitNormal, Vector3.up, Time.fixedDeltaTime * CharacterStats.AirAlignSpeed, 1.0f);


            RotaDirectionFactor += (0f - RotaDirectionFactor) * Time.fixedDeltaTime * CharacterStats.PlatformVectorFactor;
          //  RelativeVectorPlatform += (Vector3.zero - RelativeVectorPlatform) * Time.fixedDeltaTime * CharacterStats.PlatformVectorFactor;
        }

        float tempY =Mathf.Clamp( Mathf.Abs(Vector3.Dot(Character.transform.up, RelativeVector)),0.2f,CharacterStats.SpeedStretchFactor);
        float tempX = Mathf.Clamp(Mathf.Abs(Vector3.Dot(Character.transform.right, RelativeVector)), 0.2f, CharacterStats.SpeedStretchFactor);

        float temp = 1f - CharacterStats.MaxStretchFactor * tempY / CharacterStats.SpeedStretchFactor
                     + CharacterStats.MaxStretchFactor * tempX / CharacterStats.SpeedStretchFactor
                     - JumpCharged * CharacterStats.MaxStretchFactor/2f + (CharacterStats.MaxStretchFactor * yFactor);

        scale.y = Mathf.Clamp((temp * CharacterStats.IncrementFactor) / CharacterStats.IncrementFactor, CharacterStats.MaxStretchFactor, 20f);
        //scale.y = Mathf.Clamp(Mathf.RoundToInt(temp * CharacterStats.IncrementFactor) / CharacterStats.IncrementFactor, CharacterStats.MaxStretchFactor, 20f);
        scale.x = 2f - scale.y;
        //scale.x = Mathf.Abs(scale.x) * Mathf.Sign(DirectionParameter);
        this.GetComponent<CircleCollider2D>().radius = scale.y*0.75f;
        this.GetComponent<CapsuleCollider2D>().size = new Vector2(1.125f + 0.75f*(0.5f/scale.y), 0.35f);
        Character.transform.localScale = scale;

        RelativeVector +=(Vector3.zero- RelativeVector)* Time.fixedDeltaTime * CharacterStats.StretchFactor;


        //Debug.DrawLine(this.transform.position, this.transform.position +(Vector3)RelativeVectorPlatform);
    }



    protected private  void MoveCharacter()
    {
        PawnRigidBody.AddForce(ForceVector+ ForceVectorJmp);
    }
    private void FixedUpdate()
    {
        MovementLoop();
    }


    protected private Vector3 RelativeVector = Vector3.zero;
    protected private Vector3 RelativeVectorPlatform = Vector3.zero;
    //protected private Vector2 PlatformForceVector = Vector2.zero;
    protected private float RotaDirectionFactor = 0f;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (Vector3.Dot(Vector3.up, collision.GetContact(0).normal) > 0)
            HitNormal = collision.GetContact(0).normal;
        else
            HitNormal = Vector3.up;

        yFactor = 0;

        Vector3 Temp = collision.GetContact(0).relativeVelocity;
        Temp = collision.GetContact(0).normal * Vector3.Dot(Temp, collision.GetContact(0).normal);
        RelativeVector += Temp;
      //  Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + (Vector2)RelativeVector/15, Color.black, 1f);

       
        RelativeVectorPlatform= collision.GetContact(0).relativeVelocity - (Vector2)Temp;
        RotaDirectionFactor = Vector3.Cross(RelativeVectorPlatform, collision.GetContact(0).normal).z;

    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (Vector3.Dot(Vector3.up, collision.GetContact(0).normal) > 0)
            HitNormal = collision.GetContact(0).normal;
        else
            HitNormal = Vector3.up;


        RelativeVectorPlatform = collision.GetContact(0).relativeVelocity - (Vector2)collision.GetContact(0).normal * Vector3.Dot(collision.GetContact(0).relativeVelocity, collision.GetContact(0).normal);
        RotaDirectionFactor = Vector3.Cross(RelativeVectorPlatform, collision.GetContact(0).normal).z;
        //Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + collision.GetContact(0).relativeVelocity *
        //    Mathf.Clamp01(Vector3.Dot(collision.GetContact(0).normal, collision.GetContact(0).relativeVelocity.normalized)), Color.red, 0.5f);
    }



    public void Reset()
    {
        ParentController.Reset();
    }

    public void ResetPawn()
    {
        scale = Vector3.one;
        yFactor = 0;
        JumpCharged = 0;
      //  DirectionParameter = 1;
        HitNormal = Vector2.up;
        HitTangent = Vector2.right;
        Grounded = false;
        // AnimationState = 0;
        RotaDirectionFactor = 0f;
    }
}



[System.Serializable]
public struct Stats
{
    public float MaxForce;
    public float MaxJumpForce;
    public float JumpChargeGravity;
    public float PawnHeight;
    public float PawnWidth;
    [Range(0.5f,5f)]
    public float StretchFactor;//4
    [Range(0.25f, 2f)]
    public float PlatformVectorFactor;//0.35
    [Range(10f, 50f)]
    public float SpeedStretchFactor;//50
    [Range(1.5f, 20f)]
    public float NormalAlignSpeed;//1.5
    [Range(0.5f, 5f)]
    public float AirAlignSpeed;//1.5
    [Range(2.5f, 100f)]
    public float AirStretchFactor;//10
    [Range(0.05f, 0.6f)]
    public float MaxStretchFactor;//0.5
    [Range(10f, 50f)]
    public float IncrementFactor;//40
    [Range(5f, 30f)]
    public float AirIncrementFactor;//25
}