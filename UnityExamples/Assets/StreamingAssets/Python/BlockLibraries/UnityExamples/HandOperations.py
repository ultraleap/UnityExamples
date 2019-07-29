# A Block which returns 0 if left present, 1 if right present, and 2 if both present
# Note: Hand presence is determined by the AutoMapper
from pysensationcore import *

handInfo = defineBlock("HandInfo") 
defineInputs(handInfo, "leftHand_present", "rightHand_present")
defineBlockInputDefaultValue(handInfo.leftHand_present, (0, 0, 0))
defineBlockInputDefaultValue(handInfo.rightHand_present, (0, 0, 0))

def handInfoBehaviour(inputs):
    leftPresent = inputs[0][0]
    rightPresent = inputs[1][0]

    # Left hand is present
    if leftPresent > 0.5 and rightPresent < 0.5:
        return (0,0,0)

    # Right hand is present
    elif leftPresent < 0.5 and rightPresent > 0.5:
        return (1,0,0)

    # Both hands are present
    elif leftPresent > 0.5 and rightPresent > 0.5:
        return (2,0,0)

    # No hands are present
    else:
        return (-1,0,0)

defineOutputs(handInfo, "option")
setMetaData(handInfo.option, "Sensation-Producing", False)
defineBlockOutputBehaviour(handInfo.option, handInfoBehaviour)

# OrientPathToPalm
# Orients a 2D Path (designed in X-Y of Sensation Space) to be in the same plane as the palm
orientToPalmBlock = defineBlock("OrientPathToPalm")

defineInputs(orientToPalmBlock, "path", "palm_direction","palm_normal", "offset_position")
defineBlockInputDefaultValue(orientToPalmBlock.offset_position, (0, 0, 0))
defineOutputs(orientToPalmBlock, "out")
setMetaData(orientToPalmBlock.out, "Sensation-Producing", False)

crossProductInst = createInstance("CrossProduct", "crossProduct")

# Compose a Transform based on the palm orientation to orient the path input
composeTransform = createInstance("ComposeTransform", "ComposeObjInVtlSpaceTform")
connect(crossProductInst.out, composeTransform.x)
connect(orientToPalmBlock.offset_position, composeTransform.o)

transformPath = createInstance("TransformPath", "rotatePath")

connect(orientToPalmBlock.palm_direction, crossProductInst.lhs)
connect(orientToPalmBlock.palm_normal, crossProductInst.rhs)
connect(orientToPalmBlock.palm_direction, composeTransform.y)
connect(orientToPalmBlock.palm_normal, composeTransform.z)

# Input Path -> TransformPath
connect(orientToPalmBlock.path, transformPath.path)
connect(composeTransform.out, transformPath.transform)
connect(transformPath.out, orientToPalmBlock.out)