from pysensationcore import *

# === Inverse Block ===
# A Block which returns the inverse of a scalar value, or 0 if the value is 0
inverseBlock = defineBlock("Inverse")
defineInputs(inverseBlock, "value")
defineBlockInputDefaultValue(inverseBlock.value, (1, 0, 0))

def inverse(inputs):
    value = inputs[0][0]

    if value == 0:
        return 0, 0, 0
    else:
        return 1/value, 0, 0

defineOutputs(inverseBlock, "out")
defineBlockOutputBehaviour(inverseBlock.out, inverse)
setMetaData(inverseBlock.out, "Sensation-Producing", False)
