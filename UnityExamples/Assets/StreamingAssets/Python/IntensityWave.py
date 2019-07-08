from pysensationcore import *

import math

intensityWaveBlock = defineBlock("IntensityWave")
defineInputs(intensityWaveBlock, "t", "modulationFrequency")
defineBlockInputDefaultValue(intensityWaveBlock.modulationFrequency, (143.0, 0.0, 0.0))

def cosineWave(inputs):
    time = inputs[0][0]
    modulationFrequency = inputs[1][0]

    intensity = 0.5 * (1 - math.cos(2 * math.pi * time * modulationFrequency));
    return intensity,0,0

defineOutputs(intensityWaveBlock, "out")
defineBlockOutputBehaviour(intensityWaveBlock.out, cosineWave)
setMetaData(intensityWaveBlock.out, "Sensation-Producing", False)
