using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    private float timer = 0f;

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    void Update()
    {
        if (fadeImage.color.a > 0)
        {
            timer += Time.deltaTime;
            float alpha = 1 - (timer / fadeDuration);

            fadeImage.color = new Color(0, 0, 0, alpha);
        }
    }
}