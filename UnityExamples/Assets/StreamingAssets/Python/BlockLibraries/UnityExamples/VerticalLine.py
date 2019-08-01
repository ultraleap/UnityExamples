# Tracked line between middle finger and palm
from pysensationcore import *

import sensation_helpers as sh

lineABBlock = createInstance("LinePath", "line")

trackedLine = sh.createSensationFromPath("Vertical Line",
                        {
                            ("middleFinger_distal_position", lineABBlock.endpointA) : (0, 0, 0),
                            ("wrist_position", lineABBlock.endpointB) : (0, 0, 0),
                        },
                        output = lineABBlock.out,
                        drawFrequency = 90,
                        definedInVirtualSpace = True
                        )

setMetaData(trackedLine.middleFinger_distal_position, "Input-Visibility", False)
setMetaData(trackedLine.wrist_position, "Input-Visibility", False)