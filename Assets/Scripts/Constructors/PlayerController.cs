using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractController
{

   
    override protected private void CalculateAnimationData()
    {

        Model.transform.up = Vector3.up;

        if (DirectionParameter != 0)
            AnimationState = 1;
        else
            AnimationState = 0;
        // Debug.DrawLine(this.transform.position, this.transform.position+ this.transform.up*2f, Color.green);
        //if (MoveVector.x != 0)
        //{
        //    Vector3 scale = CharModel.transform.localScale;
        //    scale.x = scale.y * MoveVector.x;
        //    CharModel.transform.localScale = scale;
        //}

        //if (!Grounded)
        //{
        //    if (MoveVector.y > 0)
        //        AnimationState = 2;
        //    else
        //    {
        //        //test//ovo triba u raycast dio gori
        //        //  hit= Physics2D.Raycast(this.transform.position, Vector3.right, PawnStats.PawnWidth*1.6f, Mask);

        //        if (WallContacted)
        //        {
        //            if (MoveVector.y != 0)
        //                AnimationState = 4;
        //            else
        //                AnimationState = 5;
        //        }
        //        else
        //            AnimationState = 3;

        //        //if (hit.collider == null)
        //        //    AnimationState = 3;
        //        //else
        //        //    AnimationState = 4;
        //    }

        //}
        //else
        //{
        //    if (MoveVector.x != 0)
        //        AnimationState = 1;
        //    else
        //        AnimationState = 0;
        //}

    }

}
