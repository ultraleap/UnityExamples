from pysensationcore import *
import sensation_helpers as sh
from TwoHandedSensation import *
import Mux
import RandomInt

# # === Lightning  ===
# A Line-Path based haptic which scans between the Palm position and finger tips
# Define our Line Path that will be scanned between fingertips
linePathBlock = createInstance("LinePath", "line")

# Find a RandomInteger Generator, which will return a random int between 0-4
randomBlockInstance = createInstance("RandomIntGenerator", "randomInstance")
connect(Constant((0,0,0)), randomBlockInstance.min)
connect(Constant((4,0,0)), randomBlockInstance.max)

# Use a Mux block to change the endpoint of the line
mux5Block = createInstance("Mux5", "mux5Instance")

# Connect the output of the RandomInteger to the Mux5 selector
connect(randomBlockInstance.out, mux5Block.selector)

connect(mux5Block.out, linePathBlock.endpointB)

lightningBlock = sh.createSensationFromPath("LightningBase",
                                            {
                                                ("t", randomBlockInstance.t) : (0, 0, 0),
                                                ("scanPeriod", randomBlockInstance.period) : (0.1, 0, 0),

                                                ("indexFinger_distal_position", mux5Block.input0) : (0, 0, 0),
                                                ("middleFinger_distal_position", mux5Block.input1) : (0, 0, 0),
                                                ("ringFinger_distal_position", mux5Block.input2) : (0, 0, 0),
                                                ("pinkyFinger_distal_position", mux5Block.input3) : (0, 0, 0),
                                                ("thumb_distal_position", mux5Block.input4) : (0, 0, 0),
                                                ("palm_position", linePathBlock.endpointA) : (0, 0, 0)
                                            },
                                            output = linePathBlock.out,
                                            definedInVirtualSpace = True
                                            )

setMetaData(lightningBlock.scanPeriod, "Type", "Scalar")
setMetaData(lightningBlock.out, "Sensation-Producing", False)

# Two handed Lightning
oneHandedLightningInst = createInstance("LightningBase", "Inst")
twoHandedLightning = makeSensationTwoHanded(oneHandedLightningInst, "Lightning2")

unconnectedInputs = ["scanPeriod"]

defineInputs(twoHandedLightning, *unconnectedInputs)
for input in unconnectedInputs:
    connect(getattr(twoHandedLightning, input), getattr(oneHandedLightningInst, input))

setMetaData(twoHandedLightning.scanPeriod, "Type", "Scalar")
defineBlockInputDefaultValue(twoHandedLightning.scanPeriod, (0.1, 0, 0))

defineOutputs(twoHandedLightning, "out")
connect(getattr(oneHandedLightningInst, "out"), getattr(twoHandedLightning, "out"))