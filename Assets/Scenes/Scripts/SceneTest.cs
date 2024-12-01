using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    public SceneTransitionManager TransitionManager;
    public bool is_next;    
        
    private void Start()
    {
        is_next = false;
    }

    private void Update()
    {
        if (is_next)
        {
            StartCoroutine(GoToScene());
        }
    }

    public IEnumerator GoToScene()
    {
        yield return new WaitForSeconds(2.0f);
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            TransitionManager.GoToScreen(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            TransitionManager.GoToScreen(0);
        }
        is_next = false;
    }

    
}
