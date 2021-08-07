using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSimpleTrigger : AbstractSoundPlayerMono
{
    //protected private AbstractPawn Pawn = null;
    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<AbstractPawn>() != null)
        //{
        //    Pawn = other.gameObject.GetComponent<AbstractPawn>();
        //    TriggerFunction();
        //}
            
    }

    public void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.GetComponent<AbstractPawn>() != null)
        //{
        //    Pawn = null;
        //    ExitFunction();
        //}
    }

    protected private abstract void ExitFunction();
    protected private abstract void TriggerFunction();

}
