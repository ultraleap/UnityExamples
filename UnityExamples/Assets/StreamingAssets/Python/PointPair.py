from pysensationcore import *

pointPairBlock = defineBlock("PointPair")
defineInputs(pointPairBlock, "direction", "distance")

defineOutputs(pointPairBlock, "positive", "negative")

def positive(inputs):
    direction = inputs[0]
    distance = inputs[1][0]

    return tuple([distance/2*directionComponent for directionComponent in direction])

def negative(inputs):
    direction = inputs[0]
    distance = inputs[1][0]

    return tuple([-distance/2*directionComponent for directionComponent in direction])

defineBlockOutputBehaviour(pointPairBlock.negative, negative)
defineBlockOutputBehaviour(pointPairBlock.positive, positive)
setMetaData(pointPairBlock.negative, "Sensation-Producing", False)
setMetaData(pointPairBlock.positive, "Sensation-Producing", False)
