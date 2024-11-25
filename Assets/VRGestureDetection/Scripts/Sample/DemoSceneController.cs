using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRGestureDetection.Sample
{
    public class DemoSceneController : MonoBehaviour
    {
    
        public void LoadScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

    }
}