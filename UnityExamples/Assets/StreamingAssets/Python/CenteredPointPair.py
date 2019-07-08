from pysensationcore import *
from VectorOperations import *

# As per the PointPair block, but adds an input translating the center point from the origin
pointPairBlock = defineBlock("CenteredPointPair")
defineInputs(pointPairBlock, "direction", "distance", "center")
defineBlockInputDefaultValue(pointPairBlock.center, (0,0,0))

defineOutputs(pointPairBlock, "positive", "negative")

def positive(inputs):
    direction = inputs[0]
    distance = inputs[1][0]
    center = inputs[2]

    return tuple(vectorAdd(center, [distance/2*directionComponent for directionComponent in direction]))

def negative(inputs):
    direction = inputs[0]
    distance = inputs[1][0]
    center = inputs[2]

    return tuple(vectorAdd(center, [-distance/2*directionComponent for directionComponent in direction]))

defineBlockOutputBehaviour(pointPairBlock.negative, negative)
defineBlockOutputBehaviour(pointPairBlock.positive, positive)
setMetaData(pointPairBlock.negative, "Sensation-Producing", False)
setMetaData(pointPairBlock.positive, "Sensation-Producing", False)
