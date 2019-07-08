from pysensationcore import *

import sensation_helpers as sh
import TriangleWave

pathInstance = createInstance("CirclePath", "CirclePathInstance")
triangleWaveBlockInstance = createInstance("TriangleWave", "triangleWave")
connect(triangleWaveBlockInstance.out, pathInstance.radius)

palmTrackedPulsingCircle = sh.createSensationFromPath("PalmTrackedPulsingCircle",
                                                      {
                                                          ("t", triangleWaveBlockInstance.t) : (0,0,0),
                                                          ("Start Radius (m)", triangleWaveBlockInstance.minValue) : (0.01, 0, 0),
                                                          ("End Radius (m)", triangleWaveBlockInstance.maxValue) : (0.05, 0, 0),
                                                          ("Pulse Period (s)", triangleWaveBlockInstance.period) : (5.0, 0, 0),
                                                      },
                                                      output = pathInstance.out,
                                                      drawFrequency = 70,
                                                      intensity = None,
                                                      renderMode = sh.RenderMode.Loop
                                                      )

setMetaData(palmTrackedPulsingCircle.__getattr__("Start Radius (m)"), "Type", "Scalar")
setMetaData(palmTrackedPulsingCircle.__getattr__("End Radius (m)"), "Type", "Scalar")
setMetaData(palmTrackedPulsingCircle.__getattr__("Pulse Period (s)"), "Type", "Scalar")
