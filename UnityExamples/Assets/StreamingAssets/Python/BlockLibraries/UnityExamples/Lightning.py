# # === Lightning  ===
# A PolylinePath based haptic which scans a line tracing along the finger positions
# For each finger 'beam' we have a Polyline Path drawn along these points
from pysensationcore import *
import sensation_helpers as sh
import RandomInt
import Mux

prefix = "point"
pointCount = 6
finger0Path = createInstance("PolylinePath", "finger0")
finger0Points = sh.createList(pointCount)
finger1Path = createInstance("PolylinePath", "finger1")
finger1Points = sh.createList(pointCount)
finger2Path = createInstance("PolylinePath", "finger2")
finger2Points = sh.createList(pointCount)
finger3Path = createInstance("PolylinePath", "finger3")
finger3Points = sh.createList(pointCount)
finger4Path = createInstance("PolylinePath", "finger4")
finger4Points = sh.createList(pointCount)

# A Multiplexer of size 5, to choose between each path
muxJointPath  = createInstance("Mux5", "muxJoints")

# Find a RandomInteger Generator, which will return a random int between 0-4
randomBlockInstance = createInstance("RandomIntGenerator", "randomInstance")
connect(Constant((0,0,0)), randomBlockInstance.min)
connect(Constant((4,0,0)), randomBlockInstance.max)

# Connect point list to each Finger Path
connect(finger0Points["output"], finger0Path.points)
connect(finger1Points["output"], finger1Path.points)
connect(finger2Points["output"], finger2Path.points)
connect(finger3Points["output"], finger3Path.points)
connect(finger4Points["output"], finger4Path.points)

# Connect finger paths to the multiplexer
connect(finger0Path.out, muxJointPath.input0 )
connect(finger1Path.out, muxJointPath.input1 )
connect(finger2Path.out, muxJointPath.input2 )
connect(finger3Path.out, muxJointPath.input3 )
connect(finger4Path.out, muxJointPath.input4 )

# Connect the output of the RandomInteger to the Mux5 selector
connect(randomBlockInstance.out, muxJointPath.selector)

lightningBlock = sh.createSensationFromPath("Lightning",
                           {
                               ("t", randomBlockInstance.t) : (0, 0, 0),
                               ("scanPeriod", randomBlockInstance.period) : (0.15, 0.0, 0.0),
                               ("wrist_position", finger0Points["inputs"][0]) : (0.0, 0, 0),
                               ("wrist_position", finger1Points["inputs"][0]) : (0.0, 0, 0),
                               ("wrist_position", finger2Points["inputs"][0]) : (0.0, 0, 0),
                               ("wrist_position", finger3Points["inputs"][0]) : (0.0, 0, 0),
                               ("wrist_position", finger4Points["inputs"][0]) : (0.0, 0, 0),
                               ("indexFinger_metacarpal_position", finger0Points["inputs"][1]) : (0, 0, 0),
                               ("middleFinger_metacarpal_position", finger1Points["inputs"][1]) : (0, 0, 0),
                               ("ringFinger_metacarpal_position", finger2Points["inputs"][1]) : (0, 0, 0),
                               ("pinkyFinger_metacarpal_position", finger3Points["inputs"][1]) : (0, 0, 0),
                               ("thumb_metacarpal_position", finger4Points["inputs"][1]) : (0, 0, 0),                               
                               ("palm_position", finger0Points["inputs"][2]) : (0.0, 0, 0),
                               ("palm_position", finger1Points["inputs"][2]) : (0.0, 0, 0),
                               ("palm_position", finger2Points["inputs"][2]) : (0.0, 0, 0),
                               ("palm_position", finger3Points["inputs"][2]) : (0.0, 0, 0),
                               ("palm_position", finger4Points["inputs"][2]) : (0.0, 0, 0),
                               ("indexFinger_proximal_position", finger0Points["inputs"][3]) : (0, 0, 0),
                               ("middleFinger_proximal_position", finger1Points["inputs"][3]) : (0, 0, 0),
                               ("ringFinger_proximal_position", finger2Points["inputs"][3]) : (0, 0, 0),
                               ("pinkyFinger_proximal_position", finger3Points["inputs"][3]) : (0, 0, 0),
                               ("thumb_proximal_position", finger4Points["inputs"][3]) : (0, 0, 0),
                               ("indexFinger_intermediate_position", finger0Points["inputs"][4]) : (0, 0, 0),
                               ("middleFinger_intermediate_position", finger1Points["inputs"][4]) : (0, 0, 0),
                               ("ringFinger_intermediate_position", finger2Points["inputs"][4]) : (0, 0, 0),
                               ("pinkyFinger_intermediate_position", finger3Points["inputs"][4]) : (0, 0, 0),
                               ("thumb_intermediate_position", finger4Points["inputs"][4]) : (0, 0, 0),
                               ("indexFinger_distal_position", finger0Points["inputs"][5]) : (0, 0, 0),
                               ("middleFinger_distal_position", finger1Points["inputs"][5]) : (0, 0, 0),
                               ("ringFinger_distal_position", finger2Points["inputs"][5]) : (0, 0, 0),
                               ("pinkyFinger_distal_position", finger3Points["inputs"][5]) : (0, 0, 0),                                                                
                               ("thumb_distal_position", finger4Points["inputs"][5]) : (0, 0, 0),
                           },
                           output = muxJointPath.out,
                           definedInVirtualSpace = True,
                           drawFrequency = 85
                           )

setMetaData(lightningBlock.wrist_position, "Input-Visibility", False)
setMetaData(lightningBlock.indexFinger_metacarpal_position, "Input-Visibility", False)
setMetaData(lightningBlock.middleFinger_metacarpal_position, "Input-Visibility", False)
setMetaData(lightningBlock.ringFinger_metacarpal_position, "Input-Visibility", False)
setMetaData(lightningBlock.pinkyFinger_metacarpal_position, "Input-Visibility", False)
setMetaData(lightningBlock.thumb_metacarpal_position, "Input-Visibility", False)
setMetaData(lightningBlock.palm_position, "Input-Visibility", False)
setMetaData(lightningBlock.indexFinger_proximal_position, "Input-Visibility", False)
setMetaData(lightningBlock.middleFinger_proximal_position, "Input-Visibility", False)
setMetaData(lightningBlock.ringFinger_proximal_position, "Input-Visibility", False)
setMetaData(lightningBlock.pinkyFinger_proximal_position, "Input-Visibility", False)
setMetaData(lightningBlock.thumb_proximal_position, "Input-Visibility", False)
setMetaData(lightningBlock.indexFinger_intermediate_position, "Input-Visibility", False)
setMetaData(lightningBlock.middleFinger_intermediate_position, "Input-Visibility", False)
setMetaData(lightningBlock.ringFinger_intermediate_position, "Input-Visibility", False)
setMetaData(lightningBlock.pinkyFinger_intermediate_position, "Input-Visibility", False)
setMetaData(lightningBlock.thumb_intermediate_position, "Input-Visibility", False)
setMetaData(lightningBlock.indexFinger_distal_position, "Input-Visibility", False)
setMetaData(lightningBlock.middleFinger_distal_position, "Input-Visibility", False)
setMetaData(lightningBlock.ringFinger_distal_position, "Input-Visibility", False)
setMetaData(lightningBlock.pinkyFinger_distal_position, "Input-Visibility", False)
setMetaData(lightningBlock.thumb_distal_position, "Input-Visibility", False)
setMetaData(lightningBlock.scanPeriod, "Type", "Scalar")