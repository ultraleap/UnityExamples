# Ultrahaptics Core Asset

Ultrahaptics Core Asset (UCA) allows you to integrate Ultrahaptics', mid-air haptic sensations
into your Unity Projects.

## System Requirements

- Unity 2019.1.x (Windows 10, 64-bit)

For hand tracking via the Leap Motion Controller:

- Leap Motion Orion 4.0.0 SDK - (https://developer.leapmotion.com/get-started)
- Leap Motion Unity Core Assets 4.4.0 (https://developer.leapmotion.com/unity)

## Installing

To get started, create a new Unity Project, and import UCA via the following methods:

- From Unity's menubar: Assets > Import Asset > Select the UCA .unitypackage.
- Double-click the UCA .unitypackge from your system's file browser.
- Drag the .unitypackage from your system's file browser into your Unity project's 'Assets' folder.
- Manually copy the UCA .unitypackge into your Unity project's 'Assets' folder.


#### Notes on Library Loading and Updating Projects

- When importing UCA for the first time, libraries included in the UCA .unitypackage
may not automatically be loaded. Unity may need to be restarted before UCA behaves as expected.
- If you are replacing an existing version of the UCA in your Project, ensure that all UCA-related folders are
deleted from the Project's Asset's directory before replacing. Deletion must occur whilst Unity is not open.

## Configuring your Array and Tracking Origin

Depending on the Ultrahaptics kit you are using, you may need to configure the positions of your
array and tracking origins in Unity, to ensure that Sensations are experienced in the desired location.

The easiest way to do this is to add an *UltrahapticsKit* Prefab to your Unity scene and select the
kit you're using from the Ultrahaptics Kit dropdown menu in the Unity Inspector.

If you are using a Leap Motion Controller and the position of the unit is in its default cradle location
for its kit, then it is recommended that you add the *Leap Hand Controller* Prefab as a Child of the "UltrahapticsKit/TrackingOrigin"
GameObject in the *UltrahapticsKit* Prefab, ensuring the following Unity Transform for TrackingOrigin are set as follows:

| Model Name           | Position (x,y,z)     | Rotation (x,y,z) | Scale (x,y,z)  |
| ---------------------| ---------------------| ---------------- | -------------- |
| STRATOS Explore      | (0, 0, 0.121)        | (0, 0, 0)        | (1, 1, 1)      |
| STRATOS Inspire      | (0, -0.0006, -0.089) | (0, 0, 0)        | (1, 1, 1)      |

Note: If your array or tracking origin are in a non-default position (e.g. rotated), you will need to
manually configure the transforms to ensure your Sensations are experienced correctly.
