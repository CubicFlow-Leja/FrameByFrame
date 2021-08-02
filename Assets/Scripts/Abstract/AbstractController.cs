using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AbstractController : MonoBehaviour
{

    protected private float SlopeAngle;
    protected private float JumpCharged;
    protected private Vector2 ForceVector;
    protected private RaycastHit2D hit;
    protected private Vector2 HitNormal;
    protected private Vector2 HitTangent;
    protected private Vector2 WallVector;
    protected private float DirectionParameter;
    protected private bool Grounded;
    protected private bool CanJump;
    protected private InputStruct Inputs;
    protected private Collider2D PawnCollider;
    protected private Rigidbody2D PawnRigidBody;
    protected private int AnimationState;
    protected private AbstractInputController ParentController;
    protected private Vector3 scale = Vector3.one;

    [Header("CharacterParams")]
    public Animator Ani;
    public GameObject Character;
    public LayerMask CollissionMask;
    public Stats CharacterStats;

    [Header("Grounded")]
    [Range(0, 2)]
    public float RaycastMaxLength = 1f;
    [Range(0, 15)]
    public int RayCastResolution=4;
    [Range(0, 2)]
    public float RayCastStretchDistance=1f;
    [Range(0, 15)]
    public int RayCastStrecthResolution=3;

    [Header("Wall Detection")]
    [Range(0, 15)]
    public int RayCastWallResolution=4;
  



    public virtual void AsignInput(InputStruct InputMain)
    {
        Inputs = InputMain;
    }

    protected private abstract void CalculateAnimationData();
   

    protected private void HandleInputs(InputStruct _Input) { Inputs = _Input; }


    public virtual void InitPawn(AbstractInputController Parent)
    {
        CanJump = false;
        Grounded = false;
        ForceVector = Vector2.zero;
        PawnCollider = GetComponent<Collider2D>();
        PawnRigidBody = GetComponent<Rigidbody2D>();
        AnimationState = 0;
        ParentController = Parent;
        HitNormal = Vector2.up;
        WallVector = Vector2.zero;
        DirectionParameter = 1;
        JumpCharged = 0;


    }

    public virtual void SetAnimationStates()
    {
        Ani.SetInteger("State", AnimationState);
    }

  
    public void MovementLoop()
    {
        HandleInputs(Inputs);
        CheckGrounded();
       // WallInfluence();

        CalculateMovement();
      
        CalculateAnimationData();
        SetAnimationStates();
        MoveCharacter();
    }


   

    protected private void CheckGrounded()
    {
       
        Vector3 StartVec;
        Grounded = false;
       
        for (int i = 0; i <= RayCastResolution; i++)
        {
            StartVec = this.transform.position + this.transform.right * i / RayCastResolution * CharacterStats.PawnWidth / 2;
            hit = Physics2D.Raycast(StartVec, -this.transform.up, RaycastMaxLength, CollissionMask);

            if (hit.collider!= null)
            {
                Debug.DrawLine(StartVec, hit.point, Color.red);


                Grounded = true;
                CanJump = true;


                HitNormal = hit.normal;
                break;
            }

            if(i!=0)
            {
                StartVec = this.transform.position  - this.transform.right * i / RayCastResolution * CharacterStats.PawnWidth / 2;
                hit = Physics2D.Raycast(StartVec, -this.transform.up, RaycastMaxLength, CollissionMask);

                if (hit.collider != null)
                {
                    Debug.DrawLine(StartVec, hit.point, Color.red);

                    Grounded = true;
                    CanJump = true;


                    HitNormal = hit.normal;
                    break;
                }

            }

        }

        if(Grounded)
        {
            SlopeAngle = Vector3.Angle(HitNormal, Vector3.up);
            if (SlopeAngle >= 90.0f)
                SlopeAngle = 0.0f;

            float x = Vector3.Dot(HitNormal, Vector3.right);
            float y = Vector3.Dot(HitNormal, Vector3.up);
            HitTangent.x = y;
            HitTangent.y = -x;
            Debug.DrawLine(Character.transform.position, this.transform.position + (Vector3)HitTangent * 1, Color.yellow);

            DirectionParameter = Mathf.Sign(Vector3.Dot(PawnRigidBody.velocity,Vector3.right));
        }


        if (!CanJump && DirectionParameter != 0)
            for (int j = 1; j <= RayCastStrecthResolution; j++)
            {
                StartVec = this.transform.position -
                    this.transform.right  * (j / (float)RayCastStrecthResolution * RayCastStretchDistance * DirectionParameter + CharacterStats.PawnWidth / 2.0f * Mathf.Sign(DirectionParameter));

                hit = Physics2D.Raycast(StartVec, -this.transform.up, RaycastMaxLength, CollissionMask);
                if (hit.collider != null)
                {
                    Debug.DrawLine(StartVec, hit.point, Color.blue);
                    CanJump = true;
                    break;
                }
            }

    }


    protected private void HeadHit()
    {
        Vector3 StartVec;
        for (int i = -RayCastResolution; i <= RayCastResolution; i++)
        {
            StartVec = this.transform.position + this.transform.right * i / RayCastResolution * CharacterStats.PawnWidth / 2;
            hit = Physics2D.Raycast(StartVec, this.transform.up, RaycastMaxLength, CollissionMask);

            if (hit.collider != null)
            {
                Debug.DrawLine(StartVec, hit.point, Color.cyan);            }
        }
    }

    protected private void WallInfluence()
    {
        Vector3 StartVec;
        for (int i = -RayCastWallResolution; i < 0 + 1; i++)
        {
            StartVec = this.transform.position - Character.transform.up * CharacterStats.PawnHeight / 6f * i / RayCastWallResolution;
               
            hit = Physics2D.Raycast(StartVec, HitTangent * Mathf.Sign(Character.transform.localScale.x), RaycastMaxLength, CollissionMask);
            Debug.DrawLine(StartVec, StartVec +(Vector3) HitTangent * Mathf.Sign(Character.transform.localScale.x) * RaycastMaxLength, Color.cyan);

            if (hit.collider != null)
            {
                float length = ((Vector3)hit.point - this.transform.position).magnitude;
                if(length<=CharacterStats.PawnWidth/6.0f)
                {
                    WallVector = hit.normal;
                    return;
                }
                
            }
  
        }
        WallVector = Vector2.zero;
    }



    protected private void CalculateMovement()
    {
       
        if (Grounded)
        {
            if (Inputs.JumpPressed)
            {
                JumpCharged += (CharacterStats.MaxJumpForce - JumpCharged) * Time.fixedDeltaTime * CharacterStats.JumpChargeGravity;
                scale.y = 1f - JumpCharged / CharacterStats.MaxJumpForce * 0.25f ;
            }
            else
            {
                if ( JumpCharged / CharacterStats.MaxJumpForce > 0.25f)
                {
                    PawnRigidBody.AddForce(Character.transform.up * JumpCharged);
                    JumpCharged = 0;
                    scale.y = 1f;
                }

                JumpCharged += (0 - JumpCharged) * Time.deltaTime * CharacterStats.JumpChargeGravity;
                scale.y = 1f - JumpCharged / CharacterStats.MaxJumpForce * 0.25f;
            }

            Character.transform.up = Vector3.RotateTowards(Character.transform.up, HitNormal, 1.5f * Time.fixedDeltaTime, 1.0f);
            Vector2 vecInput = Inputs.Dir.x * HitTangent;
            ForceVector = vecInput*CharacterStats.MaxForce;
        }
        else
        {
           

            Character.transform.up = Vector3.RotateTowards(Character.transform.up, (PawnRigidBody.velocity.normalized*Mathf.Sign(PawnRigidBody.velocity.y)+Vector2.up).normalized, 1.5f * Time.fixedDeltaTime, 1.0f);
            ForceVector = Vector2.zero;
            JumpCharged += (0 - JumpCharged) * Time.deltaTime * CharacterStats.JumpChargeGravity*5f;
            scale.y = 1f + 0.25f*Mathf.Abs(PawnRigidBody.velocity.y) / 10.0f;
            //scale.y = 1f - JumpCharged / CharacterStats.MaxJumpForce * 0.25f;
            // HeadHit();
        }


        scale.y = Mathf.RoundToInt(scale.y * 20f) / 20f;
        scale.x = 2f - scale.y;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(DirectionParameter);
        this.GetComponent<CircleCollider2D>().radius = scale.y*0.75f;
        Character.transform.localScale = scale;
    }



    protected private  void MoveCharacter()
    {
        PawnRigidBody.AddForce(ForceVector);
    }
    private void FixedUpdate()
    {
        MovementLoop();
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + collision.GetContact(0).relativeVelocity*
            Mathf.Clamp01( Vector3.Dot(collision.GetContact(0).normal, collision.GetContact(0).relativeVelocity.normalized)) , Color.red, 0.5f);
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
    public float StretchSpeed;
}