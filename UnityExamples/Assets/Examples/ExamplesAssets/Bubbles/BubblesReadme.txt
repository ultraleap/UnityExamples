Bubbles Example
---------------

The Bubbles Scene demonstrates how to position and trigger a Sensation when
of the hand intersects with virtual bubble objects.

To examine the haptics and behaviour, open the Bubbles/Prefabs/Bubble.prefab.
This object contains a Sensation Source, a Sphere Collider, and a script (TaggedTriggerRegsion.cs) 
which handles collisions of objects NOT tagged with 'Bubble' - e.g. the Leap Hand Controller.

The Sensation in question is a 'Sphere' haptic - which is setup to approximatel match the size of each bubble.
The Transform.position of each Bubble Prefab instance is used to position the 'centre' input of the Sensation.