from pysensationcore import *
import math

# === SetLogIntensity Block ===
# A Block which can be used to modify the intensity (strength) of a Sensation using a logarithmic mapping.
# This gives a more perceptually accurate intensity control compared with linear intensity.
logIntensityBlock = defineBlock("SetLogIntensity")
defineInputs(logIntensityBlock, "intensity", "point")
defineBlockInputDefaultValue(logIntensityBlock.intensity, (1, 0, 0))

def setLogIntensity(inputs):
    intensity = inputs[0][0]

    if intensity > 1.0:
        intensity = 1.0
    elif intensity < 0.01:
        intensity = 0
    else:
        intensity = 0.5 * math.log10(intensity) + 1

    point = inputs[1]
    return point[0], point[1], point[2], intensity

defineOutputs(logIntensityBlock, "out")
defineBlockOutputBehaviour(logIntensityBlock.out, setLogIntensity)
setMetaData(logIntensityBlock.out, "Sensation-Producing", False)
