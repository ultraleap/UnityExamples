# === Rotor Line Block ===
# A Sensation which produces a rotating Line Block
from pysensationcore import *
import math
import Generators
import Transform
import sensation_helpers as sh

# Define our Circle Line to use for moving around the hand
linePathInstance = createInstance("LinePath", "linePathInstance")
connect(Constant((-0.04, 0, 0)), linePathInstance.endpointA)
connect(Constant((0.04, 0, 0)), linePathInstance.endpointB)

# We use a Lerp Block to animate rotation from 0 to 360 degrees
animator = createInstance("SawtoothWave", "sawtooth")
transformXInstance = createInstance("RotateX", "transformX")
connect(animator.out, transformXInstance.angle)

transformYInstance = createInstance("RotateY", "transformY")
connect(animator.out, transformYInstance.angle)

# This Matrix is composed to handle rotation in X-Y Axes in Sensation Space
composeTransformInstance = createInstance("ComposeTransform", "composeTransform")
connect(transformXInstance.out, composeTransformInstance.x)
connect(transformYInstance.out, composeTransformInstance.y)
connect(Constant((0, 0, 1)), composeTransformInstance.z)
connect(Constant((0, 0, 0)), composeTransformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPath")
connect(composeTransformInstance.out, transformPathInstance.transform)
connect(linePathInstance.out, transformPathInstance.path)

# Create hand-tracked Ripple Sensation from output of TransformPath
rotorSensation = sh.createSensationFromPath("Rotor",
                                                {
                                                    ("t" ,animator.t) : (0, 0, 0),
                                                    ("angleStart" ,animator.min) : (0, 0, 0),
                                                    ("angleEnd" ,animator.max) : (360, 0, 0),
                                                    ("period",animator.period) : (1, 0, 0),
                                                },
                                                output = transformPathInstance.out,
                                                drawFrequency = 70)

setMetaData(rotorSensation.angleStart, "Type", "Scalar")
setMetaData(rotorSensation.angleEnd, "Type", "Scalar")
setMetaData(rotorSensation.period, "Type", "Scalar")

setMetaData(rotorSensation, "Allow-Transform", True)
