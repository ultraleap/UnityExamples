# Ultrahaptics Core Asset - Block Manifest

This file describes the Sensation Blocks available to the Ultrahaptics Core Asset for Unity.

A _'Sensation-producing'_ Block (one that outputs control points) can by added to Unity be entering its name into the *Sensation Block* field of a *Sensation Source* component.

You can use the Blocks described below to build your own Block graphs, by creating an instance of that Block (using `createInstance`), and connecting Block inputs and outputs (using `connect`)

For more information on creating Block Graphs via Python, refer to the **pysensationcore** Python module, included in StreamingAssets/Python.



## Types

### transform

Used to store or manipulate position, rotation, scale and other types of affine transform.

### path

A channel-based type representing a finite-length path in 3-dimensional space.

Evaluation of the `point` forward channel with a value between 0.0 and 1.0 on
the `u` backward channel will yield a point on the path.  Sweeping the `u`
value through its range will visit every point on the path.

The result of evaluating `point` for `u` values outside of the valid range
is undefined.

Paths can be input to the **RenderPath** Block to produce control point output.

### boolean

A 3-tuple where the first element carries a real-valued scalar and the remaining two values are ignored. Scalar values approximately 0 represent False, and values close to 1 represent True

### scalar

A 3-tuple where the first element carries a real-valued scalar and the
remaining two values are ignored

### time

Synonym for `scalar`.  Used to denote time values in seconds.

### uhsclVector3_t

A 3-tuple of real-valued numbers.  Used to represent position or direction
in 3-dimensional space.

### Point

A 3-tuple of real-valued numbers. Represents a position in 3-dimensional space.

### List

A variable length array, each element in the list can be of any type.

#### Channels

* `u` (backward, scalar) Supply a value in the range 0.0 <= `u` <= 1.0
* `point` (forward, uhsclVector3_t) point on the path corresponding to
          the given `u` value


## DataSources

When an `AutoMapper` exists in the Unity scene, `DataSources` allow data derived from the Unity scene to be passed on to Block inputs.
`DataSource` inputs registered with the `AutoMapper` get "auto-mapped" (or resolved) on each frame update.
This mechanism allows Block inputs to receive dynamic data as gameplay goes on.

It is possible to write your own custom `DataSources` and register them to the `AutoMapper`, to give your Blocks custom data.
For example, you may wish to provide custom hand tracking data, or properties of your game, (e.g. 'health' of a character) as an input available to your Block.

The UCA includes a number of `DataSources` which are described below:

-----------------

### SensationSpaceToVirtualSpaceDataSource

This data source provides the required transform for going between Sensation Space and Virtual Space.

* `sensationXInVirtualSpace` (uhsclVector3_t): direction of the Sensation Space x axis in Virtual Space
* `sensationYInVirtualSpace` (uhsclVector3_t): direction of the Sensation Space y axis in Virtual Space
* `sensationZInVirtualSpace` (uhsclVector3_t): direction of the Sensation Space z axis in Virtual Space
* `sensationOriginInVirtualSpace` (uhsclVector3_t): origin of the Sensation Space axis in Virtual Space

-----------------

### LeapDataSource

This data source provides the required transform for going between Virtual Space and Virtual Hand Space.
This data source relies on the Leap Motion Core Asset for Unity and gets information from the Leap Hand Controller.
This is used for sensations which are to be mapped onto the hand.

* `virtualObjectXInVirtualSpace` (uhsclVector3_t): direction of the virtual hand x axis in Virtual Space
* `virtualObjectYInVirtualSpace` (uhsclVector3_t): direction of the virtual hand y axis in Virtual Space
* `virtualObjectZInVirtualSpace` (uhsclVector3_t): direction of the virtual hand z axis in Virtual Space
* `virtualObjectOriginInVirtualSpace` (uhsclVector3_t): origin of the virtual hand axis in Virtual Space

This data source provides information about hand, wrist, and finger positions.

#### Full list of "hand agnostic" data items

##### Hand

* `palm_position` (uhsclVector3_t): position of the palm in Virtual Space
* `palm_direction` (uhsclVector3_t): direction of the palm in Virtual Space
* `palm_normal` (uhsclVector3_t): normal of the palm in Virtual Space
* `palm_scaled_direction` (uhsclVector3_t): direction of the palm. The size of the vector is half the length of the hand.
* `palm_scaled_transverse` (uhsclVector3_t): vector perpendicular to palm_length. The size of the vector is half the width of the hand.


##### Wrist

* `wrist_position` (uhsclVector3_t): position of the wrist in Virtual Space

