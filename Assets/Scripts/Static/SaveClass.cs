using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveClass 
{
    public static SaveData _Save;
    public static int LastSceneIndex = 40;
    public static int FinalCreditsIndex = 41;
    public static void SaveGame(int LevelNumber,float Time,int Reward)
    {
        if(Reward!=0)
        {
            _Save.LastLevelCompleted = (_Save.LastLevelCompleted > LevelNumber) ? _Save.LastLevelCompleted : LevelNumber;
            _Save.LevelTime[LevelNumber] = (_Save.LevelTime[LevelNumber] < Time && _Save.LevelTime[LevelNumber]!=0f) ? _Save.LevelTime[LevelNumber] : Time;
            _Save.LevelReward[LevelNumber] = (_Save.LevelReward[LevelNumber] > Reward) ? _Save.LevelReward[LevelNumber] : Reward;
        }
        _SaveGame();
    }
    
    public static void _SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.sv");
        bf.Serialize(file, SaveClass._Save);
        file.Close();
    }
 

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.sv"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save.sv", FileMode.Open);
            SaveClass._Save = (SaveData)bf.Deserialize(file);
            file.Close();

        }
        else
            _Save = new SaveData();
    }

    public static SaveData RequestSaveData()
    {
        return _Save;
    }


    //SETTINGS

    public static Settings _Settings=null;

    public static void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Settings.opt");
        bf.Serialize(file, SaveClass._Settings);
        file.Close();
    }

    public static void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/Settings.opt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Settings.opt", FileMode.Open);
            SaveClass._Settings = (Settings)bf.Deserialize(file);
            file.Close();
            return;
        }
        else
            if (_Settings == null)
            {
                _Settings = new Settings();
                SaveSettings();
                return;
            }
        
    }



    public static void RestoreDefaults()
    {
        _Settings = new Settings();
    }

    public static void SetWithoutSave(Settings _Set)
    {
        _Settings = new Settings(_Set);
    }

    public static void AcceptSettings(Settings _Set)
    {
        _Settings = new Settings( _Set);
        SaveSettings();
        LoadSettings();
    }

    public static Settings GetSettings()
    {
        return _Settings;
    }

    
}


[System.Serializable]
public class Settings
{
    public float MasterVol;
    public float Music;
    public float Ambient;
    public float Effects;
    public float Voice;
    public float UI;

    public Settings(float _Master,float _Music, float _Ambient,float _Effects,float _Voice,float _UI)
    {
        this.MasterVol = _Master;
        this.Music = _Music;
        this.Ambient = _Ambient;
        this.Effects = _Effects;
        this.Voice = _Voice;
        this.UI = _UI;
    }

    public Settings()
    {
        this.MasterVol = 0.8f;
        this.Music = 0.75f;
        this.Ambient = 0.65f;
        this.Effects = 0.65f;
        this.Voice = 0.9f;
        this.UI = 0.35f;
    }

    public Settings(Settings Set)
    {
        this.MasterVol = Set.MasterVol;
        this.Music = Set.Music;
        this.Ambient = Set.Ambient;
        this.Effects = Set.Effects;
        this.Voice = Set.Voice;
        this.UI = Set.UI;
    }
}


[System.Serializable]
public class SaveData
{
    public int LastLevelCompleted;
    public float[] LevelTime;
    public int[] LevelReward;

    public SaveData()
    {
        this.LastLevelCompleted = -1;
        this.LevelTime = new float[150];
        this.LevelReward = new int[150];
    }


}