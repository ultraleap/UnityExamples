from pysensationcore import *
from sensation_helpers import *

import NearestPointOnLine
import Ops

# A path constrained to move along a 'rail' defined by a direction and a point on the rail
block = defineBlock("RangeSlider")
defineInputs(block,
             "palm_position",
             "wrist_position",
             "middleFinger_distal_proximal",
             "minHeight",
             "maxHeight"
             )

defineBlockInputDefaultValue(block.minHeight, (0.1, 0, 0))
defineBlockInputDefaultValue(block.maxHeight, (0.4, 0, 0))
setMetaData(block.minHeight, "Type", "Scalar")
setMetaData(block.maxHeight, "Type", "Scalar")

# We will linearly interpolate Y-position of the Bar, between wrist and middleFingertip, based on the height of the palm
lerpInstance = createInstance("Lerp", "lerp")

wristPos = createInstance("GetZ","wristY")
connect(block.wrist_position, wristPos.vector3)
connect(wristPos.z, lerpInstance.x0)

tipPos = createInstance("GetZ","tipY")
connect(block.middleFinger_distal_proximal, tipPos.vector3)
connect(tipPos.z, lerpInstance.x1)

connect(block.minHeight, lerpInstance.y0)
connect(block.maxHeight, lerpInstance.y1)

palmHeight = createInstance("GetY","getY")
connect(block.palm_position, palmHeight.vector3)
connect(palmHeight.y, lerpInstance.x)

defineOutputs(block, "out")
setMetaData(block.out, "Sensation-Producing", True)
setMetaData(block.palm_position, "Input-Visibility", True)
setMetaData(block.wrist_position, "Input-Visibility", False)
setMetaData(block.middleFinger_distal_proximal, "Input-Visibility", False)

palmPosition = block.palm_position

bar = createInstance("PolylinePath", "bar")
prefix = "point"
points = createList(5)

halfWidth = 0.05
halfDepth = 0.007
connect(Constant((-halfWidth, 0, -halfDepth)), points["inputs"][0])
connect(Constant((halfWidth, 0, -halfDepth)), points["inputs"][1])
connect(Constant((halfWidth, 0, halfDepth)), points["inputs"][2])
connect(Constant((-halfWidth, 0, halfDepth)), points["inputs"][3])
connect(Constant((-halfWidth, 0, -halfDepth)), points["inputs"][4])

connect(points["output"], bar.points)
path = bar.out

# Compose a Transform which will take transform along the palm of the Hand, between wrist and fingertip
lineTransform = createInstance("ComposeVector3", "composeVec3")
connect(Constant((0,0,0)), lineTransform.x)
connect(lerpInstance.out, lineTransform.z)
connect(palmHeight.y, lineTransform.y)

# Transform from Virtual Space to Emitter space
transformInstance = createInstance("ComposeTransform", "transformInstance")
connect(Constant((1, 0, 0)), transformInstance.x)
connect(Constant((0, 1, 0)), transformInstance.y)
connect(Constant((0, 0, 1)), transformInstance.z)
connect(lineTransform.out, transformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPathInstance")
connect(transformInstance.out, transformPathInstance.transform)
connect(path, transformPathInstance.path)
focalPoints = createVirtualToPhysicalFocalPointPipeline(block, transformPathInstance.out, 70, renderMode=RenderMode.Loop)
connect(focalPoints, block.out)