using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
            FadeIn();
    }

    public void FadeIn(float fadeDuration = 2f)
    {
        Fade(1, 0, fadeDuration);
    }

    public void FadeOut(float fadeDuration = 2f)
    {
        Fade(0, 1, fadeDuration);
    }

    public void Fade(float alphaIn, float alphaOut, float fadeDuration)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, fadeDuration));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut,float fadeDuration)
    {
        float timer = 0;
        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        rend.material.SetColor("_Color", newColor2);
    }
}
