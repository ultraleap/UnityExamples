from VectorOperations import *

def planeLineIntersection(planeNormal, planePoint, lineDirection, linePoint):

    # print("planeLineIntersection(%s, %s, %s, %s)" % (str(planeNormal), str(planePoint), str(lineDirection), str(linePoint)))

    # See https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
    denominator = dotProduct(planeNormal, lineDirection)

    if abs(denominator) == 0:
        return None

    numerator = dotProduct(vectorSubtract(planePoint, linePoint), planeNormal)

    d = numerator / denominator

    r = tuple(vectorAdd(scalarMultiply(d, lineDirection), linePoint))
    # print("-> %s" % str(r))
    return r


from pysensationcore import *

block = defineBlock("PlaneLineIntersection")
defineInputs(block, "planeNormal", "planePoint", "lineDirection", "linePoint")
defineOutputs(block, "out", "intersected")
setMetaData(block.out, "Sensation-Producing", False)
setMetaData(block.intersected, "Sensation-Producing", False)


def outBehaviour(inputs):
    return planeLineIntersection(inputs[0], inputs[1], inputs[2], inputs[3])

defineBlockOutputBehaviour(block.out, outBehaviour)


def intersectedBehaviour(inputs):
    planeNormal = inputs[0]
    lineDirection = inputs[2]

    dot = dotProduct(planeNormal, lineDirection)

    if dot == 0:
        return (0,0,0) # False
    else:
        return (1,0,0) # True

defineBlockOutputBehaviour(block.intersected, intersectedBehaviour)