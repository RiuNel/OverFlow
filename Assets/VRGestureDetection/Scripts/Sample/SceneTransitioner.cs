using UnityEngine;
using UnityEngine.UI; // Import this for UI manipulation
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

namespace VRGestureDetection.Sample
{
    public class SceneTransitioner : MonoBehaviour
    {
        public static SceneTransitioner Instance;
        public Image fadeImage;
        public float fadeDuration = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void TransitionToScene(int sceneId)
        {
            StartCoroutine(TransitionRoutine(sceneId));
        }

        private IEnumerator TransitionRoutine(int sceneId)
        {
            yield return StartCoroutine(FadeToBlack());
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            yield return StartCoroutine(FadeFromBlack());
        }

        private IEnumerator FadeToBlack()
        {
            float elapsedTime = 0;
            Color color = new Color(0, 0, 0, 1);
            if (fadeImage != null)
            {
                color = fadeImage.color;
            }

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                if (fadeImage) {
                    fadeImage.color = color;
                }
                yield return null;
            }
        }

        private IEnumerator FadeFromBlack()
        {
            float elapsedTime = 0;
            Color color = fadeImage.color;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }
    }
}