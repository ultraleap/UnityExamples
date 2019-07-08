from pysensationcore import *
import sensation_helpers as sh

pathInstance = createInstance("CirclePath", "CirclePathInstance")

palmTrackedCircle = sh.createSensationFromPath("PalmTrackedCircle",
                                               {
                                                   ("radius", pathInstance.radius) : (0.03, 0, 0),
                                               },
                                               output = pathInstance.out,
                                               drawFrequency = 60,
                                               renderMode = sh.RenderMode.Loop
                                               )

setMetaData(palmTrackedCircle.radius, "Type", "Scalar")
