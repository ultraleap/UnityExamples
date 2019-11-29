Forcefield Example Scene
---------------------------

This example demonstrates the Forcefield Sensation Block to produce a virtual haptic forcefield.

The Forcefield Sensation Block can be used to create haptic feedback boundaries.

When hands interact with the forcefield beam, a Sensation is experienced at the lines of intersection of the hand(s) and beam.

The "ForcefieldBeam" GameObject provides the Sensation Block with input data to evaluate the intersection of the hand(s) and forcefield beam.
It can be moved, scaled and rotated in the Scene to produce different haptic feedback boundaries.

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
