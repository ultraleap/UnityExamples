# === Warning Block ===
# A Sensation which behaves like the Warning Sensation in Sensation Editor
# A horizontal line which randomly jumps around along the Y-axis.
from pysensationcore import *
import sensation_helpers as sh
import RandomInt
import Ops
from Mux import *

# Define our Line Path to use for moving around the hand
linePathInstance = createInstance("LinePath", "circleInstance")

# Path will be 10cm wide
connect(Constant((-0.05,0,0)),linePathInstance.endpointA)
connect(Constant((0.05,0,0)),linePathInstance.endpointB)

# These are the default Y-band offsets for the Warning Sensation
yBandOffsets = [0.01600, 0.07200, -0.04000, 0.04400, -0.01200, 0.10000]
numBands = len(yBandOffsets)

# We need a multiplexer, which selects between these offsets
multiplexer = createInstance("Mux6", "mux6")

# Find a RandomInteger Generator, which will return a random int between 0-4
randomBlockInstance = createInstance("RandomIntGenerator", "randomInstance")
connect(Constant((0,0,0)), randomBlockInstance.min)
connect(Constant((numBands-1,0,0)), randomBlockInstance.max)

# Connect the output of the RandomInteger to the Mux5 selector
connect(randomBlockInstance.out, multiplexer.selector)

# Hook up yBands to Multiplexer inputs
for i in range(numBands):
    connect(Constant((yBandOffsets[i],0,0)), getattr(multiplexer, "input" + str(i)))

composeVec3BlockInstance = createInstance("ComposeVector3", "composeVec3")

# Create a ComposeVector3 Bloack with Y-band Offset
connect(Constant((0.0,0,0)), composeVec3BlockInstance.x )
connect(multiplexer.out, composeVec3BlockInstance.y )
connect(Constant((0.0,0,0)), composeVec3BlockInstance.z )

composeTransformInstance = createInstance("ComposeTransform", "composeTransform")

connect(Constant((1, 0, 0)), composeTransformInstance.x)
connect(Constant((0, 1, 0)), composeTransformInstance.y)
connect(Constant((0, 0, 1)), composeTransformInstance.z)
connect(composeVec3BlockInstance.out, composeTransformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPath")
connect(composeTransformInstance.out, transformPathInstance.transform)
connect(linePathInstance.out, transformPathInstance.path)

# Create Warning Sensation from output of TransformPath
warningSensation = sh.createSensationFromPath("Warning",
                                                {
                                                    ("t", randomBlockInstance.t) : (0,0,0),
                                                    ("period", randomBlockInstance.period) : (1.0/numBands, 0.0, 0.0),
                                                },
                                                output = transformPathInstance.out,
                                                drawFrequency = 70)

setMetaData(warningSensation.period, "Type", "Scalar")
setMetaData(warningSensation, "Allow-Transform", True)
