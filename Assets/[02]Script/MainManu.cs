using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManu : MonoBehaviour
{
    public float count = 3f;
    private float delay;
    private bool Isstarted = false;
    private bool Isgone = false;
    private bool IsReadyCount = false;
    public int ReadyCount = 0;
    public Material targetMaterial;
    public float Blur;
    public float DownBlur = 0.0005f;
    public TextBlinkAlpha textblinkAlpha;
    public Image image;
    public void PlayGame()
    {
        Isstarted = true;
        if (textblinkAlpha != null)
        {
            textblinkAlpha.isBlink = false;
            textblinkAlpha.IsonClick();
        }
        delay = 0;
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void Gameover()
    {
        SceneManager.LoadScene(2);
    }
    public void SceneMainManu()
    {
        targetMaterial.SetFloat("_BlurAmount", 0.005f);
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        targetMaterial.SetFloat("_BlurAmount", 0.005f);
    }
    private void Update()
    {
        if (Isstarted)
        {
            delay += Time.deltaTime;
            if (image != null)
            {
                Color imageColor = image.color;
                if (imageColor.a > 0)
                {
                    imageColor.a -= Time.deltaTime*0.5f;
                    if (imageColor.a < 0) imageColor.a = 0; // ไม่ให้ติดลบ
                    image.color = imageColor;
                }
            }
            if (targetMaterial.GetFloat("_BlurAmount") >= 0)
            {
                if (IsReadyCount)
                {
                    Blur = targetMaterial.GetFloat("_BlurAmount") - DownBlur;
                    if (Blur < 0) Blur = 0;
                    targetMaterial.SetFloat("_BlurAmount", Blur);
                    IsReadyCount = false;
                }
                else { ReadyCount++; }
                if(ReadyCount == 5)
                {
                    IsReadyCount = true;
                    ReadyCount = 0;
                }
            }
            if (delay >= count)
            {
                Isgone = true;
                Isstarted = false;
            }
            textblinkAlpha.isBlink = false;
            if (Isgone)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
