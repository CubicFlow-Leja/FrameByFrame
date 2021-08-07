using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlatform : MonoBehaviour
{
    public float MaxSpeed = 2f;
    protected private Rigidbody2D Rigid;
    protected private Vector3 initialPosition;
    protected private Quaternion InitialRotation;

    private void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        Move();
    }

    public abstract void Move();
    public virtual void Init()
    {
        Rigid = GetComponent<Rigidbody2D>();
        initialPosition = this.transform.position;
        InitialRotation = this.transform.localRotation;
        InputController.input.Platforms.Add(this);
    }
    public virtual void Reset()
    {
        this.transform.position = initialPosition;
        this.transform.localRotation = InitialRotation;
        Rigid.velocity = Vector2.zero;
        Rigid.angularVelocity = 0;
    }
}
