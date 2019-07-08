from pysensationcore import *
import sensation_helpers as sh

pathInstance = createInstance("CirclePath", "CirclePathInstance")

palmPresence = sh.createSensationFromPath("PalmPresence",
                                          {
                                                ("radius", pathInstance.radius) : (0.02, 0, 0),
                                          },
                                          output = pathInstance.out,
                                          drawFrequency = 70,
                                          renderMode = sh.RenderMode.Loop
                                          )

defineBlockInputDefaultValue(palmPresence.intensity, (0.7, 0, 0))
setMetaData(palmPresence.radius, "Type", "Scalar")
