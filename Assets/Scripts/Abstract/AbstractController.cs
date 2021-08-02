using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AbstractController : MonoBehaviour
{
    
    protected private Vector2 MoveVector;
    protected private RaycastHit2D hit;
    protected private bool Grounded;
    protected private bool CanJump;
    protected private InputStruct Inputs;
    protected private Action<InputStruct> InputFunction;
    
    public Animator Ani;
    public GameObject CharModel;
    public LayerMask Mask;
  

    //default
    protected private Collider2D PawnCollider;
    protected private Rigidbody2D PawnRigidBody;
    protected private int AnimationState;
    protected private int AnimationSubState;
    protected private AbstractInputController ParentController;


    public virtual void AsignInput(InputStruct InputMain)
    {
        Inputs = InputMain;
    }
    protected private abstract void HandleEnviroment();
    protected private abstract void CalculateAnimationData();
    protected private abstract void MoveCharacter();



    public virtual void InitPawn(AbstractInputController Parent)
    {
        CanJump = false;
        Grounded = false;
        MoveVector = Vector2.zero;
        PawnCollider = GetComponent<Collider2D>();
        PawnRigidBody = GetComponent<Rigidbody2D>();
        AnimationState = 0;
        AnimationSubState = 0;
        ParentController = Parent;
    }

    public virtual void SetAnimationStates(int State, int SubState)
    {
        Ani.SetInteger("State", AnimationState);
    }

  
    public void MovementLoop()
    {
        InputFunction(Inputs);
        HandleEnviroment();
        CalculateAnimationData();
        SetAnimationStates(AnimationState, AnimationSubState);
        MoveCharacter();
    }

    private void Update()
    {
        MovementLoop();
    }
}



[System.Serializable]
public struct Stats
{
    public float Health;
    public float Speed;
    public float JmpHeight;
    public float PawnHeight;
    public float PawnWidth;
}