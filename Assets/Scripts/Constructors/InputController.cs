using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputController : AbstractInputController
{
    public GameObject PlayerCharacter;
    public InputStruct InputMain;
    public Vector3 StartPosition;
    public Vector3 StartPositionPlayer;
    public Image FadeOutEffect;
    public float FadeoutTimer = 1f;
    private bool once = false;
    override protected void Init()
    {
        StartCoroutine(InitialFade());

        _Pawn = PlayerCharacter.GetComponent<AbstractController>();
        _Pawn.InitPawn(this);
        CameraController.Cam.SetPlayerObj(_Pawn.transform);

        StartPosition = this.GetComponent<CameraController>().CameraRigidbody.transform.position;
        StartPositionPlayer = _Pawn.transform.position;
    }

  
    override protected void SendInput()
    {

        DirVector.x = Input.GetAxisRaw("Horizontal");
        DirVector.y = Input.GetAxisRaw("Vertical");

        InputMain.Dir = DirVector;


        InputMain.JumpPressed =  Input.GetKeyDown(KeyCode.Space)|| Input.GetKey(KeyCode.Space);
        _Pawn.AsignInput(InputMain);
    }


    public override void Reset()
    {
        if(!once)
            StartCoroutine(ResetDelay());
    }

  

    public IEnumerator InitialFade()
    {
        FadeOutEffect.CrossFadeAlpha(0, FadeoutTimer, true);
        float f = 0;
        while (f < 1f)
        {
            Time.timeScale = f;
            f += Time.unscaledDeltaTime;
            f = Mathf.Clamp01(f);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Time.timeScale = 1f;

    }
    public IEnumerator ResetDelay()
    {
        once = true;
        float f = 1f;
        FadeOutEffect.CrossFadeAlpha(1f, FadeoutTimer, true);
        while (f>0)
        {
            Time.timeScale = f;
            f -= Time.unscaledDeltaTime;
            f = Mathf.Clamp01(f);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Time.timeScale = 0;

        FadeOutEffect.CrossFadeAlpha(0, FadeoutTimer, true);
        

        this.GetComponent<CameraController>().CameraRigidbody.transform.position = StartPosition;
        this.GetComponent<CameraController>().CameraRigidbody.velocity = Vector2.zero;

       
        _Pawn.transform.position = StartPositionPlayer;
        _Pawn.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _Pawn.ResetPawn();

        ParallaxController.Parallax.ResetParallax();

        while (f < 1f)
        {
            Time.timeScale = f;
            f += Time.unscaledDeltaTime;
            f = Mathf.Clamp01(f);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Time.timeScale = 1f;

        once = false;
        yield return null;
    }
}


public struct InputStruct
{
    public Vector2 Dir;
    public bool JumpPressed;
}