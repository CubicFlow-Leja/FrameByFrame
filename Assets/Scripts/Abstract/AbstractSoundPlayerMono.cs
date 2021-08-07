using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSoundPlayerMono : MonoBehaviour
{
    [Header("RepeatingSound")]
    private bool RepeatingSoundBool = false;
    public AudioSource RepeatingSource;
    public int RepeatingSoundInt;

    [Header("SingleShot")]
    protected private AudioSource Src;
    public void PlaySound(int id)
    {
        SoundController.SoundSystem.PlaySound(Src, id);
    }

    public void PlaySoundOptional(int id, float Vol)
    {
        SoundController.SoundSystem.PlaySoundOptional(Src, id, Vol);
    }


    protected private virtual void PlayRepeatingSound()
    {
        if (!RepeatingSoundBool)
        {
            RepeatingSoundBool = true;
            StartCoroutine(RepeatingSoundCooldown(SoundController.SoundSystem.PlaySound(RepeatingSource, RepeatingSoundInt)));
        }

    }
    protected private IEnumerator RepeatingSoundCooldown(float f)
    {
        yield return new WaitForSeconds(f*0.95f);
        RepeatingSoundBool = false;

    }
}
