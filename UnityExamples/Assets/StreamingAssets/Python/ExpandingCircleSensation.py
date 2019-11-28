from pysensationcore import *

import sensation_helpers as sh

# Inner blocks
lerp = createInstance("Lerp", "lerp")
circlePath = createInstance("CirclePath", "CirclePathInstance")
comparator = createInstance("Comparator", "ComparatorInstance")

# Inner block connections
connect(lerp.out, circlePath.radius)
connect(Constant((0, 0, 0)), lerp.y0)
connect(Constant((0, 0, 0)), comparator.returnValueIfAGreaterThanB)
connect(Constant((1, 0, 0)), comparator.returnValueIfAEqualsB)
connect(Constant((1, 0, 0)), comparator.returnValueIfALessThanB)

expandingCircle = sh.createSensationFromPath("ExpandingCircleSensation",
                                             {
                                                 ("t", lerp.x) : (0, 0, 0),
                                                 ("duration", lerp.y1) : (1, 0, 0),
                                                 ("startRadius", lerp.x0) : (0.01, 0, 0),
                                                 ("endRadius", lerp.x1) : (0.05, 0, 0),
                                                 ("t", comparator.a) : (0, 0, 0),
                                                 ("duration", comparator.b) : (1, 0, 0),
                                             },
                                             output = circlePath.out,
                                             drawFrequency = 70,
                                             intensity = comparator.out,
                                             renderMode=sh.RenderMode.Loop
                                             )

setMetaData(expandingCircle, "Allow-Transform", True)
setMetaData(expandingCircle.duration, "Type", "Scalar")
setMetaData(expandingCircle.startRadius, "Type", "Scalar")
setMetaData(expandingCircle.endRadius, "Type", "Scalar")
setMetaData(expandingCircle.duration, "Type", "Scalar")
