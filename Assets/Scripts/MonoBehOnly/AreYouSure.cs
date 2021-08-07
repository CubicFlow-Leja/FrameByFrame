using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class AreYouSure : MonoBehaviour
{

    public GameObject panel;
    public Text PanelText;
 

    public List<Button> Buttons;
    private int Current = 1;
    public GameObject SelectionObj;
    public float SelectObjSpeed = 6f;
    private Action YesAction = delegate { };
    private Action NoAction = delegate { };
    public bool ReleasedInput = true;
    public static AreYouSure YesNo { set; get; }

    [Header("Audio")]
    public AudioSource Src;
    public int SndId = 0;


    private void Awake()
    {
        if (YesNo == null)
            YesNo = this;
        DeactivatePanel(false, panel);

    }
    private void Start()
    {
        ReleasedInput = true;

        int c = 0;
        foreach (Button Butt in Buttons)
        {
            EventTrigger trigger = Butt.GetComponent<EventTrigger>();
            EventTrigger.TriggerEvent Event = new EventTrigger.TriggerEvent();
            int a = c;
            Event.AddListener((eventData) => OnSelectCounter(a));
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = Event, eventID = EventTriggerType.PointerEnter };

            trigger.triggers.Add(entry);
            c++;
        }
    }

    private void ResetButtons()
    {
        Current = 1;
    }

   
    private void Update()
    {
   

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
            ReleasedInput = true;

        if ((panel.activeSelf ) && ReleasedInput)
            HandlePanel();
        
    }

    public void OnSelectCounter(int i)
    {
        Current = i;
    }
    protected private void ChangeTargetCounter(int Increment,List<Button> ButtList)
    {
        Current += Increment;
        if (Current >= ButtList.Count)
            Current = 0;
        else
        {
            if (Current < 0)
                Current = ButtList.Count - 1;
        }
        SoundController.SoundSystem.PlaySound(Src, SndId);
    }

    private void HandlePanel()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeTargetCounter(1,  Buttons );
        else
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                ChangeTargetCounter(-1,  Buttons );
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Buttons[Current].onClick.Invoke();
            ReleasedInput = false;
        }
            
    

        SelectionObj.transform.localPosition += ( Buttons[Current].transform.localPosition - SelectionObj.transform.localPosition) * Time.fixedDeltaTime * SelectObjSpeed;
    }

    
    public void ActivateQuestion(Action Yes, Action No, string Question)
    {
        ReleasedInput = false;
        SelectionObj.SetActive(true);
        ResetButtons();
        YesAction = Yes;
        NoAction = No;
        DeactivatePanel(true, panel);
        PanelText.text = Question;
    }

    public void YesPls()
    {
        YesAction();
        DeactivatePanel(false, panel);
        SelectionObj.SetActive(false);
    }

    public void WtfNo()
    {
        NoAction();
        DeactivatePanel(false, panel);
        SelectionObj.SetActive(false);
    }
    private void DeactivatePanel(bool Act,GameObject Panel)
    {
        Panel.SetActive(Act);
    }



}
