from pysensationcore import *
import sensation_helpers as sh
import IntensityWave

path = createInstance("CirclePath","innerPath")
cosineWaveInstance = createInstance("IntensityWave","IntensityWaveInstance")

pointAM = sh.createSensationFromPath("PointAM",
                                  {
                                      ("size", path.radius) : (0.005, 0, 0),
                                      ("amFrequency", cosineWaveInstance.modulationFrequency) : (143, 0, 0),
                                      ("t", cosineWaveInstance.t) : (0, 0, 0)
                                  },
                                  output = path.out,
                                  intensity = cosineWaveInstance.out,
                                  drawFrequency = 70,
                                  renderMode=sh.RenderMode.Loop
                                  )

setMetaData(pointAM, "Allow-Transform", True)
setMetaData(pointAM.size, "Type", "Scalar")
setMetaData(pointAM.size, "Input-Visibility", False)
setMetaData(pointAM.size, "Type", "Scalar")
setMetaData(pointAM.size, "Input-Visibility", False)
setMetaData(pointAM.amFrequency, "Type", "Scalar")
setMetaData(pointAM.amFrequency, "Input-Visibility", False)