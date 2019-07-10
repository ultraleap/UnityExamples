from pysensationcore import *

import math

# A 'private' block that calculates vectors that can then be composed
# into a matrix (since we can't create a matrix from a python output behaviour)
#
# see http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToMatrix/index.htm

def defineRotationVectorsBlock():
    b = defineBlock("RotationVectors")
    defineInputs(b, "axis", "angle")
    defineOutputs(b, "x", "y", "z")
    setMetaData(b.x, "Sensation-Producing", False)
    setMetaData(b.y, "Sensation-Producing", False)
    setMetaData(b.z, "Sensation-Producing", False)

    def calcIntermediates(axis, angle):
        c = math.cos(angle)
        return (axis[0], #x
                axis[1], #y
                axis[2], #z
                c, #c
                1-c, #t
                math.sin(angle)) #s

    def calcXVector(inputs):
        axis = inputs[0]
        angle = inputs[1][0]

        (x,y,z,c,t,s) = calcIntermediates(axis, angle)

        return (x*x*t + c,
                t*x*y + z*s,
                t*x*z - y*s)

    def calcYVector(inputs):
        axis = inputs[0]
        angle = inputs[1][0]

        (x,y,z,c,t,s) = calcIntermediates(axis, angle)

        return (t*x*y - z*s,
                t*y*y + c,
                t*y*z + x*s)

    def calcZVector(inputs):
        axis = inputs[0]
        angle = inputs[1][0]

        (x,y,z,c,t,s) = calcIntermediates(axis, angle)

        return (t*x*z + y*s,
                t*y*z - x*s,
                t*z*z + c)

    defineBlockOutputBehaviour(b.x, calcXVector)
    defineBlockOutputBehaviour(b.y, calcYVector)
    defineBlockOutputBehaviour(b.z, calcZVector)

    # def test(axis, angle):
    #     print("Axis: %s  Angle: %s ->\n X:%s\n Y:%s\n Z:%s" % (str(axis), str(angle),
    #                                                      str(calcXVector([axis, angle])),
    #                                                      str(calcYVector([axis, angle])),
    #                                                      str(calcZVector([axis, angle]))))
    #
    # test((0,0,1), (math.pi/2, 0, 0))
    # test((0,0,1), (-math.pi/2, 0, 0))
    # test((0,1,0), (math.pi/2, 0, 0))
    # test((0,1,0), (-math.pi/2, 0, 0))
    # test((1,0,0), (math.pi/2, 0, 0))
    # test((1,0,0), (-math.pi/2, 0, 0))

b = defineBlock("RotationTransform")
defineInputs(b, "axis",  # Normal to the plane of rotation for any given point
                "angle") # Angle in radians (direction consistent with the right hand rule)
defineOutputs(b, "out")
setMetaData(b.out, "Sensation-Producing", False)

defineRotationVectorsBlock()
rotationVectors = createInstance("RotationVectors", "rotationVectors")
connect(b.axis, rotationVectors.axis)
connect(b.angle, rotationVectors.angle)

compose = createInstance("ComposeTransform", "compose")
connect(rotationVectors.x, compose.x)
connect(rotationVectors.y, compose.y)
connect(rotationVectors.z, compose.z)
connect(Constant((0,0,0)), compose.o)

connect(compose.out, b.out)


