using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : AbstractInputController
{
    public GameObject PlayerCharacter;
    public InputStruct InputMain;

    override protected void Init()
    {
        _Pawn = PlayerCharacter.GetComponent<AbstractController>();
        _Pawn.InitPawn(this);
        CameraController.Cam.SetPlayerObj(_Pawn.transform);
    }

  
    override protected void SendInput()
    {

        DirVector.x = Input.GetAxisRaw("Horizontal");
        DirVector.y = Input.GetAxisRaw("Vertical");

        InputMain.Dir = DirVector;
        InputMain.JumpPressed = Input.GetKey(KeyCode.Space);//|| Input.GetKeyDown(KeyCode.Space);
        InputMain.JumpReleased = Input.GetKeyUp(KeyCode.Space);
        _Pawn.AsignInput(InputMain);
    }
 


}


public struct InputStruct
{
    public Vector2 Dir;
    public bool JumpPressed;
    public bool JumpReleased;
}