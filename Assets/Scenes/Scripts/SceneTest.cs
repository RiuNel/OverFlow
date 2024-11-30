using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    public SceneTransitionManager TransitionManager;

    private void Start()
    {
        StartCoroutine(GoToScene());
    }

    private void Update()
    {
        
    }

    IEnumerator GoToScene()
    {
        yield return new WaitForSeconds(2.0f);
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            TransitionManager.GoToScreen(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            TransitionManager.GoToScreen(0);
        }
    }
}
