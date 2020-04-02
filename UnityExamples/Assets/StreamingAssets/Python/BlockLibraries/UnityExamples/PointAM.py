# An approximation to an Amplitude Modulation point, which uses the path rendering pipeline.
# This makes it more convenient to transform a point, via Unity Transfrom in the inspector.
from pysensationcore import *
import sensation_helpers as sh
import IntensityWave

path = createInstance("CirclePath","innerPath")

# We use a Circle Path of zero radius
connect(Constant((0,0,0)), path.radius)
cosineWaveInstance = createInstance("IntensityWave","IntensityWaveInstance")

pointAM = sh.createSensationFromPath("PointAM",
                                  {
                                      ("amFrequency", cosineWaveInstance.modulationFrequency) : (143, 0, 0),
                                      ("t", cosineWaveInstance.t) : (0, 0, 0)
                                  },
                                  output = path.out,
                                  intensity = cosineWaveInstance.out,
                                  drawFrequency = 0,
                                  renderMode=sh.RenderMode.Loop
                                  )

setMetaData(pointAM, "Allow-Transform", True)
setMetaData(pointAM.amFrequency, "Type", "Scalar")
setMetaData(pointAM.drawFrequency, "Input-Visibility", False)