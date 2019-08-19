# Modified version of HandScan - allowing user to specify the start end points via Unity Transforms
from pysensationcore import *
import sensation_helpers as sh
import Scan
import NormalizedDirectionFromTwoPoints
from VectorOperations import vectorSubtract, vectorNormalize
import TimeOps

# Create a Block which returns the direction vector of two points.
directionBlockInstance = createInstance("NormalizedDirectionFromTwoPoints", "normDirection")

# Inner blocks
scan = createInstance("Scan", "scan")
reverse = createInstance("ReverseTime", "reverseInstance")
timeLoop = createInstance("LoopTime", "loopTimeInstance")
connect(reverse.time, timeLoop.t)
connect(timeLoop.time, scan.t)


# Connect the output of the direction Block 
connect(directionBlockInstance.direction, scan.barDirection)

loopScan = sh.createSensationFromPath("LoopScan",
                                      {
                                          ("t", reverse.t) : (0, 0, 0),
                                          ("duration", scan.duration) : (2, 0, 0),
                                          ("duration", timeLoop.duration) : (2, 0, 0),
                                          ("barLength", scan.barLength) : (0.1, 0, 0),
                                          ("startPoint", directionBlockInstance.pointA) : (0, 0.2, -0.06),
                                          ("endPoint", directionBlockInstance.pointB) : (0, 0.2, 0.06),
                                          ("startPoint", scan.animationPathStart) : (0, 0.2, -0.06),
                                          ("endPoint", scan.animationPathEnd) : (0, 0.2, 0.06),
                                          ("reverse", reverse.reversed) : (0, 0, 0),                                          
                                      },
                                      output = scan.out,
                                      definedInVirtualSpace = True
                                      )

setMetaData(loopScan.startPoint, "Type", "Point")
setMetaData(loopScan.endPoint, "Type", "Point")
setMetaData(loopScan.duration, "Type", "Scalar")
setMetaData(loopScan.barLength, "Type", "Scalar")

# There's currently no 'Type' to determine a Boolean, so need custom metadata
setMetaData(loopScan.reverse, "MetaType", "Boolean")
