from pysensationcore import *

import sensation_helpers as sh
import IntensityWave
import Inverse

innerPath = createInstance("CirclePath","innerPath")
outerPath = createInstance("CirclePath","outerPath")
anim = createInstance("TranslateAlongPath","anim")
cosineWaveInstance = createInstance("IntensityWave","IntensityWaveInstance")
inverseInstance = createInstance("Inverse","InverseInstance")

connect(outerPath.out,anim.animationPath)
connect(innerPath.out,anim.objectPath)
connect(inverseInstance.out, anim.duration)
connect(Constant((1, 0, 0)), anim.direction)

dial = sh.createSensationFromPath("DialSensation",
                                  {
                                      ("t", anim.t):(0, 0, 0),
                                      ("innerRadius", innerPath.radius) : (0.005, 0, 0),
                                      ("outerRadius", outerPath.radius) : (0.025, 0, 0),
                                      ("rate", inverseInstance.value) : (1, 0, 0),
                                      ("t", cosineWaveInstance.t) : (0, 0, 0)
                                  },
                                  output = anim.out,
                                  intensity = cosineWaveInstance.out,
                                  drawFrequency = 70,
                                  renderMode=sh.RenderMode.Loop
                                  )

setMetaData(dial, "Allow-Transform", True)
setMetaData(dial.innerRadius, "Type", "Scalar")
setMetaData(dial.outerRadius, "Type", "Scalar")
setMetaData(dial.rate, "Type", "Scalar")
