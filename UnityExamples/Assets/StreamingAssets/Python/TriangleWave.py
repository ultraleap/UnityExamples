from pysensationcore import *
import math

# === TriangleWave Block ===
# A Triangle Wave Generator Block, which oscillates between a min-max value with a period (specified in seconds)
triangleWaveBlock = defineBlock("TriangleWave")
defineInputs(triangleWaveBlock, "t", "minValue", "maxValue", "period")
defineBlockInputDefaultValue(triangleWaveBlock.minValue, (0.01, 0.0, 0.0))
defineBlockInputDefaultValue(triangleWaveBlock.maxValue, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(triangleWaveBlock.period, (1, 0.0, 0.0))

def triangleWaveFunction(inputs):
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0

    f = 1.0/period
    angle = 2*math.pi*f*t
    expression = (((((2*math.asin(math.sin(angle)))/math.pi) / 2)+0.5) * (maxValue-minValue)) + minValue
    return expression, 0, 0

defineOutputs(triangleWaveBlock, "out")
defineBlockOutputBehaviour(triangleWaveBlock.out, triangleWaveFunction)
setMetaData(triangleWaveBlock.out, "Sensation-Producing", False)