##### Fingers

All finger information is of type (uhsclVector3_t)

* `thumb_distal_position`
* `thumb_intermediate_position`
* `thumb_proximal_position`
* `thumb_metacarpal_position` (Note, this does not exist, it is provided and is equivalent to the proximal bone for parity)

* `indexFinger_distal_position`
* `indexFinger_intermediate_position`
* `indexFinger_proximal_position`
* `indexFinger_metacarpal_position`

* `middleFinger_distal_position`
* `middleFinger_intermediate_position`
* `middleFinger_proximal_position`
* `middleFinger_metacarpal_position`

* `ringFinger_distal_position`
* `ringFinger_intermediate_position`
* `ringFinger_proximal_position`
* `ringFinger_metacarpal_position`

* `pinkyFinger_distal_position`
* `pinkyFinger_intermediate_position`
* `pinkyFinger_proximal_position`
* `pinkyFinger_metacarpal_position`

#### Hand specific

* `leftHand_present` (boolean): boolean whether the left hand is present
* `rightHand_present` (boolean): boolean whether the right hand is present

##### Left hand

All of the "hand agnostic" data items prefixed with `leftHand_`

For example: `leftHand_thumb_distal_position`

##### Right hand

All of the "hand agnostic" data items prefixed with `rightHand_`

For example: `rightHand_thumb_distal_position`

-----------------

### EmitterDataSource

This data source provides the required transform for going between Virtual Space and Virtual Emitter Space.
This is used for sensations produced to be relative to a virtual emitter in Virtual Space.
For example when moving the virtual emitter closer to the virtual object which the sensation is mapped to, the sensation in Emitter Space will appear closer to the emitter. This is because the relative distance between the virtual emitter and the virtual object has reduced.

* `virtualEmitterXInVirtualSpace` (uhsclVector3_t): direction of the virtual emitter x axis in Virtual Space
* `virtualEmitterYInVirtualSpace` (uhsclVector3_t): direction of the virtual emitter y axis in Virtual Space
* `virtualEmitterZInVirtualSpace` (uhsclVector3_t): direction of the virtual emitter z axis in Virtual Space
* `virtualEmitterOriginInVirtualSpace` (uhsclVector3_t): origin of the virtual emitter axis in Virtual Space

-----------------

### VirtualSpaceToEmitterSpaceDataSource

This data source provides the required transform for going between Virtual Space and Emitter Space.

* `virtualXInEmitterSpace` (uhsclVector3_t): direction of the Virtual x axis in Emitter Space
* `virtualYInEmitterSpace` (uhsclVector3_t): direction of the Virtual y axis in Emitter Space
* `virtualZInEmitterSpace` (uhsclVector3_t): direction of the Virtual z axis in Emitter Space
* `virtualOriginInEmitterSpace` (uhsclVector3_t): origin of the Virtual axis in Emitter Space

## Blocks

-----------------

### **EmptyList**

* Sensation-producing: **NO**

Outputs an empty list to be used in conjunction with other blocks

#### Output:

* `out` (List): An empty list

-----------------

### **ListAppend**

* Sensation-producing: **NO**

Outputs a list with an `element` appended to the end of the `list`

#### Inputs:

* `list` (List): List to append
* `element` (any): Element to append to `list`

#### Output:

* `out` (List): The original list with the `element` input appended to the end of `list`

-----------------

### **CirclePath**
Outputs a circular path, with given radius, in the z=0 plane

#### Inputs:

* `radius` (scalar): The radius of the circle.

#### Output:

* `out` (path): A circle of the given `radius`
  * Sensation-producing: **NO**

-----------------

### **CircleSensation**

Outputs a circular path, with given radius, at the given offset

* Is Transformable

A palm-tracked alternative to `CircleSensation` exists called `PalmTrackedCircle` which additionally
requires the `LeapDataSource` to supply further auto-mapped values, but is not Transformable.

#### Inputs:

* `radius` (uhsclVector3_t): the radius of the circle
* `intensity` (scalar): Sets the intensity (strength) of the Sensation (default = 1.0)
* `drawFrequency` (scalar): Number of times per second that the Circle path is drawn (default = 70Hz)

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Output:

* `out` (uhsclVector3_t): A circle of the given `radius` and `offsetInVirtualSpace`
  * Sensation-producing: **YES**

-----------------

### **Comparator**

Compares two values (a and b) and returns a value depending on the result of comparison cases:
`a` greater than `b`, `a` equal to `b`, or `a` less than `b`.

#### Inputs:

