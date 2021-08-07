using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class AbstractMenu : MonoBehaviour
{
    [Header("Buttons And Panels")]
    public List<Sprite> RewardIcons;

    protected private Settings _Settings = null;
    public GameObject MainPanel;
    public GameObject OptionsPanel;
    public List<GameObject> LevelsPanels;
    public GameObject ChaptersPanel;

    public List<GameObject> MainMenuButtons;
    public List<GameObject> ChapterButtons;
    protected private List<GameObject> LevelButtons=new List<GameObject>();
    public List<GameObject> SettingsButtons;

    public Image Logo;
    public GameObject SelectObj;
    public GameObject SelectObjChapters;
    public GameObject SelectObjLevels;
    public GameObject SelectObjSettings;
    public float SelectObjSpeed = 5f;

    [Header("Audio")]
    public AudioSource Src;
    public int SndId = 4;

    [Header("SceneAndDelay")]
    public float Timer = 2.5f;
    protected private bool FadeEff = true;
    [Header("OptionsUI")]
    #region OptionsPanelUI
    public Slider Master;
    public Slider Effects;

    public Slider Music;
    public Slider Ambient;
    public Slider UI;
    #endregion


    private int SceneToLoad = 0;

    protected private virtual void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        FadeEff = true;
        SaveClass.Load();
        SaveClass.LoadSettings();
        ActPanel(false, false,OptionsPanel);
        ActPanel(false, false,MainPanel);
    }


    //mainmenu
    protected private virtual void Start()
    {
        FadeOutEffect.fade.FadeIn(Timer);
        ActPanel(false, false, MainPanel);
        SaveClass.LoadSettings();
        StartCoroutine(SceneStartDelay());

        InitButtonLists(MainMenuButtons);
        InitButtonLists(ChapterButtons);
        InitButtonLists(SettingsButtons);

        foreach (GameObject obj in LevelsPanels)
            for (int i = 0; i < obj.transform.childCount-1; i++)
            {
                LevelButtons.Add(obj.transform.GetChild(i).gameObject);
                obj.transform.GetChild(i).GetComponent<LevelButton>().RewardIconsList = RewardIcons;
            }
                   
               
        InitButtonLists(LevelButtons);

        SetTextMats(-1);



        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
    }

    protected private void InitButtonLists(List<GameObject> _List)
    {
        int c = 0;
        foreach (GameObject obj in _List)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            EventTrigger.TriggerEvent Event = new EventTrigger.TriggerEvent();
            int a = c;
            Event.AddListener((eventData) => OnSelectCounter(a));
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = Event, eventID = EventTriggerType.PointerEnter };

            trigger.triggers.Add(entry);
            c++;
        }
    }
   
    protected private virtual void Update()
    {
        if (FadeEff)
            return;
        
        EscMenu();

        if ((MainPanel.activeSelf|| OptionsPanel.activeSelf|| LevelsPanels[CurrentLevels].activeSelf  || ChaptersPanel.activeSelf)  && AreYouSure.YesNo.ReleasedInput)
            HandleButtons();

        if ( Input.GetKeyDown(KeyCode.Escape) && !AreYouSure.YesNo.panel.activeSelf && AreYouSure.YesNo.ReleasedInput && !PressedEsc)
        {
            if(OptionsPanel.activeSelf)
                CloseOptions();
            else
            {
                if (ChaptersPanel.activeSelf)
                    ExitChapters();
                else
                {
                    if(LevelsPanels[CurrentLevels].activeSelf)
                        ExitLevels(CurrentLevels);
                }
            }
                
            PressedEsc = true;
        }
            


   

        if (Input.GetKeyUp(KeyCode.Escape))
            PressedEsc = false;

    
    }

    protected private int CurrentLevels = 0;
    protected private int Current = 0;
    protected private int CurrentButtonsCount = 0;
    protected private List<GameObject> CurrentButtons;
    protected private GameObject CurrentSelectObj;

    public void OnSelectCounter(int i)
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        Current = i;
    }
    protected private void ChangeTargetCounter(int Increment)
    {
        Current += Increment;
        if (Current >= CurrentButtonsCount)
            Current = 0;
        else
        {
            if (Current < 0)
                Current = CurrentButtonsCount - 1;
        }
        SoundController.SoundSystem.PlaySound(Src, SndId);
    }

    protected private bool Pressed = false;
    protected private bool PressedEsc = false;
    protected private void HandleButtons()
    {
        

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangeTargetCounter(1);
        else
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                ChangeTargetCounter(-1);
             
        }
        CurrentSelectObj.transform.position += (CurrentButtons[Current].transform.position - CurrentSelectObj.transform.position) *Time.fixedDeltaTime* SelectObjSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && !Pressed)
        {
            if(CurrentButtons[Current].GetComponent<LevelButton>()==null)
                CurrentButtons[Current].GetComponent<Button>().onClick.Invoke();
            else
            {
                if (CurrentButtons[Current].GetComponent<LevelButton>().LevelIndex <= SaveClass.RequestSaveData().LastLevelCompleted + 1)
                    CurrentButtons[Current].GetComponent<Button>().onClick.Invoke();
                else
                    SoundController.SoundSystem.PlaySound(Src, SndId);
            }
              

            Pressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            Pressed = false;


    }


  
    protected private abstract void EscMenu();

    
    protected private virtual void NoDefault()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(true, true, MainPanel);
    }
    protected private virtual void NoDefaultOptions()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true,MainPanel);
        ActPanel(true, true, OptionsPanel);
    }
   
    
    protected private void ActPanel(bool State, bool _Cursor,GameObject Panel)
    {
        Panel.SetActive(State);
        Cursor.visible = _Cursor;
    }




    protected private void DefaultChange()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ChangeWithoutAccepting();
    }

    protected private void Draw()
    {
        DrawSliders();
    }

  

    protected private void DrawSliders()
    {
      
        DrawSingle(Master, _Settings.MasterVol);
        DrawSingle(Effects, _Settings.Effects);
        DrawSingle(Ambient, _Settings.Ambient);
        DrawSingle(Music, _Settings.Music);
        DrawSingle(UI, _Settings.UI);
    }

    protected private void DrawSingle(Slider _Slider,float Value)
    {
        _Slider.onValueChanged.RemoveAllListeners();
        _Slider.value = Value;
        _Slider.onValueChanged.AddListener(delegate { DefaultChange(); });
    }

    protected private void SetSett()
    {
      
        _Settings.MasterVol = Master.value;
        _Settings.Effects = Effects.value;
        _Settings.Ambient = Ambient.value;
        _Settings.Music = Music.value;
        _Settings.UI = UI.value;
    }


    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, false, MainPanel);
    }


    public void ExitChapters()
    {

        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, ChaptersPanel);
        ActPanel(true, true, MainPanel);
       

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;
    }

    public void ExitLevels(int i)
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, LevelsPanels[i]);
        ActPanel(true, true, ChaptersPanel);
      

        CurrentButtonsCount = ChapterButtons.Count;
        CurrentButtons = ChapterButtons;
        CurrentSelectObj = SelectObjChapters;
        Current = 0;
        SelectObjLevels.SetActive(false);
    }

    public void SetLevelToLoad(int i)
    {
        foreach(GameObject obj in LevelsPanels)
            ActPanel(false, false, obj);

        SoundController.SoundSystem.PlaySound(Src, SndId);
        SceneToLoad = i+1;
        SelectObj.SetActive(false);
        StartLoadingScene(SceneToLoad);
    }

    public void ChapterSelector()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, MainPanel);
        ActPanel(true, true, ChaptersPanel);
       

        CurrentButtonsCount = ChapterButtons.Count;
        CurrentButtons = ChapterButtons;
        CurrentSelectObj = SelectObjChapters;
        Current = 0;
    }



    public void _LevelList(int i)
    {
        ActPanel(false, true, ChaptersPanel);
        ActPanel(true, true, LevelsPanels[i]);
        SoundController.SoundSystem.PlaySound(Src, SndId);

        CurrentLevels = i;
        CurrentButtonsCount = LevelButtons.Count;
        CurrentButtons = LevelButtons;
        CurrentSelectObj = SelectObjLevels;
        Current = 0;
        SelectObjLevels.SetActive(true);
    }


    public void Accept()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, OptionsPanel);

        AreYouSure.YesNo.ActivateQuestion(_Accept, NoDefaultOptions,"Accept settings?");

    }

    public void ChangeWithoutAccepting()
    {
        SetSett();
        SaveClass.SetWithoutSave(_Settings);
    }

    protected private void _Accept()
    {
        SetSett();
        SaveClass.AcceptSettings(_Settings);
        CloseOptions();

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;
    }


    public void MainMenu()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        AreYouSure.YesNo.ActivateQuestion(_MainMenu, NoDefault,"Quit to menu?");
        ActPanel(false, true,MainPanel);
    }


    public void _MainMenu()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true,MainPanel);
        StartCoroutine(Delay(Menu));
    }

    protected private void Menu()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        SceneLoader.RequestSceneLoad(0);
    }


    public void Options()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true,MainPanel);
        ActPanel(true, true, OptionsPanel);

        CurrentButtonsCount = SettingsButtons.Count;
        CurrentButtons = SettingsButtons;
        CurrentSelectObj = SelectObjSettings;
        Current = 0;

        SaveClass.LoadSettings();
        _Settings = new Settings(SaveClass.GetSettings());
        Draw();
    }

    public virtual void CloseOptions()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        SaveClass.LoadSettings();
        ActPanel(false, true, OptionsPanel);
        ActPanel(true, true, MainPanel);

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;
    }
    
    public void Defaults()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, OptionsPanel);

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;

        AreYouSure.YesNo.ActivateQuestion(_Defaults, NoDefaultOptions,"Restore defaults?");
    }
    protected private void _Defaults()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(true, true, OptionsPanel);


        CurrentButtonsCount = SettingsButtons.Count;
        CurrentButtons = SettingsButtons;
        CurrentSelectObj = SelectObjSettings;
        Current = 0;

        SaveClass.RestoreDefaults();
        _Settings = new Settings(SaveClass.GetSettings());
        Draw();
    }


    public void Cancel()
    {

        CurrentButtonsCount = MainMenuButtons.Count;
        CurrentButtons = MainMenuButtons;
        CurrentSelectObj = SelectObj;
        Current = 0;
        CloseOptions();
    }

    
    protected private void _Quit()
    {
        ActPanel(false, true, MainPanel);
        SoundController.SoundSystem.PlaySound(Src, SndId);
        FadeOutEffect.fade.FadeOut(Timer);
        StartCoroutine(Delay(QuitGameStatic.QuitGame));
    }

    public abstract void QuitGame();
    

    protected private IEnumerator SceneStartDelay()
    {
        yield return new WaitForSecondsRealtime(Timer * 1.05f);
        FadeEff = false;
        FadeMenu(1);
        TextMaterialFade(1);
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in MainMenuButtons)
        {
            obj.GetComponent<Button>().interactable = true;
        }

    }
    
    public IEnumerator Delay(Action funToRun)
    {
        FadeOutEffect.fade.FadeOut(Timer);
        yield return new WaitForSecondsRealtime(Timer * 1.05f);
        funToRun();
    }
    
    public void StartLoadingScene(int Index)
    {
        FadeOutEffect.fade.FadeOut(Timer);
        StartCoroutine(SceneLoadDelay(Index));
    }

    public IEnumerator SceneLoadDelay(int Index)
    {
        yield return new WaitForSecondsRealtime(Timer * 1.05f);
        SceneLoader.RequestSceneLoad(Index);
    }

  
    protected private void FadeMenu(float Fade)
    {
        MainPanel.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        MainPanel.GetComponent<Image>().CrossFadeAlpha(1.0f, Timer, true);
        ButtonFade(Fade);
    }

    protected private void ButtonFade(float Fade)
    {
        foreach (GameObject obj in MainMenuButtons)
        {
            obj.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
            obj.GetComponent<Image>().CrossFadeAlpha(1.0f, Timer, true);
        }
        SelectObj.transform.GetComponent<Image>().CrossFadeAlpha(1.0f, Timer, true);
        Logo.CrossFadeAlpha(1.0f, Timer, true);
    }


    [Header("TextMaterial")]
    public List<Material> TextMats;

    protected private void TextMaterialFade(float Fade)
    {
        StartCoroutine(TextMaterialCoroutine());
    }

    protected private IEnumerator TextMaterialCoroutine()
    {
        float a = 0;
        SetTextMats(a);
        while (a < 1f)
        {
            SetTextMats(a);
            a += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        a = 1f;
        SetTextMats(a);

        while (a > 0.95f)
        {
            SetTextMats(a);
            a -= Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        a = 0.95f;
        SetTextMats(a);

        yield return null;
    }

    [Header("TextMeshColors")]
    public Color FaceColor;
    public Color OutlineColor;
    protected private void SetTextMats(float a)
    {
        foreach (Material mat in TextMats)
        {
            mat.SetColor("_FaceColor",new Color(FaceColor.r,FaceColor.g,FaceColor.b,a));
            mat.SetColor("_OutlineColor", new Color(OutlineColor.r, OutlineColor.g, OutlineColor.b, a));
        }
    }
}
