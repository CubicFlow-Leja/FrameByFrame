using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class InputController : AbstractInputController
{
    public static InputController input { set; get; }
    public GameObject PlayerCharacter;
    public InputStruct InputMain;
   // public Vector3 StartPosition;
  //  public Vector3 StartPositionPlayer;
    public float FadeoutTimer = 1f;
    private bool once = false;

    public Transform SpawnPosition;
    public List<AbstractPlatform> Platforms=new List<AbstractPlatform>();
    public float TIMESCALE = 1f;

    private void Awake()
    {
        if (input == null)
            input = this;

        
    }

    override protected void Init()
    {
        TIMESCALE = 1f;
        //   StartTime = Time.realtimeSinceStartup;

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(InitialFade());

        _Pawn = PlayerCharacter.GetComponent<AbstractController>();
        _Pawn.InitPawn(this);
        CameraController.Cam.SetPlayerObj(_Pawn.transform);

       // StartPosition = this.GetComponent<CameraController>().CameraRigidbody.transform.position;
       // StartPositionPlayer = _Pawn.transform.position;
        _Pawn.transform.parent = null;
       
    }


    protected private float CurrentTime = 0f;
   
    
    override protected void SendInput()
    {
        if (RewardPanel&& SubtitleController.Subs.TutorialActive)
        {
            InGameMenu.GameMenu.SetCurrentTime(CurrentTime);
            return;
        }
        else
        {
            CurrentTime +=Time.deltaTime* TIMESCALE;
           // CurrentTime =Mathf.Clamp( Time.realtimeSinceStartup - StartTime,0,1000000000f);
            CurrentTime =Mathf.Clamp(CurrentTime, 0,1000000000f);
            InGameMenu.GameMenu.SetCurrentTime(CurrentTime);
        }
       



        DirVector.x = Input.GetAxisRaw("Horizontal");
        DirVector.y = Input.GetAxisRaw("Vertical");

        InputMain.Dir = DirVector;


        InputMain.JumpPressed =  Input.GetKeyDown(KeyCode.Space)|| Input.GetKey(KeyCode.Space);
        _Pawn.AsignInput(InputMain);


        if (Input.GetKeyDown(KeyCode.T))
            Reset();
    }


 

    public override void Reset()
    {
        if(!once)
            StartCoroutine(ResetDelay());
    }

  

    public IEnumerator InitialFade()
    {
        while(FadeOutEffect.fade.image.canvasRenderer.GetAlpha()<0.99f)
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

        SubtitleController.Subs.RequestLevelText();

        while (SubtitleController.Subs.TutorialActive)
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

        Debug.Log("insert intro anim");
        float f = 0;
        while (f < 1f)
        {
            Time.timeScale = f;
            f += Time.unscaledDeltaTime;
            f = Mathf.Clamp01(f);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Time.timeScale = 1f;
        InGameMenu.GameMenu.ResetFunction = Reset;

    }
    public IEnumerator ResetDelay()
    {
        once = true;
        float f = 1f;
        FadeOutEffect.fade.FadeOut(FadeoutTimer);
        while (f>0)
        {
            Time.timeScale = f;
            f -= Time.unscaledDeltaTime;
            f = Mathf.Clamp01(f);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Time.timeScale = 0;

        FadeOutEffect.fade.FadeIn(FadeoutTimer);


        this.GetComponent<CameraController>().CameraRigidbody.transform.position = SpawnPosition.position;
        this.GetComponent<CameraController>().CameraRigidbody.velocity = Vector2.zero;
        this.GetComponent<CameraController>().Reset();


       
        _Pawn.transform.position = SpawnPosition.position;
        _Pawn.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _Pawn.ResetPawn();

        foreach (AbstractPlatform plat in Platforms)
            plat.Reset();

      //  StartTime = Time.realtimeSinceStartup;
        CurrentTime = 0;
        RewardPanel = false;

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