* `a` (scalar): First value to be compared
* `b` (scalar): Second value to be compared
* `returnValueIfAGreaterThanB` (uhsclVector3_t): Returned value when `a` is greater than `b`
* `returnValueIfAEqualsB` (uhsclVector3_t): Returned value when `a`, is equal to `b`
* `returnValueIfALessThanB` (uhsclVector3_t): Returned value when `a` is less than `b`

#### Output:
* `out` (scalar): Outputs value depending comparisson
  * Sensation-producing: **NO**

-----------------

### **ComposeInverseTransform**

Calculate the inverse of a transform using the component vectors.

#### Inputs:

* `x` (uhsclVector3_t): the value of the x vector (first column)
* `y` (uhsclVector3_t): the value of the y vector (second column)
* `z` (uhsclVector3_t): the value of the z vector (third column)
* `o` (uhsclVector3_t): the offset of the transform

#### Outputs:

* `out` (transform): the inverse of the composed transform
  * Sensation-producing: **NO**

-----------------

### **ComposeTransform**

Compose a transform using the component vectors.

#### Inputs:

* `x` (uhsclVector3_t): the value of the x vector (first column)
* `y` (uhsclVector3_t): the value of the y vector (second column)
* `z` (uhsclVector3_t): the value of the z vector (third column)
* `o` (uhsclVector3_t): the offset of the transform

#### Outputs:

* `out` (transform): the composed transform
  * Sensation-producing: **NO**

-----------------

### **IntensityWave**

Calculates the value of a cosine wave given a modulationFrequency at a point in time. It varies between 0 and 1.

#### Inputs:

* `t` (scalar): point in time
* `modulationFrequency` (scalar): frequency of the cosine wave (default = 143)

#### Outputs:

* `out` (scalar): value of the cosine wave at point t
  * Sensation-producing: **NO**

-----------------

### **Intensity Modulation**

Takes a provided `point` and returns the point with a modified intensity, whose intensity is modulated `modulationFrequency` times per second

#### Inputs:
* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `point` (uhsclVector3_t): The point whose intensity should be modulated
* `modulationFrequency` (scalar): The Frequency (in hz) with which intensity should be modulated

#### Outputs:
* `out` (uhsclVector4_t): Control point with modified intensity
  * Sensation-producing: **NO**

-----------------

### **CrossProduct**

Calculate the cross product of two 3-vectors.

#### Inputs:

* `lhs` (uhsclVector3_t): left-hand-side operand to the cross product
* `rhs` (uhsclVector3_t): right-hand-side operand to the cross product

#### Outputs:

* `out` (uhsclVector3_t): the cross-product lhs x rhs
  * Sensation-producing: **NO**
* `normalized` (uhsclVector3_t): the unit-length vector in the same direction as `out`
  * Sensation-producing: **NO**

-----------------

### **DialSensation**

Dial Sensation produces a circular sensation moving along a circular path

* Is Transformable

A palm-tracked alternative to `DialSensation` exists called `PalmTrackedDial` which additionally
requires the `LeapDataSource` to supply further auto-mapped values, but is not Transformable.

#### Inputs:

* `innerRadius` (scalar): radius of the sensation translated along the circular path (default =  0.025)
* `outerRadius` (scalar): radius of the circular path (default = 0.05)
* `rate` (scalar): determines how fast the circular sensation moves along the circular path.
                   Negative values change the direction of the path.
* `drawFrequency` (scalar): Number of times per second that the Dial's circle path is drawn (default = 70Hz)

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t): Output port, producing control points for the Dial Sensation
  * Sensation-producing: **YES**

-----------------

### **ExpandingCircleSensation**

Draws a Circle Sensation which expands from an initial radius to a final radius, over a given duration specified in seconds.

* Is Transformable

#### Inputs:

* `duration` (scalar): The duration in seconds over which the circle expands from start to end radius (default: 1.0)
* `startRadius` (scalar): the starting radius of the circle, specified in meters (default 0.01)
* `endRadius` (scalar): the final radius of the circle, specified in meters (default 0.05)
* `drawFrequency` (scalar): Number of times per second that the Expanding Circle path is drawn (default = 70Hz)

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Output:

* `out`: Output port, producing control points for the Expanding Circle Sensation
  * Sensation-producing: **YES**

-----------------

### **ColinearSegmentToSegmentIntersection**

Calculates the line segment resulting from the intersection of two colinear segments.
If the segments are not colinear the result is undetermined.

#### Inputs:

