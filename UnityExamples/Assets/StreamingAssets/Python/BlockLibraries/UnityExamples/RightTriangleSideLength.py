from pysensationcore import *

import math

b = defineBlock("RightTriangleSideLength")
defineInputs(b, "hypotenuse", "side")
defineOutputs(b, "out")
setMetaData(b.out, "Sensation-Producing", False)


def behaviour(inputs):
    hypotenuse = inputs[0][0]
    side = inputs[1][0]

    try:
        otherside = math.sqrt(hypotenuse*hypotenuse - side*side)
    except:
        print("otherside(%f,%f) error" % (hypotenuse, side))
        return

    return (otherside, 0, 0)


defineBlockOutputBehaviour(b.out, behaviour)