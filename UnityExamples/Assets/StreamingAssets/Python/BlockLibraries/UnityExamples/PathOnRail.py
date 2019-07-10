from pysensationcore import *
from sensation_helpers import *

import NearestPointOnLine
import math

# A path constrained to move along a 'rail' defined by a direction and a point on the rail
block = defineBlock("PathOnRail")
defineInputs(block,
             "railDirection",
             "railOrigin",
             "palm_position"
             )
forwardTiltAngleDegrees = 80
defineBlockInputDefaultValue(block.railDirection, (0, math.sin(math.pi*forwardTiltAngleDegrees/360), math.cos(math.pi*forwardTiltAngleDegrees/360)))
defineBlockInputDefaultValue(block.railOrigin, (0.0, 0.2, 0.0))
defineOutputs(block, "out")
setMetaData(block.out, "Sensation-Producing", True)
setMetaData(block.railOrigin, "Type", "Point")
setMetaData(block.palm_position, "Input-Visibility", False)

palmPosition = block.palm_position

bar = createInstance("PolylinePath", "bar")
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

nearestPointOnRail = createInstance("NearestPointOnLine", "nearestPointOnRail")
connect(block.railDirection, nearestPointOnRail.lineDirection)
connect(block.railOrigin, nearestPointOnRail.linePoint)
connect(palmPosition, nearestPointOnRail.point)
pointOnRail = nearestPointOnRail.nearestPointOnLine

pathOnRail = transformPathSpace(block, path, (Constant((1,0,0)),Constant((0,1,0)),Constant((0,0,1)),pointOnRail))

focalPoints = createVirtualToPhysicalFocalPointPipeline(block, pathOnRail, 70, renderMode=RenderMode.Bounce)

connect(focalPoints, block.out)