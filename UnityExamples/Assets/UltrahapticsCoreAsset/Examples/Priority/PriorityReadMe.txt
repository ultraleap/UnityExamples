Priority Example Scene
----------------------

This example demonstrates Sensation Source's 'Priority' property.

When multiple Sensation Sources are requested to be active at once, the Priority system provides control over the 'importance' of Sensations.
You can edit a Sensation Source's 'Priority' value to alter its importance when multiple active Sensation Sources exist.
Larger priority values mean that the Sensation Source is more important and will be chosen over those with lower priority values.

When running the Scene, an overview of the 'Active Sensations' can be found on the UltrahapticsKit's 'Sensation Emitter' component.
This gives an overview of the GameObjects with active Sensation Sources, and their respective Priority values.

This Scene contains the following behaviour when played:

When the hand is present, the palm position controls the CURSOR object - a 'PalmPresence' Sensation is experienced.
When the PARTICLE object collides with the CURSOR, a CursorAnimation timeline is played, producing a 'Close-Open' Sensation.
When the rising BAR object collides with the CURSOR, a 'HandScan' Sensation is experienced for 0.8 seconds.

By default, the Sensation Source on BAR has a higher priority (3) value than on PARTICLE (2)
This means that if both Sensations are asked to be Running at the same time, the BAR Sensation will be experienced.

In this scene, in addition to higher priority, the BAR object uses an 'Interruptible' state, to ensure that
if PARTICLE collides during the BAR's hover state, the PARTICLE does not 'affect' the CURSOR.
(Note: this is purely for the 'game' aspect of BAR's behaviour - which is designed to be an impenetrable, regenerative forcefield!)

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
