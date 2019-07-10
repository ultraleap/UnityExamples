import LissajousSensation
from TwoHandedSensation import *
import sensation_helpers as sh

pointBlock = createInstance("LissajousSensation", "lissajous")

fingerTrackedBlock = defineBlock("FingerTipPresence")
defineInputs(fingerTrackedBlock, "indexFinger_distal_position", "size")
defineOutputs(fingerTrackedBlock, "out")

forwardAnyInputsCreatedViaSensationHelpers(fingerTrackedBlock, pointBlock)
connect(fingerTrackedBlock.indexFinger_distal_position, pointBlock.virtualObjectOriginInVirtualSpace)
connect(fingerTrackedBlock.size, pointBlock.sizeX)
connect(fingerTrackedBlock.size, pointBlock.sizeY)

setMetaData(fingerTrackedBlock.indexFinger_distal_position, "Input-Visibility", False)
defineBlockInputDefaultValue(fingerTrackedBlock.size, (0.01,0,0))
defineBlockInputDefaultValue(fingerTrackedBlock.drawFrequency, (40,0,0))
setMetaData(fingerTrackedBlock.size, "Type", "Scalar")
connect(pointBlock.out, fingerTrackedBlock.out)