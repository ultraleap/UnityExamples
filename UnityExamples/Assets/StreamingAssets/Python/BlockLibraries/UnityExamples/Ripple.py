# === Rippling Circle Block ===
# A Sensation which uses a randomly positioned Circle haptic.
from pysensationcore import *
import Generators
import Ops
import sensation_helpers as sh

# Define our Circle Path to use for moving around the hand
circlePathInstance = createInstance("CirclePath", "circleInstance")

# We use Two random generators to provide the offset of the Circle, one for X and one for Y offsets.
randomXBlockInstance = createInstance("RandomWave", "randomX")
randomYBlockInstance = createInstance("RandomWave", "randomY")

# Define Seed constants for the Random generator blocks
connect(Constant((10,10,10)), randomXBlockInstance.seed)
connect(Constant((12,10,10)), randomYBlockInstance.seed)

composeVec3BlockInstance = createInstance("ComposeVector3", "composeVec3")

# Connect RandomX and RandomY to the ComposeVector3 Block
connect(randomXBlockInstance.out, composeVec3BlockInstance.x )
connect(randomYBlockInstance.out, composeVec3BlockInstance.y )
connect(Constant((0.0,0,0)), composeVec3BlockInstance.z )

composeTransformInstance = createInstance("ComposeTransform", "composeTransform")

connect(Constant((1, 0, 0)), composeTransformInstance.x)
connect(Constant((0, 1, 0)), composeTransformInstance.y)
connect(Constant((0, 0, 1)), composeTransformInstance.z)
connect(composeVec3BlockInstance.out, composeTransformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPath")
connect(composeTransformInstance.out, transformPathInstance.transform)
connect(circlePathInstance.out, transformPathInstance.path)

# Create hand-tracked Ripple Sensation from output of TransformPath
rippleSensation = sh.createSensationFromPath("Ripple",
                                                {
                                                    ("t", randomXBlockInstance.t) : (0,0,0),
                                                    ("t", randomYBlockInstance.t) : (0,0,0),
                                                    ("boxXmin", randomXBlockInstance.min) : (-0.04, 0.0, 0.0),
                                                    ("boxXmax", randomXBlockInstance.max) : (0.04, 0.0, 0.0),
                                                    ("boxYmin", randomYBlockInstance.min) : (-0.04, 0.0, 0.0),
                                                    ("boxYmax", randomYBlockInstance.max) : (0.08, 0.0, 0.0),
                                                    ("period", randomXBlockInstance.period) : (0.1, 0.0, 0.0),
                                                    ("period", randomYBlockInstance.period) : (0.1, 0.0, 0.0),
                                                    ("radius", circlePathInstance.radius) : (0.01, 0.0, 0.0),
                                                },
                                                output = transformPathInstance.out,
                                                drawFrequency = 70)

setMetaData(rippleSensation.boxXmin, "Type", "Scalar")
setMetaData(rippleSensation.boxXmax, "Type", "Scalar")
setMetaData(rippleSensation.boxYmin, "Type", "Scalar")
setMetaData(rippleSensation.boxYmax, "Type", "Scalar")
setMetaData(rippleSensation.period, "Type", "Scalar")
setMetaData(rippleSensation.radius, "Type", "Scalar")

setMetaData(rippleSensation, "Allow-Transform", True)
