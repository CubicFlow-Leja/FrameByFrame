using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform TeleporterExit;
    private Rigidbody2D Rigid;
    public float TpTime = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Rigid = collision.attachedRigidbody;
            StartCoroutine(Teleport());
           
        }
    }

    private IEnumerator Teleport()
    {
        
        Rigid.simulated = false;
        Rigid.transform.position = TeleporterExit.transform.position;
        yield return new WaitForSeconds(TpTime);
        Rigid.simulated = true;
        yield return null;
    }
}
