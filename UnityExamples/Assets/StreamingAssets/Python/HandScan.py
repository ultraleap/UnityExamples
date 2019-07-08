from pysensationcore import *

import sensation_helpers as sh
import Scan

# Inner blocks
scan = createInstance("Scan", "scan")
comparator = createInstance("Comparator", "ComparatorInstance")

# Inner block connections
connect(Constant((0, 0, 0)), comparator.returnValueIfAGreaterThanB)
connect(Constant((1, 0, 0)), comparator.returnValueIfAEqualsB)
connect(Constant((1, 0, 0)), comparator.returnValueIfALessThanB)

handScan = sh.createSensationFromPath("HandScan",
                                      {
                                          ("t", scan.t) : (0, 0, 0),
                                          ("duration", scan.duration) : (2, 0, 0),
                                          ("barLength", scan.barLength) : (0.1, 0, 0),
                                          ("virtualObjectXInVirtualSpace", scan.barDirection) : (1, 0, 0),
                                          ("wrist_position", scan.animationPathStart) : (0, 0.2, -0.06),
                                          ("middleFinger_distal_position", scan.animationPathEnd) : (0, 0.2, 0.06),
                                          ("t", comparator.a) : (0, 0, 0),
                                          ("duration", comparator.b) : (2, 0, 0)
                                      },
                                      output = scan.out,
                                      intensity = comparator.out,
                                      definedInVirtualSpace = True
                                      )

setMetaData(handScan.virtualObjectXInVirtualSpace, "Input-Visibility", False)
setMetaData(handScan.wrist_position, "Input-Visibility", False)
setMetaData(handScan.middleFinger_distal_position, "Input-Visibility", False)

setMetaData(handScan.duration, "Type", "Scalar")
setMetaData(handScan.barLength, "Type", "Scalar")
