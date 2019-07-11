from pysensationcore import *
from TwoHandedSensation import forwardAnyInputsCreatedViaSensationHelpers

pointBlock = createInstance("LissajousSensation", "lissajous")

fingerTrackedBlock = defineBlock("Pointing")
defineInputs(fingerTrackedBlock, "indexFinger_distal_position", "width", "length")
defineOutputs(fingerTrackedBlock, "out")

forwardAnyInputsCreatedViaSensationHelpers(fingerTrackedBlock, pointBlock)
connect(fingerTrackedBlock.indexFinger_distal_position, pointBlock.virtualObjectOriginInVirtualSpace)
connect(fingerTrackedBlock.width, pointBlock.sizeX)
connect(fingerTrackedBlock.length, pointBlock.sizeY)

setMetaData(fingerTrackedBlock.indexFinger_distal_position, "Input-Visibility", False)
defineBlockInputDefaultValue(fingerTrackedBlock.width, (0.015,0,0))
defineBlockInputDefaultValue(fingerTrackedBlock.length, (0.03,0,0))
defineBlockInputDefaultValue(fingerTrackedBlock.drawFrequency, (60,0,0))
setMetaData(fingerTrackedBlock.width, "Type", "Scalar")
setMetaData(fingerTrackedBlock.length, "Type", "Scalar")
connect(pointBlock.out, fingerTrackedBlock.out)
