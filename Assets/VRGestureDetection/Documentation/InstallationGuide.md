# Installation Guide

## Introduction

Welcome to the VR Gesture Detection System! This guide will walk you through the process of installing and setting up the VRGestureDetection package in your Unity project. By the end of this guide, you will have a functional gesture detection system integrated into your VR application.

## Requirements

- Unity 2020.3 or later
- Unity Barracuda
- VR device (e.g., Oculus Quest)

## Steps

1. **Download and Import the Package**
   - Download the VRGestureDetection package from the Unity Asset Store.
   - Open your Unity project.
   - Go to `Assets > Import Package > Custom Package`.
   - Select the downloaded package and import all assets.

2. **Add Barracuda Package**:
      - Click on the `+` button in the top-left corner of the Package Manager window.
      - Select `Add package from git URL...`.

      **Enter the Package URL**:
      - Enter the following URL: `com.unity.barracuda` and click `Add`.

      **Verify Installation**:
      - Ensure the Barracuda package appears in the list of installed packages.

3. **Set Up the Scene**
   - Create a new scene or open an existing one.
   - Add the `GestureDetection` prefab to your scene.
   - Ensure the VR camera rig is set up correctly and the necessary XR components are configured.

4. **Configure the Gesture Detection**
   - Select the `GestureDetection` GameObject in the scene.
   - Assign the VR camera and controller transforms to the appropriate fields in the inspector.
   - Ensure the `NNModel` is assigned in the `GestureDetection` component.

5. **Test the Setup**
   - Enter play mode and test the gesture detection.
   - Ensure that the gestures are being detected correctly by observing the console logs or any UI elements you have set up.

## Troubleshooting

- **Gesture Not Detected**: Ensure that the VR device is properly connected and the transforms are correctly assigned.
- **Model Not Assigned**: Make sure the `NNModel` is properly assigned in the `GestureDetection` component.
- **Performance Issues**: Check if other processes are interfering with the performance. Optimize the scene if necessary.
