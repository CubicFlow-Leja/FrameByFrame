using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController SoundSystem = null;

    public List<SoundTypeClass> Sounds;
    public List<SoundTypeClass> BackgroundSounds;
    public List<SoundTypeClass> AmbientSounds;
   
    void Awake()
    {
        if (SoundSystem == null)
            SoundSystem = this;
        else
            Destroy(this.gameObject);

    }

  
    public float PlaySound(AudioSource src, int _SoundId)
    {
        SoundClass temp = RetrieveFromList(_SoundId,Sounds);
        
        src.PlayOneShot(temp.Sound, temp.Volume* GetVolume(Sounds[_SoundId].Type)*Sounds[_SoundId].ListMasterVolume);
        return temp.Sound.length;
    }

    public float PlaySoundOptional(AudioSource src, int _SoundId,float SoundVolMulti)
    {

        SoundClass temp = RetrieveFromList(_SoundId, Sounds);

        src.PlayOneShot(temp.Sound, temp.Volume * SoundVolMulti * GetVolume(Sounds[_SoundId].Type) * Sounds[_SoundId].ListMasterVolume);
        return temp.Sound.length;
    }



    //ovo triba primat vise parametri
    public float StartBackgroundMusic(List<AudioSource> Sources,int SongId)
    {
        SoundTypeClass BackGroundSong = BackgroundSounds[SongId];

        float length = 0;
        for (int i = 0; i < Sources.Count; i++)
        {
            if (i >= BackGroundSong.SoundList.Count)
                break;

            AudioSource src = Sources[i];
            SoundClass temp = BackGroundSong.SoundList[i];
            src.PlayOneShot(temp.Sound, temp.Volume * GetVolume(BackGroundSong.Type) * BackGroundSong.ListMasterVolume);

            if (temp.Sound.length > length)
                length = temp.Sound.length;

        }
        
       
        return length;
       // return temp.Sound.length;

        //int index = Mathf.RoundToInt(Random.Range(0, BackgroundMusic.Count - 1));
        //src.PlayOneShot(BackgroundMusic[index].Sound, BackgroundMusic[index].Volume);
        //return BackgroundMusic[index].Sound.length;
    }
    public float StartAmbientMusic(List<AudioSource> Sources, int AmbientId)
    {
        SoundTypeClass AmbientAudio = AmbientSounds[AmbientId];

        float length = 0;
        for (int i = 0; i < Sources.Count; i++)
        {
            if (i >= AmbientAudio.SoundList.Count)
                break;

            AudioSource src = Sources[i];
            SoundClass temp = AmbientAudio.SoundList[i];
            src.PlayOneShot(temp.Sound, temp.Volume * GetVolume(AmbientAudio.Type) * AmbientAudio.ListMasterVolume);

            if (temp.Sound.length > length)
                length = temp.Sound.length;

        }


        return length;


        //SoundClass temp = RetrieveFromList(1, BackgroundSounds);

        //src.PlayOneShot(temp.Sound, temp.Volume * GetVolume(BackgroundSounds[1].Type) * BackgroundSounds[1].ListMasterVolume);
        //return temp.Sound.length;

        //int index = Mathf.RoundToInt(Random.Range(0, BackgroundMusic.Count - 1));
        //src.PlayOneShot(BackgroundMusic[index].Sound, BackgroundMusic[index].Volume);
        //return BackgroundMusic[index].Sound.length;
    }


    private SoundClass RetrieveFromList(int id,List<SoundTypeClass> List)
    {
        int index = Mathf.RoundToInt(Random.Range(0, List[id].SoundList.Count - 1));
        SoundClass tempSnd = List[id].SoundList[index];
        return tempSnd;
    }

    public float GetVolume(int Type)
    {
        float f = 1.0f;
        if (SaveClass.GetSettings() == null)
            return f;

        f *= SaveClass.GetSettings().MasterVol;
        switch (Type)
        { 
            case 0:
                f *= SaveClass.GetSettings().Effects;
                break;
            case 1:
                f *= SaveClass.GetSettings().Ambient;
                break;
            case 2:
                f *= SaveClass.GetSettings().Voice;
                break;
            case 3:
                f *= SaveClass.GetSettings().Music;
                break;
            case 4:
                f *= SaveClass.GetSettings().UI;
                break;

        }
        //Debug.Log(f);
        return f;
    }
}


[System.Serializable]
public class SoundTypeClass
{
    public string Name;
    public List<SoundClass> SoundList;
    public float ListMasterVolume;
    public int Type;

    public SoundTypeClass(string _Name, List<SoundClass> _SoundList, float Vol, int _Type)
    {
        this.Name = _Name;
        this.SoundList = _SoundList;
        this.ListMasterVolume = Vol;
        this.Type = _Type;
    }
}

[System.Serializable]
public class SoundClass
{
    public string Name;
    public AudioClip Sound;
    public float Volume;

    public SoundClass(string _Name, AudioClip _Sound,float Vol)
    {
        this.Name = _Name;
        this.Sound = _Sound;
        this.Volume = Vol;
    }
}

