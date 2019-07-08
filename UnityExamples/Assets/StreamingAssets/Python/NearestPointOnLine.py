from pysensationcore import *

from VectorOperations import *


b = defineBlock("NearestPointOnLine")
defineInputs(b, "lineDirection",
                "linePoint",
                "point")
defineOutputs(b, "nearestPointOnLine",
                 "distanceFromLinePoint")
setMetaData(b.nearestPointOnLine, "Sensation-Producing", False)

def lengthAlongDirectionToNearestPoint(lineDirection, linePoint, point):
    return dotProduct(lineDirection, vectorSubtract(point, linePoint))

# https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line#Vector_formulation
def nearestPointOnLine(lineDirection, linePoint, point):
    lengthAlongDirectionToNearestPoint = dotProduct(lineDirection, vectorSubtract(point, linePoint))
    nearestPoint = tuple(vectorAdd(linePoint, scalarMultiply(lengthAlongDirectionToNearestPoint, lineDirection)))

    return nearestPoint

def nearestPointOnLineBehaviour(inputs):
    lineDirection = inputs[0]
    linePoint = inputs[1]
    point = inputs[2]

    return tuple(nearestPointOnLine(lineDirection, linePoint, point))

defineBlockOutputBehaviour(b.nearestPointOnLine, nearestPointOnLineBehaviour)


def distanceFromLinePointBehaviour(inputs):
    lineDirection = inputs[0]
    linePoint = inputs[1]
    point = inputs[2]

    return (lengthAlongDirectionToNearestPoint(lineDirection, linePoint, point),0,0)

defineBlockOutputBehaviour(b.distanceFromLinePoint, distanceFromLinePointBehaviour)