# A Sensation which animates (scans) a Lissajous along the fingers
from pysensationcore import *
import sensation_helpers as sh
import Mux

scanBlock = defineBlock("PointScan")
defineInputs(scanBlock,
             "t",
             "scanDuration",
             "barDirection",
             "animationPathStart",
             "animationPathEnd",
             "objectSize")

defineBlockInputDefaultValue(scanBlock.scanDuration, (2.5, 0, 0))
defineBlockInputDefaultValue(scanBlock.barDirection, (1, 0, 0))
defineBlockInputDefaultValue(scanBlock.animationPathStart, (0, -0.06, 0.0))
defineBlockInputDefaultValue(scanBlock.animationPathEnd, (0, 0.06, 0.0))

animPath = createInstance("LinePath", "animPath")
connect(scanBlock.animationPathStart, animPath.endpointA)
connect(scanBlock.animationPathEnd, animPath.endpointB)

pointObject = createInstance("LissajousPath", "lissajous")
connect(scanBlock.objectSize, pointObject.sizeX)
connect(scanBlock.objectSize, pointObject.sizeY)
connect(Constant((3,0,0)), pointObject.paramA)
connect(Constant((2,0,0)), pointObject.paramB)

anim = createInstance("TranslateAlongPath", "anim")
connect(scanBlock.t, anim.t)
connect(scanBlock.scanDuration, anim.duration)
connect(Constant((0,0,0)), anim.direction)
connect(animPath.out, anim.animationPath)
connect(pointObject.out, anim.objectPath)

defineOutputs(scanBlock, "out")
setMetaData(scanBlock.out, "Sensation-Producing", False)
connect(anim.out, scanBlock.out)

# Inner blocks
scan = createInstance("PointScan", "scan")

# Use a Mux block to change the endpoint of the scan
mux4Block = createInstance("Mux4", "mux4")

# IntegerClock which increments an integer min to max values, at step rate given by period
# The IntegerClock will loop back to the starting point after reaching the max number
sequencerBlock = defineBlock("IntegerClock")
defineInputs(sequencerBlock, "t", "min", "max", "period")
defineBlockInputDefaultValue(sequencerBlock.min, (0,0,0))
defineBlockInputDefaultValue(sequencerBlock.max, (3,0,0))
defineBlockInputDefaultValue(sequencerBlock.period, (1,0,0))

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

defineOutputs(sequencerBlock, "out")
setMetaData(sequencerBlock.out, "Sensation-Producing", False)
defineBlockOutputBehaviour(sequencerBlock.out, intClock)

sequencer = createInstance("IntegerClock", "sequencer")
connect(Constant((0,0,0)), sequencer.min)
connect(Constant((3,0,0)), sequencer.max)

# Connect the output of the Sequencer to the Mux selector
connect(sequencer.out, mux4Block.selector)
connect(mux4Block.out, scan.animationPathEnd)

fingerScan = sh.createSensationFromPath("FingerScan",
                                      {
                                          ("t", scan.t) : (0, 0, 0),
                                          ("t", sequencer.t) : (0, 0, 0),
                                          ("start", sequencer.min) : (0, 0, 0),
                                          ("end", sequencer.max) : (3, 0, 0),
                                          ("jumpDuration", sequencer.period) : (0.2, 0, 0),
                                          ("scanDuration", scan.scanDuration) : (0.125, 0, 0),
                                          ("size", scan.objectSize) : (0.01, 0, 0),
                                          ("virtualObjectXInVirtualSpace", scan.barDirection) : (1, 0, 0),
                                          ("wrist_position", scan.animationPathStart) : (0, 0.2, -0.06),
                                          ("indexFinger_distal_position", mux4Block.input0) : (-0.04, 0.2, 0.07),
                                          ("middleFinger_distal_position", mux4Block.input1) : (-0.02, 0.2, 0.08),
                                          ("ringFinger_distal_position", mux4Block.input2) : (0.02, 0.2, 0.065),
                                          ("pinkyFinger_distal_position", mux4Block.input3) : (0.04, 0.2, 0.05),
                                      },
                                      output = scan.out,
                                      drawFrequency = 40,
                                      definedInVirtualSpace = True
                                      )

setMetaData(fingerScan.virtualObjectXInVirtualSpace, "Input-Visibility", False)
setMetaData(fingerScan.wrist_position, "Input-Visibility", False)
setMetaData(fingerScan.indexFinger_distal_position, "Input-Visibility", False)
setMetaData(fingerScan.middleFinger_distal_position, "Input-Visibility", False)
setMetaData(fingerScan.ringFinger_distal_position, "Input-Visibility", False)
setMetaData(fingerScan.pinkyFinger_distal_position, "Input-Visibility", False)
setMetaData(fingerScan.start, "Input-Visibility", False)
setMetaData(fingerScan.end, "Input-Visibility", False)
setMetaData(fingerScan.jumpDuration, "Type", "Scalar")
setMetaData(fingerScan.scanDuration, "Type", "Scalar")
setMetaData(fingerScan.size, "Type", "Scalar")
