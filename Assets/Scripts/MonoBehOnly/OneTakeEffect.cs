using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTakeEffect : MonoBehaviour
{
    public ParticleSystem Sys;
    private Transform _Target;
    //wind u updejtu fali
    private Vector3 ForceVector = Vector3.zero;
    public void FixedUpdate()
    {
       // this.transform.forward = Vector3.RotateTowards(this.transform.forward, FlowField.VectorFlowField.WindVectorMain, Time.deltaTime * 2.5f, 1);

        var ForceModule = Sys.forceOverLifetime;

        //ForceVector.x += (Vector3.Dot(FlowField.VectorFlowField.WindVectorMain, this.transform.right) - ForceVector.x) * Time.fixedDeltaTime;
        //ForceVector.z += (Vector3.Dot(FlowField.VectorFlowField.WindVectorMain, this.transform.forward)  - ForceVector.z) * Time.fixedDeltaTime;
        //float X = Vector3.Dot(FlowField.VectorFlowField.WindVectorMain, this.transform.right) * 1f;
        //float Z = Vector3.Dot(FlowField.VectorFlowField.WindVectorMain, this.transform.forward) * 1f;

        
        //AnimationCurve curvex = new AnimationCurve();
        //AnimationCurve curvez = new AnimationCurve();

        //curvex.AddKey(0.0f, ForceVector.x);
        //curvex.AddKey(Sys.main.startLifetime.constant, ForceVector.x);

        //curvez.AddKey(0.0f, ForceVector.z);
        //curvez.AddKey(Sys.main.startLifetime.constant, ForceVector.z);

      
        //ForceModule.x = new ParticleSystem.MinMaxCurve(2.5f, curvex);
        //ForceModule.z = new ParticleSystem.MinMaxCurve(2.5f, curvez);

        ForceModule.x = new ParticleSystem.MinMaxCurve(ForceVector.x, ForceVector.x);
        ForceModule.z = new ParticleSystem.MinMaxCurve(ForceVector.z, ForceVector.z);

        if (_Target!=null)
            this.transform.position=_Target.position ;
            //this.transform.Translate((_Target.position - this.transform.position) * Time.deltaTime * 2.0f);
    }
    public void PlayOneShot(Transform Target,float Param)
    {
       // Sys.gameObject.GetComponent<ParticleSystemQualityController>().Param = Param;
        Sys.Play();
       _Target = Target;
       // this.transform.position = _Target.position;
        StartCoroutine(KillDelay());
    }
    private IEnumerator KillDelay()
    {
        yield return new WaitForSeconds(Sys.main.startLifetime.constant + Sys.main.duration);
        //this.transform.parent = null;
       // Sys.gameObject.GetComponent<ParticleSystemQualityController>().Param = 1f;
        this.gameObject.SetActive(false);
    }
}
