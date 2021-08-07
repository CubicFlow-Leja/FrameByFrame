using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InGameMenu : AbstractMenu
{
    [Header("Chapters")]
    public string Chapter;
    //public int CurrentLevel; ovo se ez rijesi sa build indexon
    public List<float> TimesForLevel;
    public static InGameMenu GameMenu { set; get; }
    [Header("Reward Panel")]
    public GameObject RewardsPanel;
    public GameObject SelectObjFinishedPanel;
    public List<GameObject> RewardPanelButtons;

    [Header("TextMeshRewards")]
    public TMPro.TextMeshProUGUI ChapterText;
    public TMPro.TextMeshProUGUI LevelText;
    public TMPro.TextMeshProUGUI TimeText;
    public TMPro.TextMeshProUGUI BestTimeText;
    public Image RewardImage;


    [Header("TopBottomTexts")]
    public TMPro.TextMeshProUGUI ChapterTextUi;
    public TMPro.TextMeshProUGUI LevelTextUI;
    public TMPro.TextMeshProUGUI TimeTextUI;
    public TMPro.TextMeshProUGUI BestTimeTextUI;
    public Image RewardImageUI;

    [Header("Level Times")]
    public TMPro.TextMeshProUGUI GoldText;
    public TMPro.TextMeshProUGUI SilveText;
    public TMPro.TextMeshProUGUI BronzeText;

    public Action ResetFunction = delegate { };
    private protected override void Start()
    {
        if (GameMenu == null)
            GameMenu = this;
        Cursor.visible = false;
        base.Start();
        initializeBlackBarInfo();
        InitButtonLists(RewardPanelButtons);

    }

    protected private void initializeRewardPanelInfo()
    {
        ChapterText.text = Chapter;
        LevelText.text = Mathf.RoundToInt((SceneManager.GetActiveScene().buildIndex) % 10f).ToString();
        TimeText.text = TimeString;
        BestTimeText.text = TurnTimeToString(SaveClass.RequestSaveData().LevelTime[SceneManager.GetActiveScene().buildIndex - 1]);

        GoldText.text = TurnTimeToString(TimesForLevel[0]);
        SilveText.text = TurnTimeToString(TimesForLevel[1]);
        BronzeText.text = TurnTimeToString(TimesForLevel[2]);

        RewardImage.sprite = RewardIcons[SaveClass.RequestSaveData().LevelReward[SceneManager.GetActiveScene().buildIndex - 1]];

    }

    protected private void initializeBlackBarInfo()
    {
        ChapterTextUi.text = Chapter;
        LevelTextUI.text = Mathf.RoundToInt((SceneManager.GetActiveScene().buildIndex) % 10f).ToString();
        BestTimeTextUI.text = TurnTimeToString(SaveClass.RequestSaveData().LevelTime[SceneManager.GetActiveScene().buildIndex - 1]);
        RewardImageUI.sprite = RewardIcons[SaveClass.RequestSaveData().LevelReward[SceneManager.GetActiveScene().buildIndex - 1]];
    }


    public void NextLevel()
    {
        ActPanel(false, false, RewardsPanel);
        SetLevelToLoad(SceneManager.GetActiveScene().buildIndex);

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;
    }


    public void Retry()
    {
        ActPanel(false, false, RewardsPanel);
        ResetFunction();
        initializeBlackBarInfo();

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;

        InputController.input.TIMESCALE = 1f;
    }

    public void ToMenu()
    {
        ActPanel(false, true, RewardsPanel);
        SoundController.SoundSystem.PlaySound(Src, SndId);
        AreYouSure.YesNo.ActivateQuestion(_Quit, NoDefaultRewards, "Exit To Menu?");

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;

    }

    protected private virtual void NoDefaultRewards()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(true, true, RewardsPanel);
    }


    public void OpenRewardScreen()
    {
        int RewardInt=0;
        if (CurrentTime < TimesForLevel[0])
            RewardInt = 3;
        else
        {
            if (CurrentTime < TimesForLevel[1])
                RewardInt = 2;
            else
            {
                if (CurrentTime < TimesForLevel[2])
                    RewardInt = 1;
            }
        }
            

        SaveClass.SaveGame(SceneManager.GetActiveScene().buildIndex - 1, CurrentTime, RewardInt);
        initializeRewardPanelInfo();
        initializeBlackBarInfo();
        ActPanel(true, true, RewardsPanel);
        InputController.input.TIMESCALE = 0;
     
        CurrentButtonsCount = RewardPanelButtons.Count;
        CurrentButtons = RewardPanelButtons;
        CurrentSelectObj = SelectObjFinishedPanel;
        Current = 0;
    }
 

    private protected override void EscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) &&!PressedEsc && !AreYouSure.YesNo.panel.activeSelf && AreYouSure.YesNo.ReleasedInput && !RewardsPanel.activeSelf && !SubtitleController.Subs.TutorialActive)
        {
            if (MainPanel.activeSelf)
            {
                ContinueGame();
            }
            else
            {
                Time.timeScale = 0;
                ActPanel(true, true, MainPanel);
                SoundController.SoundSystem.PlaySound(Src, SndId);
                CurrentButtonsCount = MainMenuButtons.Count;
                CurrentButtons = MainMenuButtons;
                CurrentSelectObj = SelectObj;
                Current = 0;
            }
        }


        if (RewardsPanel.activeSelf && AreYouSure.YesNo.ReleasedInput)
            HandleButtons();
    }


    public void SetCurrentTime(float f)
    {
        SetTime(f);
        TimeTextUI.text = TurnTimeToStringRealtime(CurrentTime);
    }

    protected private string TimeString="";
    public string TurnTimeToStringRealtime(float f)
    {
        TimeString =  Minutes(f).ToString("00")  + " : " +  Seconds(f).ToString("00") + " : " + MiliSeconds(f).ToString("00");
        return TimeString;
    }

    public string TurnTimeToString(float f)
    {
        return Minutes(f).ToString("00") + " : " + Seconds(f).ToString("00") + " : " + MiliSeconds(f).ToString("00");
    }


    protected private float CurrentTime = 0;
    public void SetTime(float f)
    {
        CurrentTime = f;
    }

    public float Minutes(float f)
    {
        return Mathf.Floor(f/60);
    }

    public float Seconds(float f)
    {
        return Mathf.Floor( f % 60);
    }

    public float MiliSeconds(float f)
    {
        if (f <= 0)
            return 0;
        float floor = Mathf.Floor(f);
        float p;
        if (floor != 0)
            p = (f % floor);
        else
            p = f;
        return  Mathf.FloorToInt( p*100f);
    }

    public override void QuitGame()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, MainPanel);
        AreYouSure.YesNo.ActivateQuestion(_Quit, NoDefault, "Quit game?");

    }
}
