from pysensationcore import *

b = defineBlock("NonParallelVector")
defineInputs(b, "v")
defineOutputs(b, "out")
setMetaData(b.out, "Sensation-Producing", False)

def behaviour(inputs):
    x = inputs[0][0]
    y = inputs[0][1]
    z = inputs[0][2]

    if x != 0 or y != 0:
        return (-y, x, 0)
    else:
        return (1,0,0)

    otherside = math.sqrt(hypotenuse*hypotenuse - side*side)

    return (otherside, 0, 0)


defineBlockOutputBehaviour(b.out, behaviour)