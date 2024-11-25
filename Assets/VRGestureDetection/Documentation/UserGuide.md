# User Guide

## Setting Up the Scene
1. **Add the Gesture Detection Script**
   - Attach the `GestureDetection` script to a GameObject in your scene.
   - Configure the script properties in the Inspector.

2. **Add the Required Transforms**
   - Assign the `headTransform`, `leftHandTransform`, and `rightHandTransform` to the corresponding XR controller objects in your scene.

3. **Assign the Model**
   - Assign the `Model Asset` in the inspector of the `GestureDetection` component

## Configuring Gestures

### Adding Gesture Events

1. **Add Gesture Events**
   - In the `GestureDetection` component, add gesture events by specifying the gesture name and the corresponding UnityEvent.

2. **Responding to Gestures**
   - Implement listeners for the UnityEvents to respond to specific gestures.


```csharp
void Start()
{
    GestureDetection.Instance.AddGestureEvent(new GestureEvent
    {
        gestureName = "waving right hand",
        onGestureDetected = new UnityEngine.Events.UnityEvent()
    });

    GestureDetection.Instance.AddGestureEvent(new GestureEvent
    {
        gestureName = "waving left hand",
        onGestureDetected = new UnityEngine.Events.UnityEvent()
    });

    GestureDetection.Instance.GetGestureEvents()[0].onGestureDetected.AddListener(OnWaveDetected);
    GestureDetection.Instance.GetGestureEvents()[1].onGestureDetected.AddListener(OnWaveDetected);
}

void OnWaveDetected()
{
    Debug.Log("Wave gesture detected!");
}
```

## Another setup option

```csharp
   void Start()
   {
      if (gestureDetection == null)
            gestureDetection = gestureDetection.Instance;

      GestureEvent clapGesture = new GestureEvent();
      clapGesture.gestureName = "clap";
      clapGesture.onGestureDetected = new UnityEngine.Events.UnityEvent();
      clapGesture.onGestureDetected.AddListener(() => 
      {
         Debug.Log("User clapped!");
      });
      gestureDetection.AddGestureEvent(clapGesture);
   }

```


# List of Gestures

The following gestures are currently available in the VR Gesture Detection System:

```markdown
- push two hands
- upper cut right hand
- upper cut left hand
- upper cut two hands
- clap
- pull down right hand
- pull down left hand
- pull down two hands
- timeout two hands
- cross arms
- uncross arms
- right hand circle
- left hand circle
- right hand swipe across
- left hand swipe across
- right hand throw overhand
- left hand throw overhand
- arms pound center
- yin yang
- jazz hands
- orb hands rotate
- windmill arms
- jump rope circles
- hand circles down
- right side two hand scoop
- left side two hand scoop
- super punch right
- super punch left
- dodge left
- dodge right
- duck
- extended arm circles
- hands open skyward
- hands open groundward
- hands open right side
- hands open left side
- reverse yin yang
- two hand throw overhand
- two hands push upward
- two hands push downward
- paddleboard stroke right
- paddleboard stroke left
- draw bow right
- draw bow left
- waving right hand
- waving left hand
- reeling in right hand
- reeling in left hand
- throw fishing line right
- throw fishing line left
- fishing line precast right
- fishing line precast left
```