# Modified version of HandScan - allowing user to specify the start end points via Unity Transforms
from pysensationcore import *
import sensation_helpers as sh
import Scan
import NormalizedDirectionFromTwoPoints
from VectorOperations import vectorSubtract, vectorNormalize

# Create a Block which returns the direction vector of two points.
directionBlockInstance = createInstance("NormalizedDirectionFromTwoPoints", "normDirection")

# Inner blocks
scan = createInstance("Scan", "scan")
comparator = createInstance("Comparator", "ComparatorInstance")

# Connect the output of the direction Block 
connect(directionBlockInstance.direction, scan.barDirection)

# Inner block connections
connect(Constant((0, 0, 0)), comparator.returnValueIfAGreaterThanB)
connect(Constant((1, 0, 0)), comparator.returnValueIfAEqualsB)
connect(Constant((1, 0, 0)), comparator.returnValueIfALessThanB)

handScan = sh.createSensationFromPath("LineScan",
                                      {
                                          ("t", scan.t) : (0, 0, 0),
                                          ("duration", scan.duration) : (2, 0, 0),
                                          ("barLength", scan.barLength) : (0.1, 0, 0),
                                          ("startPoint", directionBlockInstance.pointA) : (0, 0.2, -0.06),
                                          ("endPoint", directionBlockInstance.pointB) : (0, 0.2, 0.06),
                                          ("startPoint", scan.animationPathStart) : (0, 0.2, -0.06),
                                          ("endPoint", scan.animationPathEnd) : (0, 0.2, 0.06),
                                          ("t", comparator.a) : (0, 0, 0),
                                          ("duration", comparator.b) : (2, 0, 0)
                                      },
                                      output = scan.out,
                                      intensity = comparator.out,
                                      definedInVirtualSpace = True
                                      )

setMetaData(handScan.startPoint, "Type", "Point")
setMetaData(handScan.endPoint, "Type", "Point")
setMetaData(handScan.duration, "Type", "Scalar")
setMetaData(handScan.barLength, "Type", "Scalar")
setMetaData(handScan, "IsFinite", True)