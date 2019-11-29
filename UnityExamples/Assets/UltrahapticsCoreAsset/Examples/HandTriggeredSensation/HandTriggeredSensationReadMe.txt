HandTriggeredSensation Example Scene
----------------------------------

This example demonstrates how a Sensation (PalmTrackedPulsingCircle) can be triggered by the presence of a hand.

The SensationTriggerBox toggles playback of the Sensation Source when the hand enters/leaves its box collider.

The PalmTrackedPulsingCircle is a Sensation Block designed to pulse the radius of a circle path between a given start and end radius, 
with a given pulse period.

The Auto Mapper Component (on the UltrahapticsKit Prefab) feeds palm data from the Leap Data Source into the Sensation Block, to allow
it to track the palm.

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