* `segment0` (OptionalLineSegment): First segment
* `segment1` (OptionalLineSegment): Second segment

#### Output:
* `out` (OptionalLineSegment): the segment resulting from the intersection between the two input segments
                               If there is no intersection or any of the inputs are invalid segments,
                               the block returns an invalid line segment
 * Sensation-producing: **NO**

-----------------

### **FingerPatch**

Produces a path-based sensation which tracks the 'middle' region of the fingers

#### Inputs:

* `drawFrequency` (scalar): Number of times per second that the path is drawn (default = 70Hz)
* `intensity` (scalar): the intensity to set for the control point data

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* LeapDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Output:

* `out`: Output port, producing control points for the Finger Patch Sensation
  * Sensation-producing: **YES**

-----------------

### **Inverse**

Returns the inverse of a scalar value. If value is 0 the block ouptut will be ( 0 , 0 , 0 ) .

#### Inputs:

* `value` (scalar): Value to calculate inverse

#### Output:
* `out` (scalar): inverse of value  (1 / value)
  * Sensation-producing: **NO**

-----------------

### **SetIntensity**

Use to set the intensity (strength) of a Sensation.

#### Inputs:

* `intensity` (scalar): the intensity to set for the control point data
* `point` (uhsclVector3_t): Control point x-y-z position

#### Output:
`out` (uhsclVector4_t): Control point with modified intensity
  * Sensation-producing: **NO**

-----------------

### **Lerp**

Linearly interpolates between x0 and x1.
For a value x in the interval ( y0 , y1 ), the corresponding value is interpolated in the range ( x0 , x1 ) .

#### Inputs:

* `x` (scalar): Value to vary in the interpolation range ( y0 , y1 )
* `x0` (scalar): The initial interpolated value
* `x1` (scalar): The end interpolated value
* `y0` (scalar): The initial value of the interpolation range (defaults 0.0)
* `y1` (scalar): The end value of the interpolation range

#### Output:
* `out` (scalar): Outputs the interpolated value for x
  * Sensation-producing: **NO**

-----------------

### **LinePath**

Outputs a linear path between two endpoints.

#### Inputs:

* `endpointA` (uhsclVector3_t): The start point of the line path.
* `endpointB` (uhsclVector3_t): The end point of the line path.

#### Output:
* `out` (path): A line between the given endpoints.
  * Sensation-producing: **NO**

-----------------

### **HandScan**

Produces a horizontal line that moves from the center of the palm to the tip of the middle finger.

#### Inputs:

* `duration` (scalar) The amount of time in seconds the journey from the palm to the tip of the middle finger should take (default = 2.0 seconds)
* `barLength` (scalar) The length in metres of the line drawn horizontally across the hand (default = 0.1m)
* `drawFrequency` (scalar) The number of times per second the horizontal line should be drawn (default = 70)

This block has further inputs which are expected to be supplied via automapping, provided by the following Data Sources:
* LeapDataSource
* EmitterDataSource
* SensationSpaceToVirtualSpaceDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t): A horizontal line that scans from the palm to the fingertips.
  * Sensation-producing: **YES**

-----------------

### **Scan**

Produces a path representing a line `barLength`m long pointing along `barDirection` which takes `duration`s to move between `animationPathStart` and `animationPathEnd` before stopping all output.

#### Inputs:

* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `duration` (scalar): The duration (in seconds) the sensation should take to move from `animationPathStart` to `animationPathEnd`
* `barLength` (scalar): The length of the "bar" to draw
* `barDirection` (uhsclVector3_t): The orientation the "bar" should point in as it travels
* `animationPathStart` (uhsclVector3_t): The origin point of the bar's journey (the bar's center will start here)
* `animationPathEnd` (uhsclVector3_t): The destination point of the bar's journey (the bar's center will finish here)

#### Outputs:

* `out` (path):
  * Sensation-producing: **NO**

-----------------

### **LineSensation**

Outputs a line, with given endpoints, at the given offset

#### Inputs:

* `endpointA` (Point): the start point of the line in _Virtual Space_ 
                       (bound to a Unity transform, fetching the world position of the point)
* `endpointB` (Point): the end point of the line in _Virtual Space_ 
                       (bound to a Unity transform, fetching the world position of the point)
* `intensity` (scalar): Sets the intensity (strength) of the Sensation (default = 1.0)
* `drawFrequency` (scalar): Number of times per second that the Line path is drawn (default = 125Hz)

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Output:

* `out` (uhsclVector3_t): A line of the given endpoints and `offsetInVirtualSpace`
  * Sensation-producing: **YES**

