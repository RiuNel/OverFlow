using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRGestureDetection.Core;

public class GestureImageMirror : MonoBehaviour
{
    public GestureDetection gestureDetection;
    public Image mirrorImage;
    public TextMeshProUGUI gestureNameText;

    /// <summary>
    /// Updates the mirror image to display the current gesture image detected by the gesture detection system.
    /// This is an encoded image of the gesture that was detected.
    /// </summary>
    void Update()
    {
        if (gestureDetection.GetCurrentGestureImage() != null)
        {
            mirrorImage.sprite = Sprite.Create(gestureDetection.GetCurrentGestureImage(), new Rect(0, 0, gestureDetection.GetCurrentGestureImage().width, gestureDetection.GetCurrentGestureImage().height), new Vector2(0.5f, 0.5f));
        }

        if (gestureNameText != null)
        {
            gestureNameText.text = gestureDetection.GetCurrentGesture();
        }
    }
}
