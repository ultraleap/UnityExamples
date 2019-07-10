from pysensationcore import *
from TwoHandedSensation import *
import sensation_helpers as sh

circleSensationBlock = createInstance("CircleSensation", "circleInstance")

palmTrackedBlock = defineBlock("TrackedCircle")

forwardAnyInputsCreatedViaSensationHelpers(palmTrackedBlock, circleSensationBlock)

# Create Palm Inputs
defineInputs(palmTrackedBlock, "palm_position", "palm_direction", "palm_normal")
defineOutputs(palmTrackedBlock, "out")
setMetaData(palmTrackedBlock.out, "Sensation-Producing", False)

crossProductInst = createInstance("CrossProduct", "crossProduct")
connect(palmTrackedBlock.palm_direction, crossProductInst.lhs)
connect(palmTrackedBlock.palm_normal, crossProductInst.rhs)
	
palmPosition = palmTrackedBlock.palm_position
palmNormal = palmTrackedBlock.palm_normal
palmTransverse = crossProductInst.out
palmDirection = palmTrackedBlock.palm_direction
	
connect(palmPosition, circleSensationBlock.virtualObjectOriginInVirtualSpace)
connect(palmTransverse, circleSensationBlock.virtualObjectXInVirtualSpace)
connect(palmNormal, circleSensationBlock.virtualObjectYInVirtualSpace)
connect(palmDirection, circleSensationBlock.virtualObjectZInVirtualSpace)

unconnectedInputs = ["radius"]

defineInputs(palmTrackedBlock, *unconnectedInputs)
for input in unconnectedInputs:
    connect(getattr(palmTrackedBlock, input), getattr(circleSensationBlock, input))

setMetaData(palmTrackedBlock.radius, "Type", "Scalar")
defineBlockInputDefaultValue(palmTrackedBlock.radius, (0.025,0,0))

connect(circleSensationBlock.out, palmTrackedBlock.out)


# Now for a two-handed Circle...
trackedCircleInst = createInstance("TrackedCircle", "Inst")
twoHandedCircle = makeSensationTwoHanded(trackedCircleInst, "TwoPalmPresence")

unconnectedInputs = ["radius"]

defineInputs(twoHandedCircle, *unconnectedInputs)
for input in unconnectedInputs:
    connect(getattr(twoHandedCircle, input), getattr(trackedCircleInst, input))

setMetaData(twoHandedCircle.radius, "Type", "Scalar")

defineBlockInputDefaultValue(twoHandedCircle.radius, (0.025,0,1))
defineBlockInputDefaultValue(twoHandedCircle.drawFrequency, (140,0,1))
defineBlockInputDefaultValue(twoHandedCircle.handSwitchingPeriod, (0.01,0,0))

defineOutputs(twoHandedCircle, "out")
connect(getattr(trackedCircleInst, "out"), getattr(twoHandedCircle, "out"))
