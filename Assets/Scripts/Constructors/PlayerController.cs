using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractController
{
    //grounded 
    [Range(0, 15)]
    public int RayCastResolution;
    //detection stretch
    [Range(0, 2)]
    public float RayCastStretchDistance;
    [Range(0, 15)]
    public int RayCastStrecthResolution;

    //wall detection
    [Range(0, 1)]
    public float WallSlideSpeed;
    [Range(0, 15)]
    public int RayCastWallResolution;
    [Range(0, 2)]
    public float WallDetectionDistance;
    private bool WallContacted;
    //private bool AlreadyDoubleJumped;
    [Range(0, 1)]
    public float LedgeDetectionDistance;
    [Range(0, 2)]
    public float LedgeDetectionHeight;
    [Range(0, 1)]
    public float LedgeDetectionDepth;


    public override void InitPawn(AbstractInputController Parent)
    {
        WallContacted = false;
        InputFunction = HandleInputsDefault;
        base.InitPawn(Parent);

    }



    override protected private void HandleEnviroment()
    {
        float MoveDir = MoveVector.x;
      //  InputFunction = HandleInputsDefault;

        Grounded = false;
        CanJump = false;


        Vector3 StartVec;
        Color col = Color.white;


      
        for (int i = -RayCastResolution; i < RayCastResolution + 1; i++)
        {
            StartVec = this.transform.position - Vector3.up / 2.0f + Vector3.right * i / RayCastResolution * 1 / 2;
            hit = Physics2D.Raycast(StartVec, -Vector3.up, 0.5f, Mask);  
           

            if (hit.collider == null)
            {
                col = Color.blue;
            }
            else
            {
                col = Color.green;
                Grounded = true;
                CanJump = true;
            }
            Debug.DrawLine(StartVec, StartVec - Vector3.up * 1 / 2, col);

        }

        //jump rastezanje detekcije
        if (!CanJump && MoveDir != 0)
            for (int j = 1; j <= RayCastStrecthResolution; j++)
            {
                StartVec = this.transform.position - Vector3.right * MoveDir * (j / (float)RayCastStrecthResolution * RayCastStretchDistance + 0.5f / 2.0f);

                hit = Physics2D.Raycast(StartVec, -Vector3.up, 0.5f, Mask);

                if (hit.collider == null)
                {
                    col = Color.blue;
                }
                else
                {
                    col = Color.green;
                    CanJump = true;
                  
                }
                Debug.DrawLine(StartVec, StartVec - Vector3.up * 0.5f, col);
            }


        //pogodia strop
        if (!Grounded)
        {


            ///////Strop
            if (MoveVector.y > 0.0f)
            {
                for (int i = -RayCastResolution + 1; i < RayCastResolution; i++)
                {
                    StartVec = this.transform.position + Vector3.up * 0.5f / 2.0f + Vector3.right * i / RayCastResolution * 0.5f / 2;
                    hit = Physics2D.Raycast(StartVec, Vector3.up, 0.5f/ 2.0f, Mask);

                    if (hit.collider == null)
                    {
                        col = Color.blue;
                    }
                    else
                    {
                        col = Color.red;
                        MoveVector.y = -1.0f;
                        Debug.Log("hitcieling");
                        return;
                    }
                    Debug.DrawLine(StartVec, StartVec + Vector3.up * 0.5f / 2, col);

                }
            }

            WallContacted = true;

            for (int i = -RayCastWallResolution; i < RayCastWallResolution + 1; i++)
            {
                StartVec = this.transform.position + Vector3.up * 0.5f * i / RayCastWallResolution + Vector3.right * Mathf.Sign(CharModel.transform.localScale.x) * 0.5f / 4;


                hit = Physics2D.Raycast(StartVec, Vector3.right * Mathf.Sign(CharModel.transform.localScale.x), WallDetectionDistance, Mask);

                if (hit.collider == null)
                {
                    WallContacted = false;
                    col = Color.blue;
                    //InputFunction = HandleInputsDefault;
                    return;
                }
                else
                {
                    col = Color.red;
                }
                Debug.DrawLine(StartVec, StartVec + Vector3.right * Mathf.Sign(CharModel.transform.localScale.x) * WallDetectionDistance, col);

            }

            //ledge detection, ako je proslo cilu proslu petlju onda nece returnat 
            //if (MoveVector.z < 0)
            //    return;

            StartVec = this.transform.position + Vector3.up * LedgeDetectionHeight + Vector3.right * Mathf.Sign(CharModel.transform.localScale.x) * LedgeDetectionDistance;

            hit = Physics2D.Raycast(StartVec, -Vector3.up, LedgeDetectionDepth, Mask);

            if (hit.collider != null)
            {
                col = Color.red;
               // InputFunction = HandleInputsLedge;
            }
            else
                col = Color.blue;

            Debug.DrawLine(StartVec, StartVec - Vector3.up * LedgeDetectionDepth, col);
        }

    }



    //input functions
    ///obicni movement ima samo jedan jump
    protected private void HandleInputsDefault(InputStruct Inputs)
    {
        //horizontal
        MoveVector.x = Inputs.Dir.x;
        //vertical
        //  MoveVector.z = MoveDir.y;
       // MoveVector.z = 0;

        //gravity and jump
        if (Inputs.Jump && CanJump)//|| (!AlreadyDoubleJumped && MoveVector.y<PawnStats.JmpHeight/2.0f)))//da bude additivno na dj?
        {
            MoveVector.y =5;

            CanJump = false;
            //if (!CanJump)
            //    AlreadyDoubleJumped = true;
        }
        else
        {
            if (Grounded && MoveVector.y < -0.5f)
                MoveVector.y =-0.5f;
            else
            {
                MoveVector.y -= 9.81f * Time.deltaTime;
                MoveVector.y = Mathf.Clamp(MoveVector.y, -150, 55);
            }

        }
    }

 
    protected private void HandleInputsCieling(Vector2 MoveDir, bool Jump)
    {

    }


    //movement functions
    override protected private void MoveCharacter()
    {
        PawnRigidBody.velocity = this.transform.right * MoveVector.x*25  + this.transform.up * MoveVector.y*3;
        // PawnRigidBody.velocity = this.transform.right * MoveVector.x * PawnStats.Speed + this.transform.up * MoveVector.y;
       
    }

    
    override protected private void CalculateAnimationData()
    {
        if (MoveVector.x != 0)
        {
            Vector3 scale = CharModel.transform.localScale;
            scale.x = scale.y * MoveVector.x;
            CharModel.transform.localScale = scale;
        }

        if (!Grounded)
        {
            if (MoveVector.y > 0)
                AnimationState = 2;
            else
            {
                //test//ovo triba u raycast dio gori
                //  hit= Physics2D.Raycast(this.transform.position, Vector3.right, PawnStats.PawnWidth*1.6f, Mask);

                if (WallContacted)
                {
                    if (MoveVector.y != 0)
                        AnimationState = 4;
                    else
                        AnimationState = 5;
                }
                else
                    AnimationState = 3;

                //if (hit.collider == null)
                //    AnimationState = 3;
                //else
                //    AnimationState = 4;
            }

        }
        else
        {
            if (MoveVector.x != 0)
                AnimationState = 1;
            else
                AnimationState = 0;
        }

    }

}
