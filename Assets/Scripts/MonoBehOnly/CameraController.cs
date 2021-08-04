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

        TargetVelocity.x = (PlayerObj.position.x - this.transform.position.x);
        TargetVelocity.y = (PlayerObj.position.y - this.transform.position.y);

        CameraVelocity += (TargetVelocity - CameraVelocity) * Time.deltaTime * CameraSpeedGravity;
        CameraRigidbody.velocity = CameraVelocity * CameraSpeed;
        ParallaxController.Parallax.SetVelocity(CameraRigidbody.velocity);
    }

    public void SetPlayerObj(Transform obj)
    {
        PlayerObj = obj;
    }
}
