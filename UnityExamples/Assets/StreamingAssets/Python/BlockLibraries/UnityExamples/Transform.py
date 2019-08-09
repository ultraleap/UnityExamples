# Transform Blocks
# These Transform Blocks apply transforms to Vector3 data
from pysensationcore import *
import math

# Leaf Blocks to produce X-Y rotation rows in our transform matrix
# Ref: http://www.fastgraph.com/makegames/3drotation/
xRotateBlock = defineBlock("RotateX")
defineInputs(xRotateBlock, "angle")
defineBlockInputDefaultValue(xRotateBlock.angle, (0,0,0))
def xRowVector(inputs):
    angle = (inputs[0][0]*math.pi)/180
    return (math.cos(angle), -math.sin(angle), 0)

defineOutputs(xRotateBlock, "out")
defineBlockOutputBehaviour(xRotateBlock.out, xRowVector)
setMetaData(xRotateBlock.out, "Sensation-Producing", False)

yRotateBlock = defineBlock("RotateY")
defineInputs(yRotateBlock, "angle")
defineBlockInputDefaultValue(yRotateBlock.angle, (0,0,0))
def yRowVector(inputs):
    angle = (inputs[0][0]*math.pi)/180
    return (math.sin(angle), math.cos(angle), 0)

defineOutputs(yRotateBlock, "out")
defineBlockOutputBehaviour(yRotateBlock.out, yRowVector)
setMetaData(yRotateBlock.out, "Sensation-Producing", False)

# === Rotate2D ===
# A Block which rotates an XY-point given by position input, about an axis, by an angle in degrees
rotateBlock = defineBlock("Rotate2D")
defineInputs(rotateBlock, "position", "axis", "angle")
defineOutputs(rotateBlock, "out")

defineBlockInputDefaultValue(rotateBlock.position, (0.0, 0, 0.0))
defineBlockInputDefaultValue(rotateBlock.axis, (0.0, 0, 0.0))
defineBlockInputDefaultValue(rotateBlock.angle, (0.0, 0, 0.0))

def rotate2D(inputs):
    position = list(inputs[0])
    axis = inputs[1]
    angle = (inputs[2][0]*math.pi)/180
    s = math.sin(angle)
    c = math.cos(angle)
    position[0] -= axis[0]
    position[1] -= axis[1]
    p = [position[0]*c - position[1]*s, position[0]*s + position[1]*c, position[2]]
    p[0] += axis[0]
    p[1] += axis[1]
    return tuple(p)

defineBlockOutputBehaviour(rotateBlock.out, rotate2D)
setMetaData(rotateBlock.out, "Sensation-Producing", False)