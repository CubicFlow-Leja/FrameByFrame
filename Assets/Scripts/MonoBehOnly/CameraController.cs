using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Cam { set; get; }
    private Vector2 CameraVelocity;
    private Vector2 TargetVelocity;
    private Transform PlayerObj;
    public float CameraSpeedGravity=2.0f;
    public float CameraSpeed = 2.0f;
    public Rigidbody2D CameraRigidbody;

    public float CameraSizeMin = 10f;
    public float CameraSizeMax = 20f;
    public float CameraSpeedFactor = 50f;
    public Camera _Camera;
    void Awake()
    {
        CamInit();
    }
    void FixedUpdate()
    {
        MovementFunction();
    }


    void CamInit()
    {
        if (!Cam)
            Cam = this;
        else
            Destroy(this.gameObject);

        CameraVelocity = Vector2.zero;
        CameraRigidbody = this.GetComponent<Rigidbody2D>();
    }

   
   
    void MovementFunction()
    {
        TargetVelocity.x += ((PlayerObj.position.x - this.transform.position.x) *CameraSpeed - TargetVelocity.x) * Time.deltaTime * CameraSpeedGravity;
        TargetVelocity.y += ((PlayerObj.position.y - this.transform.position.y) * CameraSpeed - TargetVelocity.y) * Time.deltaTime * CameraSpeedGravity;
        //  TargetVelocity.y += (PlayerObj.position.y - this.transform.position.y);

       // CameraVelocity += (TargetVelocity * CameraSpeed - CameraVelocity) * Time.deltaTime * CameraSpeedGravity;
        CameraRigidbody.velocity = TargetVelocity;
        ParallaxController.Parallax.SetVelocity(CameraRigidbody.velocity);

        _Camera.orthographicSize += (Mathf.Lerp(CameraSizeMin, CameraSizeMax, Mathf.Clamp01( CameraRigidbody.velocity.magnitude / CameraSpeedFactor)) - _Camera.orthographicSize)*Time.fixedDeltaTime* CameraSpeedGravity;
    }

    public void SetPlayerObj(Transform obj)
    {
        PlayerObj = obj;
    }
}
