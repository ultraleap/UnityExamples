# Block to return a normalized direction vector from pointA to pointB
from pysensationcore import *
from VectorOperations import vectorSubtract, vectorNormalize

def directionFromPoints(inputs):
    A = inputs[0]
    B = inputs[1]
    vector = list(vectorSubtract(A, B))
    normVector = list(vectorNormalize(vector))
    return (-normVector[2],normVector[1],normVector[0])

# Create a Block which returns the direction vector of two points.
directionBlock = defineBlock("NormalizedDirectionFromTwoPoints")
defineInputs(directionBlock, "pointA", "pointB")
defineBlockInputDefaultValue(directionBlock.pointA, (1,0,0))
defineBlockInputDefaultValue(directionBlock.pointB, (0,0,0))

defineOutputs(directionBlock, "direction")
defineBlockOutputBehaviour(directionBlock.direction, directionFromPoints)

setMetaData(directionBlock.direction, "Sensation-Producing", False)