-----------------

### **LineToQuadIntersection**

Outputs the line segment resulting from the intersection of a line with the quad.
That line and the quad must be coplanar, if they are not, the result is undetermined.

#### Inputs:

* `line` (OptionalLine): the line that intersects the quad
* `quad` (Quad): the quad to be intersected

#### Output:

* `out` (OptionalLineSegment): A line segment representing the intersection of the line and the quad.
                               If there is no intersection or the line input is invalid, the block
                               returns an invalid line segment.
 * Sensation-producing: **NO**

-----------------

### **LissajousPath**

Outputs a Lissajous curve path, used to produce LissajousSensation.

#### Inputs:

* `sizeX` (scalar): the amplitude of the Lissajous path along the X-axis in Sensation Space (default = 0.01)
* `sizeY` (scalar): the amplitude of the Lissajous path along the Y-axis in Sensation Space (default = 0.01)
* `paramA` (scalar): the A parameter of the Lissajous curve (default = 3)
* `paramB` (scalar): the B parameter of the Lissajous curve (default = 2)

#### Output:

* `out` (path): A Lissajous curve specified by its inputs
  * Sensation-producing: **NO**

-----------------

### **LissajousSensation**

Lissajous Sensation defined using Size X, Size Y, Parameter A, Parameter Y, drawFrequency.

The Lissajous parametric equations in X,Y axes are defined as:

x = SizeX*cos(A*2*pi*f*t)
y = SizeY*sin(B*2*pi*f*t)

Visually, the ratio of A/B parameters determines the number of "lobes" of the Lissajous figure. For example, a ratio of 3/1 or 1/3 produces a figure with three major lobes.

* Is Transformable

#### Inputs:

* `sizeX` (scalar): the amplitude of the Lissajous along the X-axis in meters (default = 0.01)
* `sizeY` (scalar): the amplitude of the Lissajous along the Y-axis in meters (default = 0.01)
* `paramA` (scalar): the A parameter of the Lissajous curve (default = 3)
* `paramB` (scalar): the B parameter of the Lissajous curve (default = 2)
* `intensity` (scalar): Sets the intensity (strength) of the Sensation (default = 1.0)
* `drawFrequency` (scalar): Number of times per second that the Lissajous path is drawn (default = 40Hz)

This block has inputs that we recommend you provide via automapping by including the following Data Sources in your scene:
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:
* `out` (uhhsclVector4_t): A lissjous figure centred around `offset` with provided parameters
  * Sensation-producing: **YES**

-----------------

### **ForcefieldLine**

Calculates the intersection between a single palm and a virtual 'Forcefield' quad represented by two vectors and a point in space.
Note: ForcefieldLine is strictly a Sensation-Producing Block, but is marked as "Non-Sensation Producing" because
the [Forcefield](#Forcefield) Block can be used with one or two hands.

#### Inputs:

* `forcefieldCenter` (uhsclVector3_t): center of the force field
* `forcefieldUp` (uhsclVector3_t): vector representing one of the axis the force field is oriented
                                   The size of this vector is the length of the force field divided by 2
* `forcefieldRight` (uhsclVector3_t): vector perpendicular to forcefieldUp
                                      The size of this vector is the width of the force field divided by 2
                                      
This block has further inputs which are expected to be supplied via automapping, provided by the following Data Sources:
* LeapDataSource
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t): a line sensation at the intersection between hand and Forcefield quad.
  * Sensation-producing: **NO**
  
### **Forcefield**

Based upon [ForcefieldLine](#ForcefieldLine) Block, this Sensation allows up to two hands to interact with a virtual 'Forcefield' quad.
When two hands are present, the line intersection path switches rapidly between left and right hands, to give the illusion two hands 
interacting with the Forcefield simulataneously.

#### Inputs:

For both left_ and right_ hands: 

* `forcefieldCenter` (uhsclVector3_t): center of the force field
* `forcefieldUp` (uhsclVector3_t): vector representing one of the axis the force field is oriented
                                   The size of this vector is the length of the force field divided by 2
* `forcefieldRight` (uhsclVector3_t): vector perpendicular to forcefieldUp
                                      The size of this vector is the width of the force field divided by 2
* `intensity` (scalar): Sets the intensity (strength) of the Sensation (default = 1.0)
* `drawFrequency` (scalar): Number of times per second that the path is drawn (default = 100Hz)

This block has further inputs which are expected to be supplied via automapping, provided by the following Data Sources:
* LeapDataSource
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t): a line sensation at the intersection between the hand(s) and Forcefield quad.
  * Sensation-producing: **YES**  

