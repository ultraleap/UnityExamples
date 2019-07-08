# Random Blocks
# These Operation Blocks produce random numbers
from pysensationcore import *
import random

# === RandomInteger ===
# A Block which Returns a random integer value in the range of min to max
randomIntBlock = defineBlock("RandomIntGenerator")
defineInputs(randomIntBlock, "t", "min", "max", "period", "seed")
defineOutputs(randomIntBlock, "out")
setMetaData(randomIntBlock.out, "Sensation-Producing", False)

defineBlockInputDefaultValue(randomIntBlock.min, (0, 0, 0))
defineBlockInputDefaultValue(randomIntBlock.max, (3, 0, 0))
defineBlockInputDefaultValue(randomIntBlock.period, (1.0, 0, 0))
defineBlockInputDefaultValue(randomIntBlock.seed, (12, 0, 0))

sequence_length = 200
random_int_sequence = {}

def randomIntFunction(inputs):
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0
    
    seed = inputs[4][0]
    f = 1.0/period    
    random.seed(seed)
    if seed not in random_int_sequence:
        random_int_sequence[seed] = [random.randint(minValue, maxValue) for i in range(0, sequence_length)]

    expression = random_int_sequence[seed][int((t * f) % sequence_length)]
    return expression, 0, 0

defineBlockOutputBehaviour(randomIntBlock.out, randomIntFunction)