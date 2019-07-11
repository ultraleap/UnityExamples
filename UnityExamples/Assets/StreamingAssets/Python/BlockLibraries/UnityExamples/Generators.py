# Waveform Generator Blocks
# These Blocks produce waveforms which can be used to drive behaviour of other Blocks
# e.g. a Circle's radius can be connected to a SineWave Block output to produce a pulsing circle.
from pysensationcore import *
import math
import random

# === SawtoothWave Block ===
# A Sawtooth Wave function, which oscillates between a min-max value with a period (specified in seconds)
sawToothBlock = defineBlock("SawtoothWave")
defineInputs(sawToothBlock, "t", "min","max", "period")
defineOutputs(sawToothBlock, "out")
defineBlockInputDefaultValue(sawToothBlock.min, (0.01, 0.0, 0.0))
defineBlockInputDefaultValue(sawToothBlock.max, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(sawToothBlock.period, (1, 0.0, 0.0))

def sawtoothWaveFunction(inputs):
    # A Sawtooth function specified between a min and max value
    # ((1/waveLength)*(((frame-1)+offset) % waveLength) * (maxVal-minVal) ) + minVal
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0

    f = 1.0/period
    expression = (f * (t % period) * (maxValue-minValue) ) + minValue
    return expression, 0, 0

defineBlockOutputBehaviour(sawToothBlock.out, sawtoothWaveFunction)
setMetaData(sawToothBlock.out, "Sensation-Producing", False)

# === SineWave Block ===
# A Sine Wave Generator Block, which oscillates between a min-max value with a period (specified in seconds)
sineWaveBlock = defineBlock("SineWave")
defineInputs(sineWaveBlock, "t", "min","max","period")
defineBlockInputDefaultValue(sineWaveBlock.min, (0.01, 0.0, 0.0))
defineBlockInputDefaultValue(sineWaveBlock.max, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(sineWaveBlock.period, (1, 0.0, 0.0))
defineOutputs(sineWaveBlock, "out")

def sineWaveFunction(inputs):
    # A sinusoid function specified between a min and max value
    #((max - min) * sin(t) + max + min) / 2
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0

    f = 1.0/period
    angle = 2*math.pi*f*t
    expression = ((maxValue-minValue) * math.sin(angle) + (maxValue+minValue)) / 2
    return expression, 0, 0

defineBlockOutputBehaviour(sineWaveBlock.out, sineWaveFunction)
setMetaData(sineWaveBlock.out, "Sensation-Producing", False)


# === SquareWave Block ===
# A Square Wave function, which oscillates between a min-max value with a period (specified in seconds)
squareWaveBlock = defineBlock("SquareWave")
defineInputs(squareWaveBlock, "t", "min", "max", "period")
defineBlockInputDefaultValue(squareWaveBlock.min, (0.01, 0.0, 0.0))
defineBlockInputDefaultValue(squareWaveBlock.max, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(squareWaveBlock.period, (1, 0.0, 0.0))
defineOutputs(squareWaveBlock, "out")

def squareWaveFunction(inputs):
    # A Square wave function specified between a min and max value
    # int(sin(2*pi*(frame+offset)/waveLength)+1) * (maxVal-minVal) + minVal
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0

    f = 1.0/period
    expression = int(math.sin(2*math.pi*f*t)+1) * (maxValue-minValue) + minValue
    return expression, 0, 0

defineBlockOutputBehaviour(squareWaveBlock.out, squareWaveFunction)
setMetaData(squareWaveBlock.out, "Sensation-Producing", False)


# === RandomStepWave Block ===
# A Random Step Wave Generator Block, which oscillates between a min-max value with a period (specified in seconds)
randomWaveBlock = defineBlock("RandomWave")
defineInputs(randomWaveBlock, "t", "min", "max", "period", "seed")
defineBlockInputDefaultValue(randomWaveBlock.min, (-0.05, 0.0, 0.0))
defineBlockInputDefaultValue(randomWaveBlock.max, (0.05, 0.0, 0.0))
defineBlockInputDefaultValue(randomWaveBlock.period, (1, 0.0, 0.0))
defineBlockInputDefaultValue(randomWaveBlock.seed, (0, 0, 0))
defineOutputs(randomWaveBlock, "out")

length = 200
random_sequence = {}

def randomWaveFunction(inputs):
    # A random step wave function specified between a min and max value with a period (specified in seconds)
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    period = inputs[3][0]

    if period == 0:
        return 0,0,0
    
    seed = inputs[4][0]
    f = 1.0/period    

    global random_sequence
    if seed not in random_sequence.keys() or (random_sequence[seed]['min'] != minValue) or (random_sequence[seed]['max'] != maxValue):
        random.seed(seed)
        random_sequence[seed] = {}
        random_sequence[seed]['sequence'] = [random.uniform(minValue, maxValue) for i in range(0, length)]
        random_sequence[seed]['min'] = minValue
        random_sequence[seed]['max'] = maxValue

    expression = random_sequence[seed]['sequence'][int((t * f) % length)]
    
    return expression, 0, 0

defineBlockOutputBehaviour(randomWaveBlock.out, randomWaveFunction)
setMetaData(randomWaveBlock.out, "Sensation-Producing", False)


# === Pulse Width Block ===
# A PWM-type function which gives a square wave pulse with min/max value, with Pulse Length and Gap duration
# Reference: Modified 'Blip' function from: http://www.cameroncarson.com/nuke-wave-expressions/
pulseBlock = defineBlock("PulseWave")
defineInputs(pulseBlock, "t", "min", "max", "pulseLength", "repeatGap")
defineBlockInputDefaultValue(pulseBlock.min, (0.0, 0.0, 0.0))
defineBlockInputDefaultValue(pulseBlock.max, (1.0, 0.0, 0.0))
defineBlockInputDefaultValue(pulseBlock.pulseLength, (0.2, 0.0, 0.0))
defineBlockInputDefaultValue(pulseBlock.repeatGap, (0.1, 0, 0))
defineOutputs(pulseBlock, "out")

def pulseFunction(inputs):
    t = inputs[0][0]
    minValue = inputs[1][0]
    maxValue = inputs[2][0]
    pulsePeriod = inputs[3][0]
    gapPeriod = inputs[4][0]

    if pulsePeriod <=0:
        return minValue,0,0
    if gapPeriod <=0:
        return maxValue,0,0

    exp = ((t+(gapPeriod)) % (gapPeriod+pulsePeriod)/(gapPeriod))*(gapPeriod/pulsePeriod)-(gapPeriod/pulsePeriod)
    if exp >= 0:
        return maxValue,0,0
    else:
        return minValue,0,0

defineBlockOutputBehaviour(pulseBlock.out, pulseFunction)
setMetaData(pulseBlock.out, "Sensation-Producing", False)

# === PulseWaveRepeat ===
# Similar to PulseWave, but number of pulses is limited to repeat N times (as specified by repetitions input)
# Reference: Modified 'Blip' function from: http://www.cameroncarson.com/nuke-wave-expressions/
pulseWaveRepeatBlock = defineBlock("PulseWaveRepeat")
defineInputs(pulseWaveRepeatBlock, "t", "repetitions", "pulseLength", "repeatGap", "min", "max" )
defineBlockInputDefaultValue(pulseWaveRepeatBlock.min, (0.0, 0.0, 0.0))
defineBlockInputDefaultValue(pulseWaveRepeatBlock.max, (1.0, 0.0, 0.0))
defineBlockInputDefaultValue(pulseWaveRepeatBlock.pulseLength, (0.2, 0.0, 0.0))
defineBlockInputDefaultValue(pulseWaveRepeatBlock.repeatGap, (0.1, 0, 0))
defineBlockInputDefaultValue(pulseWaveRepeatBlock.repetitions, (3, 0, 0))
defineOutputs(pulseWaveRepeatBlock, "out")

def pulseRepeatFunction(inputs):
    t = inputs[0][0]
    repetitions = inputs[1][0]
    pulsePeriod = inputs[2][0]
    gapPeriod = inputs[3][0]
    
    # The total duration of the pulse, given its pulse length and gaps
    duration = (repetitions*pulsePeriod) + ((repetitions-1)*gapPeriod)
    
    if t > duration or duration <= 0:
        return (0,0,0)
    
    minValue = inputs[4][0]
    maxValue = inputs[5][0]

    if pulsePeriod <=0:
        return minValue,0,0
    if gapPeriod <=0:
        return maxValue,0,0

    exp = ((t+(gapPeriod)) % (gapPeriod+pulsePeriod)/(gapPeriod))*(gapPeriod/pulsePeriod)-(gapPeriod/pulsePeriod)
    if exp >= 0:
        return maxValue,0,0
    else:
        return minValue,0,0

defineBlockOutputBehaviour(pulseWaveRepeatBlock.out, pulseRepeatFunction)
setMetaData(pulseWaveRepeatBlock.out, "Sensation-Producing", False)

# === BinaryWave Block ===
# A BinaryWave oscillates between 0-1 with a period (specified in seconds)
binaryBlock = defineBlock("BinaryWave")
defineInputs(binaryBlock, "t", "period")
defineBlockInputDefaultValue(binaryBlock.period, (0.05, 0.0, 0.0))
defineOutputs(binaryBlock, "out")

def binaryFunction(inputs):
    t = inputs[0][0]
    period = inputs[1][0]

    if period == 0:
        return 0,0,0

    f = 1.0/period
    expression = int(math.sin(2*math.pi*f*t)+1)
    return (expression, 0, 0)

defineBlockOutputBehaviour(binaryBlock.out, binaryFunction)
setMetaData(binaryBlock.out, "Sensation-Producing", False)


# === IntegerClock Block ===
# IntegerClock which increments an integer min to max values, at step rate given by period
# The IntegerClock will loop back to the starting point after reaching the max number
integerClockBlock = defineBlock("IntegerClock")
defineInputs(integerClockBlock, "t", "min", "max", "period")
defineBlockInputDefaultValue(integerClockBlock.min, (0,0,0))
defineBlockInputDefaultValue(integerClockBlock.max, (3,0,0))
defineBlockInputDefaultValue(integerClockBlock.period, (1,0,0))

def intClock(inputs):
    t = inputs[0][0]
    
    start = int(inputs[1][0])
    end = int(inputs[2][0])
    period = inputs[3][0]

    if period <= 0:
        return (0,0,0)

    sequence = list(range(start, end+1))

    # Get the index of the closest step
    ix = int(t/period)
    index = ix % len(sequence)
    
    return (sequence[index], 0 , 0)

defineOutputs(integerClockBlock, "out")
setMetaData(integerClockBlock.out, "Sensation-Producing", False)
defineBlockOutputBehaviour(integerClockBlock.out, intClock)