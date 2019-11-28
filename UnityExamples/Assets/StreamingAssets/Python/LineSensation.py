from pysensationcore import *

import sensation_helpers as sh

pathInstance = createInstance("LinePath", "LinePathInstance")

line = sh.createSensationFromPath("LineSensation",
                                  {
                                      ("endpointA", pathInstance.endpointA) : (-0.04, 0.0, 0.0),
                                       ("endpointB", pathInstance.endpointB) : (0.04, 0.0, 0.0),
                                  },
                                  output = pathInstance.out,
                                  drawFrequency = 125,
                                  definedInVirtualSpace = True
                                  )

setMetaData(line.endpointA, "Type", "Point")
setMetaData(line.endpointB, "Type", "Point")
