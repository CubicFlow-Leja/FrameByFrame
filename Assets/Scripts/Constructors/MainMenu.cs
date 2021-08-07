using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class MainMenu : AbstractMenu
{
  
  
    private protected override void Start()
    {
        base.Start();
        ActPanel(true, true, MainPanel);
        MainPanel.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        foreach (GameObject obj in MainMenuButtons)
            obj.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        SelectObj.transform.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        Logo.canvasRenderer.SetAlpha(0.0f);
    }


    
    private protected override void EscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MainPanel.activeSelf && !AreYouSure.YesNo.panel.activeSelf  && AreYouSure.YesNo.ReleasedInput && !Pressed)
            QuitGame();
    }



    public override void QuitGame()
    {
        SoundController.SoundSystem.PlaySound(Src, SndId);
        ActPanel(false, true, MainPanel);
        AreYouSure.YesNo.ActivateQuestion(_Quit, NoDefault, "Quit game?");
    }

}
