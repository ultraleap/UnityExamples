from pysensationcore import *

import math

# === IntensityModulation Block ===
# A Block which can be used to modulate the intensity (strength) of a Sensation at a supplied frequency
intensityModulationBlock = defineBlock("IntensityModulation")
defineInputs(intensityModulationBlock,
            "t",
            "point",
            "modulationFrequency")

defineBlockInputDefaultValue(intensityModulationBlock.modulationFrequency, (143.0, 0.0, 0.0))
setMetaData(intensityModulationBlock.modulationFrequency, "Type", "Scalar")

def modulateIntensity(inputs):
    time = inputs[0][0]
    point = inputs[1]
    modulationFrequency = inputs[2][0]

    intensity = 0.5 * (1 - math.cos(2 * math.pi * time * modulationFrequency))
    return (point[0], point[1], point[2], intensity)

defineOutputs(intensityModulationBlock, "out")

defineBlockOutputBehaviour(intensityModulationBlock.out, modulateIntensity)
setMetaData(intensityModulationBlock.out, "Sensation-Producing", False)
