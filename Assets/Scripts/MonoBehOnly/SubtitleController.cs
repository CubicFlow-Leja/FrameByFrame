using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SubtitleController : MonoBehaviour
{
    public static SubtitleController Subs { set; get; }
 
    public TMPro.TextMeshProUGUI LevelText;
    public TMPro.TextMeshProUGUI ChapterText;
    public TMPro.TextMeshProUGUI DescriptionText;
    public string Description;

    public GameObject TutorialObj;
    public GameObject SubtitleObj;

    public bool Tutorial=true;
    public bool TutorialActive=true;

    private void Awake()
    {
        if (Subs == null)
            Subs = this;
        else
            Destroy(this.gameObject);

    }

  

    public void RequestLevelTutorial()
    {
        TutorialObj.SetActive(true);
        StartCoroutine(TutorialPanel());
    }

    public void RequestLevelText()
    {
        if (Tutorial)
            RequestLevelTutorial();
        else
            TextMaterialFade();
        
    }

    private IEnumerator TutorialPanel()
    {
        while (!(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Space)))
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

       
        TutorialObj.SetActive(false);
        TextMaterialFade();
        yield return null;
    }



    [Header("TextMaterial")]
    public List<Material> TextMats;

    protected private void TextMaterialFade()
    {
        TutorialActive = false;
        StartCoroutine(TextMaterialCoroutine());
    }

    protected private IEnumerator TextMaterialCoroutine()
    {
        SubtitleObj.SetActive(true);

        LevelText.text = "Level : " + Mathf.RoundToInt((SceneManager.GetActiveScene().buildIndex) % 10f).ToString();
        ChapterText.text = "Chapter : " + InGameMenu.GameMenu.ChapterText.text;
        DescriptionText.text = Description;

        float a = 0;
        SetTextMats(a);
        while (a < 1f)
        {
            SetTextMats(a);
            a += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        a = 1f;
        SetTextMats(a);

        while (a > 0)
        {
            SetTextMats(a);
            a -= Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        a = 0;
        SetTextMats(a);
        SubtitleObj.SetActive(false);
        yield return null;
    }


    [Header("TextMeshColors")]
    public Color FaceColor;
    public Color OutlineColor;
    protected private void SetTextMats(float a)
    {
        foreach (Material mat in TextMats)
        {
            mat.SetColor("_FaceColor", new Color(FaceColor.r, FaceColor.g, FaceColor.b, a));
            mat.SetColor("_OutlineColor", new Color(OutlineColor.r, OutlineColor.g, OutlineColor.b, a));
        }
    }
}
