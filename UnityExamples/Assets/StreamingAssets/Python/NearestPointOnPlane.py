from pysensationcore import *

from VectorOperations import *

b = defineBlock("NearestPointOnPlane")
defineInputs(b, "planeNormal", "planePoint", "point")
defineOutputs(b, "nearestPointOnPlane", "distance")
setMetaData(b.nearestPointOnPlane, "Sensation-Producing", False)
setMetaData(b.distance, "Sensation-Producing", False)


def distanceToPointOnPlane(planeNormal, planePoint, point):
    return abs(dotProduct(planeNormal, vectorSubtract(point, planePoint)))


def nearestPointOnPlane(planeNormal, planePoint, point):
    distance = dotProduct(planeNormal, vectorSubtract(point, planePoint))
    vectorFromPointToPlaneAlongNormal = scalarMultiply(distance, planeNormal)
    return vectorSubtract(point, vectorFromPointToPlaneAlongNormal)


def nearestPointOnPlaneBehaviour(inputs):
    planeNormal = inputs[0]
    planePoint = inputs[1]
    point = inputs[2]

    result = tuple(nearestPointOnPlane(planeNormal, planePoint, point))
    # print("nearestPoint(%s,%s,%s)=%s" % (str(planeNormal),str(planePoint),str(point),str(result)))
    return result


def distanceBehaviour(inputs):
    planeNormal = inputs[0]
    planePoint = inputs[1]
    point = inputs[2]

    result = (distanceToPointOnPlane(planeNormal, planePoint, point), 0, 0)
    # print("distance(%s,%s,%s)=%s" % (str(planeNormal),str(planePoint),str(point),str(result)))
    return result


defineBlockOutputBehaviour(b.nearestPointOnPlane, nearestPointOnPlaneBehaviour)
defineBlockOutputBehaviour(b.distance, distanceBehaviour)