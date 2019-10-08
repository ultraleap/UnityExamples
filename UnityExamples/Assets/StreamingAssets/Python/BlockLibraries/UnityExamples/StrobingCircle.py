# === Strobing Circle Block ===
# A Circle Sensation which Strobes between two positions
from pysensationcore import *
import Generators
import Ops
import Mux
import sensation_helpers as sh

# Define our Circle Path to use for moving around the hand
circlePathInstance = createInstance("CirclePath", "circleInstance")

# A Mux2 to do the Strobing Logic
strobeMux = createInstance("Mux2", "mux2")

# A wave which oscilates between 0 and 1 with a given period
onOffWave = createInstance("BinaryWave", "square")

# connect the output of the BinaryBlock to the StrobeMux
connect(onOffWave.out, strobeMux.selector)

composeTransformInstance = createInstance("ComposeTransform", "composeTransform")
connect(Constant((1, 0, 0)), composeTransformInstance.x)
connect(Constant((0, 1, 0)), composeTransformInstance.y)
connect(Constant((0, 0, 1)), composeTransformInstance.z)
connect(strobeMux.out, composeTransformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPath")
connect(composeTransformInstance.out, transformPathInstance.transform)
connect(circlePathInstance.out, transformPathInstance.path)

# Create hand-tracked Ripple Sensation from output of TransformPath
circleStrobe = sh.createSensationFromPath("StrobingCircle",
                                                {
                                                    ("t", onOffWave.t) : (0,0,0),
                                                    ("originA", strobeMux.input0) : (-0.04, 0.0, 0.0),
                                                    ("originB", strobeMux.input1) : (0.04, 0.0, 0.0),
                                                    ("period", onOffWave.period) : (0.1, 0.0, 0.0),
                                                    ("radius", circlePathInstance.radius) : (0.01, 0.0, 0.0)
                                                },
                                                output = transformPathInstance.out,
                                                drawFrequency = 70)

setMetaData(circleStrobe.period, "Type", "Scalar")
setMetaData(circleStrobe.radius, "Type", "Scalar")