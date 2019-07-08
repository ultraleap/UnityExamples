from pysensationcore import *
from sensation_helpers import *

import NearestPointOnLine
import RotationTransform
from QuickBlock import createQuickBlock
from Ops import quantizeAndScale

import math

# A notched-indicator that rotates around according to the height of the hand
block = defineBlock("NotchedRotaryControl")
defineInputs(block,
             "railDirection",
             "railPoint",
             "palm_position"
             )
defineBlockInputDefaultValue(block.railDirection, (0, 1, 0))
defineBlockInputDefaultValue(block.railPoint, (0.0, 0.2, 0))
setMetaData(block.railPoint, "Type", "Point")
setMetaData(block.palm_position, "Input-Visibility", False)
defineOutputs(block, "out")

palmPosition = block.palm_position

bar = createInstance("PolylinePath", "bar")
pointCount = 6
points = createList(pointCount)

# Create a Bar which will be rotated based on hand height
halfWidth = 0.05
halfDepth = 0.007
left = -0.007
right = 0.007
bottom = 0.03
top = 0.07
connect(Constant((left, 0, bottom)), points["inputs"][0])
connect(Constant((right, 0, bottom)), points["inputs"][1])
connect(Constant((right, 0, top)), points["inputs"][2])
connect(Constant((left, 0, top)), points["inputs"][3])
connect(Constant((left, 0, bottom)), points["inputs"][4])
connect(Constant((left, 0, bottom)), points["inputs"][5])
path = bar.out

# Connect points List output to the bar PolylinePath 
connect(points["output"], bar.points)

nearestPointOnRail = createInstance("NearestPointOnLine", "nearestPointOnRail")
connect(block.railDirection, nearestPointOnRail.lineDirection)
connect(block.railPoint, nearestPointOnRail.linePoint)
connect(palmPosition, nearestPointOnRail.point)
pointOnRail = nearestPointOnRail.nearestPointOnLine

step = 0.02
notchSize = step/2
scaleFactor = 30

rotationTransform = createInstance("RotationTransform", "rotationTransform")
connect(Constant((0,1,0)), rotationTransform.axis)
angle = createQuickBlock(nearestPointOnRail.distanceFromLinePoint, lambda inputs: (quantizeAndScale(step, scaleFactor, inputs[0][0]),0,0), blockName = "NotchAngle")
connect(angle, rotationTransform.angle)


applyRotation = createInstance("TransformPath", "applyRotation")
connect(rotationTransform.out, applyRotation.transform)
connect(path, applyRotation.path)
rotatedPath = applyRotation.out

pathOnRail = transformPathSpace(block, rotatedPath, (Constant((1,0,0)),Constant((0,1,0)),Constant((0,0,1)),pointOnRail))

intensity = createQuickBlock(nearestPointOnRail.distanceFromLinePoint, lambda inputs: (0 if abs(math.fmod(inputs[0][0]+notchSize/2,step))<notchSize else 1.0,0,0), blockName="NotchIntensity")

focalPoints = createVirtualToPhysicalFocalPointPipeline(block, pathOnRail, 70, intensity=intensity)

connect(focalPoints, block.out)