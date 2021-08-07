using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeOutEffect : MonoBehaviour
{
    public static FadeOutEffect fade = null;

    public Image image;
    public float DefaultFadeIntTime;
    public float GetAlpha()
    {
        return image.GetComponent<CanvasRenderer>().GetAlpha();
    }
    private void Awake()
    {
        if(fade==null)
            fade = this;
        image = GetComponent<Image>();
    }

  

    public void FadeOut(float _Time)
    {
        image.canvasRenderer.SetAlpha(0.0f);
        image.CrossFadeAlpha(1.0f, _Time, true);
    }

    public void FadeIn(float _Time)
    {
        image.canvasRenderer.SetAlpha(1.0f);
        image.CrossFadeAlpha(0.0f, _Time, true);
        
    }
}

