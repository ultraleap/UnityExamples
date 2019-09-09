from pysensationcore import *
from sensation_helpers import *

import NearestPointOnLine
import Ops
import math

# A path constrained to move along a 'rail' defined by a direction and a point on the rail
block = defineBlock("Cone")
defineInputs(block,
             "baseOrigin",
             "coneDirection",
             "baseRadius",
             "minHeight",
             "maxHeight",
             "palm_position"
             )
forwardTiltAngleDegrees = 0
defineBlockInputDefaultValue(block.coneDirection, (0, math.cos(math.pi*forwardTiltAngleDegrees/360), math.sin(math.pi*forwardTiltAngleDegrees/360)))
defineBlockInputDefaultValue(block.baseOrigin, (0.0, 0.05, 0.0))
defineBlockInputDefaultValue(block.baseRadius, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(block.minHeight, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(block.maxHeight, (0.4, 0.0, 0.0))
defineOutputs(block, "out")
setMetaData(block.out, "Sensation-Producing", True)
setMetaData(block.palm_position, "Input-Visibility", False)
setMetaData(block.coneDirection, "Input-Visibility", False)
setMetaData(block.baseRadius, "Type", "Scalar")
setMetaData(block.baseOrigin, "Type", "Point")
setMetaData(block.minHeight, "Type", "Scalar")
setMetaData(block.maxHeight, "Type", "Scalar")

palmPosition = block.palm_position
radius = block.baseRadius

coneCircle = createInstance("CirclePath", "coneCircle")
path = coneCircle.out

# We will linearly interpolate the radius of the circle, between min/maxHeight
lerpInstance = createInstance("Lerp", "lerp")

connect(block.minHeight, lerpInstance.y0)
connect(block.maxHeight, lerpInstance.y1)

palmHeight = createInstance("GetY","getY")
connect(block.palm_position, palmHeight.vector3)
connect(palmHeight.y, lerpInstance.x)

connect(block.baseRadius, lerpInstance.x0)
connect(Constant((0.0001,0,0)), lerpInstance.x1)

nearestPointOnRail = createInstance("NearestPointOnLine", "nearestPointOnRail")
connect(block.coneDirection, nearestPointOnRail.lineDirection)
connect(block.baseOrigin, nearestPointOnRail.linePoint)
connect(palmPosition, nearestPointOnRail.point)
connect(lerpInstance.out, coneCircle.radius)
pointOnRail = nearestPointOnRail.nearestPointOnLine

# A Comparator, to return 1 if inside range, and 0 if not...
evalOnlyIfIntersecting = createInstance("ValueInRange", "inRange")
connect(block.minHeight, evalOnlyIfIntersecting.min)
connect(block.maxHeight, evalOnlyIfIntersecting.max)
connect(palmHeight.y, evalOnlyIfIntersecting.x)
connect(Constant((1,0,0)), evalOnlyIfIntersecting.returnValueIfXInRange)
connect(Constant((0,0,0)), evalOnlyIfIntersecting.returnValueIfXNotInRange)

# For Unity Transform
pathOnRail = transformPathSpace(block, path, (Constant((1,0,0)),Constant((0,0,1)),Constant((0,1,0)),pointOnRail))

focalPoints = createVirtualToPhysicalFocalPointPipeline(block, pathOnRail, 70, renderMode=RenderMode.Loop, intensity= evalOnlyIfIntersecting.out)
connect(focalPoints, block.out)
