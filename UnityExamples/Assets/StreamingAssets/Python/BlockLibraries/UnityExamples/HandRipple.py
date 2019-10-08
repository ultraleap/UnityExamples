# === FingerRipple ===
# A Sensation which uses a randomly positioned Circle at 22 positions of the hand
from pysensationcore import *
import sensation_helpers as sh
import RandomInt
from Mux import createMuxBlockInstance
import HandOperations

# Define our Circle Path to use for moving around the hand
circlePathInstance = createInstance("CirclePath", "circleInstance")

# We have a Block which randomly generates an integer
randomBlockInstance = createInstance("RandomIntGenerator", "randomiser")

# There will be 22 inputs in total, so need a random range of 0-21
connect(Constant((0,0,0)), randomBlockInstance.min)
connect(Constant((21,0,0)), randomBlockInstance.max)

# Define Seed constants for the Random generator blocks
connect(Constant((10,10,10)), randomBlockInstance.seed)

# Use a Mux block to change the offset of the Circle Path
muxBlockInstance = createMuxBlockInstance(22, "muxInstance")

# Connect the output of the RandomInteger to the Mux selector
connect(randomBlockInstance.out, muxBlockInstance.selector)

# Additional Transfer to ensure Circle is oriented to palm
orientToPalmInstance = createInstance("OrientPathToPalm", "orientToPalm")

# The origin of the transform will be the output of the Multiplexer
connect(muxBlockInstance.out, orientToPalmInstance.offset_position)

connect(circlePathInstance.out, orientToPalmInstance.path)

# Create hand-tracked Ripple Sensation from output of TransformPath
rippleSensation = sh.createSensationFromPath("Hand Ripple",
                                                {
                                                    ("t", randomBlockInstance.t) : (0,0,0),
                                                    ("period", randomBlockInstance.period) : (0.05, 0.0, 0.0),
                                                    ("radius", circlePathInstance.radius) : (0.008, 0.0, 0.0),
                                                    ("palm_direction", orientToPalmInstance.palm_direction) : (0, 0, 0),
                                                    ("palm_normal", orientToPalmInstance.palm_normal) : (0, 0, 0),
                                                },
                                                output = orientToPalmInstance.out,
                                                drawFrequency = 70,
                                                definedInVirtualSpace = True
                                                )

setMetaData(rippleSensation.period, "Type", "Scalar")
setMetaData(rippleSensation.radius, "Type", "Scalar")
setMetaData(rippleSensation.palm_direction, "Input-Visibility", False)
setMetaData(rippleSensation.palm_normal, "Input-Visibility", False)

# We will use the 20 joint positions of the hand to the inputs
fingers = ["thumb", "indexFinger", "middleFinger", "ringFinger", "pinkyFinger"]
bones = ["metacarpal", "proximal", "intermediate", "distal"]

inputIndex = 0
for finger in fingers:
    for bone in bones:
        muxInputName = "input%d" % inputIndex
        jointInputName = "%s_%s_position" % (finger, bone)
        defineInputs(rippleSensation, jointInputName)
        setMetaData(getattr(rippleSensation, jointInputName), "Input-Visibility", False)
        connect(getattr(rippleSensation, jointInputName), getattr(muxBlockInstance, muxInputName))
        inputIndex+=1

# Finally we'll hook up the palm and wrist positions too...
positions = ["palm_position", "wrist_position"]
for position in positions:
    defineInputs(rippleSensation, position)
    muxInputName = "input%d" % inputIndex
    setMetaData(getattr(rippleSensation, position), "Input-Visibility", False)
    connect(getattr(rippleSensation, position), getattr(muxBlockInstance, muxInputName))
    inputIndex+=1
