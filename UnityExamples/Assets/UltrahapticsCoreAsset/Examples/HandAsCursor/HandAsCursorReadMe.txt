HandAsCursor Example Scene
---------------------------

This example demonstrates how to use hand tracking data to control a cursor and navigate UI elements.

When the hand is present, the palm position controls the cursor and a 'Presence' haptic is experienced.
When the hand hovers over the 'NO PROGRESS' zones, a 'Click' Sensation is experienced.
When the hand hovers over the 'PROGRESS' zones, a Dial Sensation is experienced, followed by a 'Click'.

The example requires Leap Unity Core Assets, which are not included with the UCA.

To use this example, you will need to run through the following steps:

1. Download and Install Leap Motion Orion 4.0.0 SDK (https://developer.leapmotion.com/get-started/)
2. Download Leap Motion Unity Core Assets 4.4.0, (https://developer.leapmotion.com/unity/)
3. Import the Leap Motion Unity Core Asset (from Step 2) into your Unity project
4. From LeapMotion/Core/Prefab, drag the LeapHandController prefab as a child of "UltrahapticsKit/TrackingOrigin" in the Scene

Note: Ensure that the Unity Transform for TrackingOrigin matches your Ultrahaptics Kit (see UltrahapticsCoreAsset/README)

5. In the LeapHandController Hand Model Manager component, type ‘2’ into the Model Pool > Size field
6. Ensure that both “Is Enabled” boxes are checked
7. Add the following Leap objects from Assets to the Graphics and Physics Hands sections, as below:

Graphics Hands:

-HandModelsNonHuman/Capsule Hand Left
-HandModelsNonHuman/Capsule Hand Right

Physics Hands:

-HandModelsPhysical/RigidRoundHand_L
-HandModelsPhysical/RigidRoundHand_R
