using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class LevelButton : MonoBehaviour
{
    public int LevelIndex;
    public Button _Button;
    public Image ButtonImage;
    public Color ActiveColor;
    public Color InactiveColor;

    private GameObject TextMain;
    private GameObject TextLocked;
    private Image RewardIcon;

    public List<Sprite> RewardIconsList;
    public bool HasRewardIcon = true;

    private bool AlreadyInit = false;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        AlreadyInit = true;
        TextMain = this.transform.GetChild(0).gameObject;
        TextLocked = this.transform.GetChild(1).gameObject;
        if (HasRewardIcon)
            RewardIcon = this.transform.GetChild(2).GetComponent<Image>();
    }
    void Update()
    {

        if (!AlreadyInit)
            Init();

        if (SaveClass.RequestSaveData().LastLevelCompleted >= LevelIndex-1  || LevelIndex==0)
        {
            TextLocked.SetActive(false);
            TextMain.SetActive(true);
            _Button.interactable = true;
            ButtonImage.color = ActiveColor;
            if (HasRewardIcon)
            {
                RewardIcon.sprite = RewardIconsList[SaveClass.RequestSaveData().LevelReward[LevelIndex]];
                RewardIcon.gameObject.SetActive(true);
            }  
        }
        else
        {
            _Button.interactable = false;
            TextLocked.SetActive(true);
            TextMain.SetActive(false);
            ButtonImage.color = InactiveColor;
            if (HasRewardIcon)
                RewardIcon.gameObject.SetActive(false);
        }
    }

  
}