-----------------

### **PalmPresence**

Functions similarly to the [PalmTrackedCircle](#PalmTrackedCircle), providing a circle centred on the palm,
however comes with a smaller default radius and lower default intensity.

#### Inputs:

* `radius` (scalar) The radius of the circle centered on the palm (default = 0.02)
* `intensity` (scalar) The strength to produce the sensation at (from 0 to 1) (default = 0.5)
* `drawFrequency` (scalar) The number of times per second the Circle should be drawn (default = 70)

This block has further inputs which are expected to be supplied via automapping, provided by the following Data Sources:
* LeapDataSource
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t) A circle of radius `radius` centered around the hand
  * Sensation-producing: **YES**

-----------------

### **PalmTrackedCircle**

See [CircleSensation](#CircleSensation) for further detail

-----------------

### **PalmTrackedDial**

See [DialSensation](#DialSensation) for further detail

-----------------

### **PalmTrackedPulsingCircle**

Functions similarly to the [PalmTrackedCircle](#PalmTrackedCircle), providing a circle centred on the palm,
however the radius of the circle moves between two determined radii over a determined period, after
which emission stops.

#### Inputs:

* `startRadius` (scalar) The initial radius of the circle to be drawn (default = 0.01)
* `endRadius` (scalar) The final radius of the circle to be drawn (default = 0.05)
* `pulsePeriod` (scalar) The amount of time (in seconds) it should take to transition from `startRadius` to `endRadius`
* `intensity` (scalar) The strength to produce the sensation at (from 0 to 1) (default = 1.0)
* `drawFrequency` (scalar) The number of times per second the Circle should be drawn (default = 70)

This block has further inputs which are expected to be supplied via automapping, provided by the following Data Sources:
* LeapDataSource
* SensationSpaceToVirtualSpaceDataSource
* EmitterDataSource
* VirtualSpaceToEmitterSpaceDataSource

#### Outputs:

* `out` (uhsclVector3_t) A circle centered around the hand whose radius changes over time, before stopping after the period ends
  * Sensation-producing: *YES**

-----------------

### **PlaneToPlaneIntersection**

Outputs the line resulting from the intersection of two planes, or an invalid line if the planes do not intersect.

#### Inputs:

* `normal0` (uhsclVector3_t): normal vector of the first plane
* `point0` (Point): point in the first plane
* `normal1` (uhsclVector3_t): normal vector of the second plane
* `point1` (Point): point in the second plane

#### Output:

* `out` (OptionalLine): A line representing the intersection between the two planes.
                        If there is no intersection the block returns an invalid line.
 * Sensation-producing: **NO**

-----------------

### **Point**

Outputs an intensity-modulated control point offset 0.2 in Z. This can
be used as a trivial Sensation-producing Block for test purposes.

#### Inputs:

* `t` (time): The time since the start of a Block's evaluation (in seconds).

#### Output:
* `out` (uhsclVector4_t): Control point with modulated intensity offset by 0.2 in Z.
  * Sensation-producing: **YES**

-----------------

### **PointPair**

Returns a pair of points (`negative` and `positive`) offset by a supplied `distance` in a given `direction`

#### Inputs:

* `direction` (uhsclVector3_t): The directional vector along which to offset the points from the origin
* `distance` (Scalar): The distance (in metres) to offset the points along the specified vector

#### Outputs:

* `positive` (uhsclVector3_t): A point positively offset by half of `distance` along `direction`
  * Sensation-producing: **NO**
* `negative` (uhsclVector3_t): A point negatively offset by half of `distance` along `direction`
  * Sensation-producing: **NO**

-----------------

### **PolylinePath**

Outputs a polyline path defined by a list of points.

To provide values to the `points` input you will need to construct a list using the `EmptyList` and `ListAppend` blocks. The sensation_helpers Python module provides a function to help with this, expandListToIndividualInputs.

#### Inputs:

* `points` (list of uhsclVector3_t): The points defining the polyline path

#### Outputs:
* `out` (path): A polyline path between `points`
  * Sensation-producing: **NO**

-----------------

### **Polyline6**

A polyline Sensation driven by 6 point inputs (point0-point5), which define sequential line segments of a polyline path.
The default shape produced by Polyline6 is a pentagon.

* Is Transformable

#### Inputs:

* `point0` (uhsclVector3_t): the position of the 1st polyline point
* `point1` (uhsclVector3_t): the position of the 2nd polyline point
* `point2` (uhsclVector3_t): the position of the 3rd polyline point
* `point3` (uhsclVector3_t): the position of the 4th polyline point
* `point4` (uhsclVector3_t): the position of the 5th polyline point
* `point5` (uhsclVector3_t): the position of the 6th polyline point
* `intensity` (scalar): Sets the intensity (strength) of the Sensation (default = 1.0)
* `drawFrequency` (scalar): Number of times per second that the Polyline path is drawn (default = 70Hz)

#### Outputs:

* `out` (uhsclVector3_t): A polyline path-based Sensation, defined by its 6 point inputs
  * Sensation-producing: **YES**

-----------------

### **QuadFromVectors**

Outputs quad defined by two vectors and a center

#### Inputs:

* `up` (uhsclVector3_t): first vector used to define the quad. The size of the vector represents half the length of the quad
* `right` (uhsclVector3_t): second vector used to define the quad. The size of the vector represents half the width of the quad
* `center` (uhsclVector3_t): center of the quad

#### Outputs:

* `out` (Quad): Quad defined by the two vectors and center
   * Sensation-producing: **NO**

-----------------

### **QuadToQuadIntersection**

Calculates the line segment resulting from the intersection of two quads.
If the quads do not intersect the result the block returns two points at the origin.

#### Inputs:

* `up0` (uhsclVector3_t): first vector used to define the first quad. The size of the vector represents half the length of the quad
* `right0` (uhsclVector3_t): second vector used to define the first quad, perpendicular to up0. The size of the vector represents half the width of the quad
* `center0` (uhsclVector3_t): center of the first quad
* `up1` (uhsclVector3_t): first vector used to define the second quad. The size of the vector represents half the length of the quad
* `right1` (uhsclVector3_t): second vector used to define the second quad, perpendicular to up1. The size of the vector represents half the width of the quad
* `center1` (uhsclVector3_t): center of the second quad

#### Outputs:

* `endpointA` (uhsclVector3_t): First point of the line segment resulting from the intersection of the two quads, or a point at (0,0,0) if there is no intersection
* `endpointB` (uhsclVector3_t): Second point of the line segment resulting from the intersection of the two quads, or a point at (0,0,0) if there is no intersection
  * Sensation-producing: **NO**

-----------------

### **RenderPath**

Evaluates a path (e.g. **LinePath**) to produce control point positions.
Note: The RenderPath Block does not produce control point output, unless
it receives a valid path-producing input, to its **path** input.

#### Inputs:

* `path` (path): The path to render
* `drawFrequency`: (scalar): The number of times per second the RenderPath draws one complete path.
* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `renderMode`: (enum) Set this to either "Loop" or "Bounce" to change how the path is drawn

#### Output:
* `out` (uhsclVector3_t): Outputs control point positions (if a valid
                          path has been specified)
  * Sensation-producing: **NO**

-----------------

### **SensationSpaceToVirtualHandSpace**

Transforms a point from Sensation Space to Virtual Hand Space.
Use this Block if your need your Sensation designed in _Sensation Space_ to map to the hand in _Virtual Hand Space_ (provided by the tracking device)
Note: This Block outputs a point in _Virtual Space_ and is typically used in conjunction with a Block that transforms from _Virtual_ to _Emitter Space_.

#### Inputs:

* `point` (uhsclVector3_t): Point in, defined in _Sensation Space_
* `palm_position` (uhsclVector3_t): The position of the hand in _Virtual Space_
* `palm_direction` (uhsclVector3_t): The direction of the hand in _Virtual Space_
* `palm_normal` (uhsclVector3_t): The normal of the hand in _Virtual Space_

#### Output:
* `out` (uhsclVector3_t): Control point out, defined in _Virtual Space_
  * Sensation-producing: **NO**

-----------------

### **SegmentToVector3**

Outputs the two endpoints of the segment as two Vector3.

#### Inputs:

* `segment` (OptionalLineSegment): Segment defined by two points

#### Output:
* `endpointA` (uhsclVector3_t): Vector3 representing the first point of the segment
* `endpointB` (uhsclVector3_t): Vector3 representing the second point of the segment
 * Sensation-producing: **NO**

-----------------

### **TransformPath**

Apply a transform to a path to generate a new path

#### Inputs

* `path` (path): Path to transform
* `transform` (uhsclMatrix4x4_t): transform to apply to `path`

#### Output

* `out` (path): A new path resulting from applying `transform` to `path`
  * Sensation-producing: **NO**

-----------------

### **TransformPoint**

Apply a transform to a point vector and get the result

#### Inputs:

* `lhsMatrix` (uhsclMatrix4x4_t): The matrix transformation to apply to the Vector
* `rhsVector` (uhsclVector3_t): The point to transform

#### Outputs:
* `out` (uhsclVector3_t): The transformed point
  * Sensation-producing: **NO**

-----------------

### **TranslateAlongPath**

Animates the offset of a path (the `objectPath`) along another path
(the `animationPath`) over a period of `duration` seconds

#### Inputs:

* `t` (time): Time within animation duration
* `duration` (uhsclVector3_t): Duration of the animation along the
                               animation path in seconds
* `direction` (scalar): if equals ( 1 , 0 , 0 ) the direction of the
                        sensation is inverted, if ( 0 , 0 , 0 ) the
                        direction doesn't change
* `animationPath` (path): Path that the translated origin of the object
                          path will follow
* `objectPath` (path): Path defining the shape to be animated along the
                       animation path

#### Outputs:
* `out` (uhsclVector3_t): A point representing `t` / `duration` of the way through the journey along the combined path
  * Sensation-producing: **NO**

-----------------

### **Matrix4Multiply***

Multiplies two matrices (`lhs` and `rhs`) together and returns the output

#### Inputs:

* `lhs` (uhsclMatrix4x4_t): The matrix for the left hand side of the multiplication
* `rhs` (uhsclMatrix4x4_t): The matrix for the right hand side of the multiplication

#### Outputs:

* `out` (uhschlMatrix4x4_t): The resulting matrix
  * Sensation-producing: **NO**

-----------------

### **IntensityModulation**

Outputs a point at an input position whose intensity modulates at a specified frequency

#### Inputs:
* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `point` (uhsclVector3_t): The position at which to return the output
* `modulationFrequency` (scalar): The frequency (in Hz) at which to modulate 
                                  the intensity of the point.

#### Outputs:
* `out` (uhsclVector4_t): A point with intensity reflecting the modulation at given time
  * Sensation-producing: **NO**

-----------------

### **IntensityWave**

Output a scalar value representing a scalar waveform with specified frequency

#### Inputs:
* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `modulationFrequency` (scalar): The frequency (in Hz) at which to modulate
                                  the intensity of the point.

#### Outputs:
* `out` (scalar): A value representing the value of the sinwave of
                  `modulationFrequency` at given time
 * Sensation-producing: **NO**

-----------------

### **ProjectPathOntoPlane**

A block that produces a path that is an oblique, parallel projection of a given path input.
This Block can be used to create the illusion of extruded 3D shapes by projecting 
the cross-section of the shape onto the plane of the hand.

#### Inputs:

* `path` (path): The path input which is to be projected
* `projectionDirection` (uhsclVector3_t): The direction vector of the plane projection
* `planeNormal` (uhsclVector3_t): The normal vector of the plane on which to project onto.
* `planePoint` (uhsclVector3_t): A point which lies on the plane on which to project onto.

#### Outputs:

* `out` (path): The resultant projected path
* `valid` (boolean): True if the path projection is valid, False otherwise.
 * Sensation-producing: **NO**
-----------------

### **TriangleWave**

Outputs a value representing a given time on a triangle wave form, moving linearly between
two specified values over a specified period

#### Inputs:
* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `minValue` (scalar): The minimum value for the function to return
* `maxValue` (scalar): The maximum value for the function to return
* `period` (scalar): The duration in seconds of one transition from `minVal` to `maxVal` **and back**

 * Sensation-producing: **NO**

-----------------

### **TwoHandsMux**

Block whose inputs correspond to tracking data corresponding to both the left and right hands,
and whose outputs select either the data for the left or the right hand, alternating which hand
is selected regularly over time.

* When only one hand is preset, the tracking data for that hand is always selected by the outputs.
* When no hands are present, the block outputs will evaluate to (0, 0, 0).

This block can be used to create a 2-handed version of a 1-handed block, by connecting the outputs
of this block to the inputs of the 1-handed block.

#### Inputs:

* `t` (time): The time since the start of a Block's evaluation (in seconds).
* `handSwitchingPeriod` (scalar): Duration the sensation evaluates on each hand
* `leftHand_present` (boolean): True if left hand is present, false otherwise
* `rightHand_present` (boolean): True if right hand is present, false otherwise
* ... PLUS all hand inputs described in LeapDataSource section above, prefixed with `leftHand_` and `rightHand_`. 

#### Outputs:

* Hand tracking data for left / right hand, selected regularly over time, based on handSwitchingPeriod.
 * Sensation-producing: **NO**