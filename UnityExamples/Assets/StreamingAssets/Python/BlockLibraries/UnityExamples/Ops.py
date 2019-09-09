# Ops Blocks
# These Operation Blocks can be used to modify behaviour of Blocks
# e.g. time can be reversed to change the direction of an expanding Circle
from pysensationcore import *
import math

# === ComposeVector3 ===
# A Block which composes a Vector3 (X-Y-Z) from three tuple inputs.
# Where input1, input2, input3 are tuples of length=3 -> returns a tuple (input1[0], input2[0], input3[0])
composeVector3Block = defineBlock("ComposeVector3")
defineInputs(composeVector3Block, "x", "y", "z")
defineBlockInputDefaultValue(composeVector3Block.x, (0, 0, 0))
defineBlockInputDefaultValue(composeVector3Block.y, (0, 0, 0))
defineBlockInputDefaultValue(composeVector3Block.z, (0, 0, 0))
defineOutputs(composeVector3Block, "out")

def composeVector3Function(inputs):
    x = inputs[0][0]
    y = inputs[1][0]
    z = inputs[2][0]
    return x,y,z

defineBlockOutputBehaviour(composeVector3Block.out, composeVector3Function)
setMetaData(composeVector3Block.out, "Sensation-Producing", False)

# Return the X Component (1st value) of a Vector3 as a Tuple (X,0,0)
xComponent = defineBlock("GetX")
defineInputs(xComponent, "vector3")
defineOutputs(xComponent, "x")

def getX(inputs):
	return (inputs[0][0],0,0)

defineBlockInputDefaultValue(xComponent.vector3, (0,0,0))
defineBlockOutputBehaviour(xComponent.x, getX)
setMetaData(xComponent.x, "Sensation-Producing", False)

# Return the Y-Component (2nd value) of a Vector3 as a Tuple (Y,0,0)
yComponent = defineBlock("GetY")
defineInputs(yComponent, "vector3")
defineOutputs(yComponent, "y")

def getY(inputs):
	return (inputs[0][1],0,0)

defineBlockInputDefaultValue(yComponent.vector3, (0,0,0))
defineBlockOutputBehaviour(yComponent.y, getY)
setMetaData(yComponent.y, "Sensation-Producing", False)

# Return the Z-Component (third value) of a Vector3 as a Tuple (Z,0,0)
zComponent = defineBlock("GetZ")
defineInputs(zComponent, "vector3")
defineOutputs(zComponent, "z")

def getZ(inputs):
	return (inputs[0][2],0,0)

defineBlockInputDefaultValue(zComponent.vector3, (0,0,0))
defineBlockOutputBehaviour(zComponent.z, getZ)
setMetaData(zComponent.z, "Sensation-Producing", False)

def quantizeAndScale(step, scaleFactor, x):
    x = x - math.fmod(x,step)
    return x * scaleFactor

# === ValueInRange ===
# Like Comparator, but allows inRange/outRange value to be defined.
comparatorRangeBlock = defineBlock("ValueInRange")
defineInputs(comparatorRangeBlock, "x", "min", "max", "returnValueIfXInRange", "returnValueIfXNotInRange")
defineBlockInputDefaultValue(comparatorRangeBlock.x, (0, 0, 0))
defineBlockInputDefaultValue(comparatorRangeBlock.min, (0, 0, 0))
defineBlockInputDefaultValue(comparatorRangeBlock.max, (0, 0, 0))
defineOutputs(comparatorRangeBlock, "out")

def compareRange(inputs):
	value = inputs[0][0]
	minValue = inputs[1][0]
	maxValue = inputs[2][0]
	returnInRangeValue = inputs[3]
	returnOutRangeValue = inputs[4]

	inRange = (value >= minValue) and (value <= maxValue)
	if inRange:
		return returnInRangeValue
	else:
		return returnOutRangeValue

defineBlockOutputBehaviour(comparatorRangeBlock.out, compareRange)
setMetaData(comparatorRangeBlock.out, "Sensation-Producing", False)
