# Tracked line between middle finger and palm
from pysensationcore import *

import sensation_helpers as sh

lineABBlock = createInstance("LinePath", "line")

trackedLine = sh.createSensationFromPath("TrackedLine",
                        {
                            ("middleFinger_distal_position", lineABBlock.endpointA) : (0, 0, 0),
                            ("palm_position", lineABBlock.endpointB) : (0, 0, 0),
                        },
                        output = lineABBlock.out,
                        drawFrequency = 70,
                        definedInVirtualSpace = True
                        )

setMetaData(trackedLine.middleFinger_distal_position, "Input-Visibility", False)
setMetaData(trackedLine.palm_position, "Input-Visibility", False)