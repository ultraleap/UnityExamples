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