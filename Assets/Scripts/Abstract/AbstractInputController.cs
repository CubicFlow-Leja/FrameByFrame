using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInputController : MonoBehaviour
{
    protected private AbstractController _Pawn;
    protected private Vector2 DirVector;
    public bool RewardPanel = false;
    protected private virtual void Start() { Init(); }
    protected private virtual void Update() { HandleInputs(); }

    protected abstract void Init();

    protected virtual void HandleInputs()
    {
        SendInput();
    }
    protected abstract void SendInput();

    public abstract void Reset();

}
