# Ops Blocks
# These Operation Blocks can be used to modify behaviour of Blocks
# e.g. time can be reversed to change the direction of an expanding Circle
from pysensationcore import *

# === LoopTime ===
# A Block which loops the time input, such that the incoming 'world' time 
# is always looped between 0 -> Duration
# If Duration is less than or equal to zero, regular time is used.
# By default, Loop Time will loop time every 2 seconds.
loopTimeBlock = defineBlock("LoopTime")
defineInputs(loopTimeBlock, "t", "duration")
defineBlockInputDefaultValue(loopTimeBlock.t, (0, 0, 0))
defineBlockInputDefaultValue(loopTimeBlock.duration, (2, 0, 0))

defineOutputs(loopTimeBlock, "time")

def loopTime(inputs):
    t = inputs[0][0]
    duration = inputs[1][0]
    if duration <= 0:
    	return t
    else:
    	loopedTime = t % duration
    return (loopedTime,0,0)

defineBlockOutputBehaviour(loopTimeBlock.time, loopTime)
setMetaData(loopTimeBlock.time, "Sensation-Producing", False)


# === ReverseTime ===
# A Block which reverses the time input, such that the incoming 'world' time 
# is negated
reverseTimeBlock = defineBlock("ReverseTime")
defineInputs(reverseTimeBlock, "t", "reversed")
defineBlockInputDefaultValue(reverseTimeBlock.t, (0, 0, 0))
defineBlockInputDefaultValue(reverseTimeBlock.reversed, (1, 0, 0))
defineOutputs(reverseTimeBlock, "time")

def reverseTime(inputs):
    t = inputs[0][0]
    reversed = int(inputs[1][0])
    if (reversed >= 1):
    	return (-t,0,0)
    else:
    	return (t,0,0)

defineBlockOutputBehaviour(reverseTimeBlock.time, reverseTime)
setMetaData(reverseTimeBlock.time, "Sensation-Producing", False)

# === LoopTime ===
# A Block which bounces the time input, such that the incoming 'world' time 
# is always looped between 0 -> Duration -> 0 -> - Duration
# If Duration is less than or equal to zero, regular time is used.
# By default, Loop Time will loop time every 2 seconds.
bounceTimeBlock = defineBlock("BounceTime")
defineInputs(bounceTimeBlock, "t", "duration")
defineBlockInputDefaultValue(bounceTimeBlock.t, (0, 0, 0))
defineBlockInputDefaultValue(bounceTimeBlock.duration, (2, 0, 0))

defineOutputs(bounceTimeBlock, "time")

def bounceTime(inputs):
    t = inputs[0][0]
    duration = inputs[1][0]
    if duration <= 0:
    	return t
    else:
    	loopedTime = t % duration
    return (loopedTime,0,0)

defineBlockOutputBehaviour(bounceTimeBlock.time, bounceTime)
setMetaData(bounceTimeBlock.time, "Sensation-Producing", False)
